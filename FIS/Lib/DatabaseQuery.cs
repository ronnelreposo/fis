using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FIS.Extensions;
using static System.Diagnostics.Contracts.Contract;
using static System.Tuple;
using System.Windows;
using FIS.Records;
using System.Reactive.Linq;
using System.Reactive;

namespace FIS.Lib
{
    internal static class DatabaseQuery
    {
        static string ConnectionString { get; } = "host=127.0.0.1; database=pmis; user=root; password=";
        static MySqlConnection Connection { get; } = new MySqlConnection(connectionString: ConnectionString);
        static MySqlCommand ConnectedCommand { get; } = new MySqlCommand().WithConnection(Connection: Connection);

        static MySqlCommand CommandWithParameter (MySqlCommand command, Tuple<string, object> parameter_value) => command.WithParameter(parameter_value);

        internal static async Task<MySqlCommand> QueryAsync (string Procedure, Tuple<string, object> paramValue, Action<Exception> onFail, Action<MySqlCommand> onSuccess)
        {
            Requires(string.IsNullOrEmpty(Procedure));
            Requires(paramValue != null);

            var param = paramValue.Item1;
            var value = paramValue.Item2;

            Requires(string.IsNullOrEmpty(param));
            Requires(value != null);

            var atParam = "@" + param;
            var command = ConnectedCommand
                .CallProcedure(Procedure: Procedure, QueryParameter: atParam)
                .WithParameter(ParameterValue: Create(atParam, value));

            try
            {
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                onSuccess(command);
            }
            catch ( Exception ex ) { onFail(ex); }
            finally { await command.Connection.CloseAsync(); }
            return command;
        }

        internal static async Task<MySqlCommand> QueryAsync (string Procedure, Tuple<string, object> paramValue, Action<Exception> onFail)
        {
            Requires(string.IsNullOrEmpty(Procedure));
            Requires(paramValue != null);

            var param = paramValue.Item1;
            var value = paramValue.Item2;
            Requires(string.IsNullOrEmpty(param));
            Requires(value != null);

            var atParam = "@" + param;
            var command = ConnectedCommand
                .CallProcedure(Procedure: Procedure, QueryParameter: atParam)
                .WithParameter(ParameterValue: Create(atParam, value));

            try
            {
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return command;
            }
            catch ( Exception ex ) { onFail(ex); }
            finally { await command.Connection.CloseAsync(); }
            return command;
        }

        internal static async Task<MySqlCommand> QueryAsync (string procedure, Tuple<string[], object[]> paramValues, Action<Exception> onFail, Action<MySqlCommand> onSuccess)
        {
            Requires(string.IsNullOrEmpty(procedure), "Procedure must not be null or empty.");
            Requires(paramValues != null, "ParamValues must not be null.");

            var param_xs = paramValues.Item1;
            var value_xs = paramValues.Item2;
            Requires(param_xs != null, "param_xs must not be null.");
            Requires(param_xs.Length > 0, "param_xs length must be greater than 0.");
            Requires(value_xs != null, "value_xs must not be null.");
            Requires(value_xs.Length > 0, "value_xs length must be greater than 0.");

            var queryParams = param_xs.Select(x => "@" + x);
            var queryParamAndValues = queryParams.Zip(value_xs, Tuple.Create);
            var queryParam = queryParams.Aggregate((a, b) => a + ", " + b);

            var calledProcedure = ConnectedCommand.CallProcedure(Procedure: procedure, QueryParameter: queryParam);
            var readyCommand = queryParamAndValues.Aggregate(calledProcedure, CommandWithParameter);

            try
            {
                await readyCommand.Connection.OpenAsync();
                await readyCommand.ExecuteNonQueryAsync();
                onSuccess(readyCommand);
            }
            catch ( Exception ex ) { onFail(ex); }
            finally { await readyCommand.Connection.CloseAsync(); }
            return readyCommand;
        }

        internal static Unit AddUserAccount (UserAccount userAccount, Action<bool> added, string procedure = "add_faculty_account")
        {
            ( from command in
                  from u in Observable.FromAsync(_ =>
                    ConnectedCommand.Connection.OpenAsync())
                  select ConnectedCommand
              let idParam = "@id"
              let passwordParam = "@password"
              let saltParam = "@salt"
              let queryParams = $"{ idParam }, { passwordParam }, { saltParam }"
              let commandWithParams = command
                  .CallProcedure(procedure, queryParams)
                  .WithParameter(Create(idParam, userAccount.Username as object))
                  .WithParameter(Create(passwordParam, userAccount.EncryptedPassword as object))
                  .WithParameter(Create(saltParam, userAccount.PasswordSalt as object))
              let sAdded = from n in Observable.FromAsync(_ => commandWithParams.ExecuteNonQueryAsync())
                           select n.Equals(1)
              select new { Command = command, Added = sAdded.Single() } )

            .Subscribe(onNext: obj =>
            {
                Observable.FromAsync(task => obj.Command.Connection.CloseAsync())
                .Subscribe(onNext: _ => { added(obj.Added); }, onError: ex => MessageBox.Show(ex.Message));
            },
            onError: ex => MessageBox.Show(ex.Message));

            return Unit.Default;
        }

        internal static async Task<Option<UserAccount>> QueryUserAccountAsync(string id, string procedure = "get_faculty_account", string queryParameter = "@id")
        {
            var command = ConnectedCommand
                    .CallProcedure(Procedure: procedure, QueryParameter: queryParameter)
                    .WithParameter(Create<string, object>(queryParameter, id));
            try
            {
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                var adapter = new MySqlDataAdapter(command);
                var dt = new DataTable();
                await adapter.FillAsync(dt);

                using ( adapter )
                using ( dt )
                {
                    const byte PasswordIndex = 2;
                    const byte PasswordSaltIndex = 3;

                    return dt.HasRows() ?
                        Option<UserAccount>.Some(
                            UserAccount.Create(
                                dt.Rows[0][1].ToString(),
                                dt.Rows[0][PasswordIndex].ToString(),
                                dt.Rows[0][PasswordSaltIndex].ToString())
                        )
                        : Option<UserAccount>.None;
                }
            }
            catch ( Exception err )
            {
                MessageBox.Show(err.Message);
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    await command.Connection.CloseAsync();
            }

            return Option<UserAccount>.None;
        }
    }
}