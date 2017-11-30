using FIS.Lib;

namespace FIS.Windows.LogIn
{
    /// <summary>
    /// A data class that represents the LogInFields used in LogInReactiveEngine.
    /// </summary>
    struct LogInFields
    {
        internal string Username { get; private set; }
        internal string Password { get; private set; }
        internal Option<string> ErrorField { get; private set; }

        internal LogInFields (string Username, string Password, Option<string> ErrorField)
        {
            this.Username = Username;
            this.Password = Password;
            this.ErrorField = ErrorField;
        }
    }
}
