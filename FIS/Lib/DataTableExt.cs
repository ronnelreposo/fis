using System.Data;
using static System.Diagnostics.Contracts.Contract;

namespace FIS.Lib
{
    /// <summary>
    /// Represents an Extension for DataTable.
    /// </summary>
    internal static class DataTableExt
    {
        internal static bool HasRows (this DataTable dataTable) {
            Requires(dataTable != null);
            return dataTable.Rows.Count > 0;
        } /* end HasRows. */
    } /* end class. */
} /* end namespace. */