using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Controls;

namespace FIS.Windows.LogIn
{
    /// <summary>
    /// A data class that represents the controls used in LogIn.
    /// </summary>
    class LogInControls
    {
        internal TextBox UsernameTextBox { get; private set; }
        internal PasswordBox PasswordPasswordBox { get; private set; }
        internal Button RegisterButton { get; private set; }
        internal Button LogInButton { get; private set; }
        internal ModernWindow LogInWindow { get; private set; }

        LogInControls (
            TextBox UsernameTextBox,
            PasswordBox PasswordPasswordBox,
            Button RegisterButton,
            Button LogInButton,
            ModernWindow LogInWindow)
        {
            this.UsernameTextBox = UsernameTextBox;
            this.PasswordPasswordBox = PasswordPasswordBox;
            this.RegisterButton = RegisterButton;
            this.LogInButton = LogInButton;
            this.LogInWindow = LogInWindow;
        }

        protected internal static LogInControls Create (
            TextBox UsernameTextBox,
            PasswordBox PasswordPasswordBox,
            Button RegisterButton,
            Button LogInButton,
            ModernWindow LogInWindow)
            => new LogInControls(
                UsernameTextBox,
                PasswordPasswordBox,
                RegisterButton,
                LogInButton,
                LogInWindow);
    }
}
