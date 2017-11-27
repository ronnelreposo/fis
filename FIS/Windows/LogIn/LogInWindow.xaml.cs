using FirstFloor.ModernUI.Windows.Controls;

namespace FIS.Windows.LogIn
{
    /// <summary>
    /// Interaction logic for ModernWindow1.xaml
    /// </summary>
    public partial class LogInWindow : ModernWindow
    {
        public LogInWindow ()
        {
            InitializeComponent();

            var logInControls = LogInControls.Create(
                    UsernameTextBox: usernameTextBox,
                    PasswordPasswordBox: passwordPasswordBox,
                    RegisterButton: registerButton,
                    LogInButton: loginButton,
                    RegistrationWindow: new FacultyRegistrationWindow(),
                    MainWindow: new MainWindow(),
                    LogInWindow: this);

            LogInReactiveEngine
                .Create(LogInControls: logInControls)
                .SetUpOnClosing()
                .SetUpOnRegistration()
                .SetUpOnLogInFieldError()
                .SetUpOnLogIn();

        } /* end constructor. */
    }
}
