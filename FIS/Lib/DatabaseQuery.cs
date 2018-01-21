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
    }
}