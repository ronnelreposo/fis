using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Controls;

namespace FIS.Windows.LogIn
{
    class LogInControls
    {
        internal TextBox UsernameTextBox { get; private set; }
        internal PasswordBox PasswordPasswordBox { get; private set; }
        internal Button RegisterButton { get; private set; }
        internal Button LogInButton { get; private set; }
        internal ModernWindow MainWindow { get; private set; }
        internal ModernWindow LogInWindow { get; private set; }

        LogInControls (
            TextBox UsernameTextBox,
            PasswordBox PasswordPasswordBox,
            Button RegisterButton,
            Button LogInButton,
            ModernWindow MainWindow,
            ModernWindow LogInWindow)
        {
            this.UsernameTextBox = UsernameTextBox;
            this.PasswordPasswordBox = PasswordPasswordBox;
            this.RegisterButton = RegisterButton;
            this.LogInButton = LogInButton;
            this.MainWindow = MainWindow;
            this.LogInWindow = LogInWindow;
        }

        protected internal static LogInControls Create (
            TextBox UsernameTextBox,
            PasswordBox PasswordPasswordBox,
            Button RegisterButton,
            Button LogInButton,
            ModernWindow MainWindow,
            ModernWindow LogInWindow)
            => new LogInControls(
                UsernameTextBox,
                PasswordPasswordBox,
                RegisterButton,
                LogInButton,
                MainWindow,
                LogInWindow);
    }
}
