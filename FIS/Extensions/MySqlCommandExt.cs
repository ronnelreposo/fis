using MySql.Data.MySqlClient;
using System;
using System.Data;
using static System.Diagnostics.Contracts.Contract;

namespace FIS.Extensions
{
    /// <summary>
    /// Represents A MySqlCommand Extension.
    /// </summary>
    static class MySqlCommandExt
    {
        /// <summary>
        /// Stablishes a Connection in MySqlCommand.
        /// </summary>
        /// <param name="Connection">The Connection to be stablished.</param>
        /// <returns>A new MySqlCommand.</returns>
        internal static MySqlCommand WithConnection (this MySqlCommand command, MySqlConnection Connection)
        {
            Requires(Connection != null, "Connection");

            command.Connection = Connection;
            var connectedCommand = command.Clone();

            Ensures(!command.Equals(connectedCommand));
            Ensures(connectedCommand != null);

            return connectedCommand;
        }
        /// <summary>
        /// Adds a Command Parameter.
        /// </summary>
        /// <param name="ParameterValue">The Parameter Value to be added.</param>
        /// <returns>A new MySqlCommand.</returns>
        internal static MySqlCommand WithParameter (this MySqlCommand command, Tuple<string, object> ParameterValue)
        {
            Requires(ParameterValue != null);
            var parameter = ParameterValue.Item1;
            var value = ParameterValue.Item2;
            Requires(string.IsNullOrEmpty(parameter));
            Requires(value != null);

            command.Parameters.AddWithValue(parameter, value);
            var commandWithParameters = command.Clone();

            Ensures(commandWithParameters != null);
            Ensures(!commandWithParameters.Equals(command));

            return commandWithParameters;
        }
        /// <summary>
        /// Builds a Call Procedure Query Command Text.
        /// </summary>
        /// <param name="Procedure">The Procedure to be called.</param>
        /// <param name="QueryParameter">The Query Parameter.</param>
        /// <returns>A new MySqlCommand.</returns>
        internal static MySqlCommand CallProcedure (this MySqlCommand command, string Procedure, string QueryParameter)
        {
            Requires(string.IsNullOrEmpty(Procedure), "Procedure");
            Requires(string.IsNullOrEmpty(QueryParameter), "QueryParameter");

            command.CommandText = "call " + Procedure + "(" + QueryParameter + ")";
            var calledProcedureCommand = command.Clone();

            Ensures(calledProcedureCommand != null);
            Ensures(calledProcedureCommand.Equals(command));

            return calledProcedureCommand;
        }

        /* extension method of DataTable. (should not be here)*/
        /// <summary>
        /// Fills a DataTable with the following Command.
        /// </summary>
        /// <param name="command">The base Command.</param>
        /// <returns>New Filled New DataTable.</returns>
        internal static DataTable FillWithCommand (this DataTable dataTable, MySqlCommand command)
        {
            Requires(dataTable != null);
            Requires(command != null);
            ( new MySqlDataAdapter(command) ).Fill(dataTable); /*** pass to parameter as extension. */
            return dataTable.Copy();
        }
    }
}
