using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Reactive.Linq;
using static System.Reactive.Linq.Observable;
using static System.Tuple;
using static FirstFloor.ModernUI.Windows.Controls.ModernDialog;
using static FIS.Lib.DatabaseQuery;
using static FIS.Lib.Util;
using static System.Convert;
using static FIS.Lib.Option<string>;

namespace FIS
{
    /// <summary>
    /// Interaction logic for FacultyRegistration.xaml
    /// </summary>
    public partial class FacultyRegistrationWindow : ModernWindow
    {
        public FacultyRegistrationWindow ()
        {
            InitializeComponent();

            var sRegisterClick = FromEventPattern(signupButton, "Click");

            var sRegisterFields = from _ in sRegisterClick
                                  let username = empId_textBox.Text
                                  let password = password_passwordBox.Password
                                  let passwordConfirmation = confirmPassword_passwordBox.Password
                                  let isUsernameEmpty = string.IsNullOrEmpty(username.Trim())
                                  let isPasswordEmpty = string.IsNullOrEmpty(password.Trim())
                                  let isPasswordConfirmationEmpty = string.IsNullOrEmpty(passwordConfirmation)
                                  let isPasswordConfirmationMatch = password.Equals(passwordConfirmation)
                                  let errorField = isUsernameEmpty ? Some("Username is required.") :
                                                    isPasswordEmpty ? Some("Password is required.") :
                                                    isPasswordConfirmationEmpty ?  Some("Password Confirmation is required.")  :
                                                    isPasswordConfirmationMatch ? Some("Password Confirmation doesn't match.") : None
                                  select new
                                  {
                                      Username = username,
                                      Password = password,
                                      PasswordConfirmation = passwordConfirmation,
                                      Error = errorField
                                  };

            (from fields in sRegisterFields
                           where fields.Error.IsSome
                           select fields.Error.Value)
                           .Subscribe(error => ShowMessage(error, string.Empty, MessageBoxButton.OK, this));

            (from fields in sRegisterFields
                           where fields.Error.IsNone
                           select fields)
                .Subscribe(async fields =>
                {
                    var passwordHash = DataHash(fields.Password);
                    var passwordStringHash = ToBase64String(passwordHash.Item1);
                    var saltString = passwordHash.Item2.ToString("N");
                    var paramsAndValues = Create(new[] { "id", "password", "salt" },
                        new object[] { fields.Username, passwordStringHash, saltString });

                    await QueryAsync(procedure: "add_faculty_account",
                        paramValues: paramsAndValues,
                        onFail: __ => MessageBox.Show("Failed to Register."),
                        onSuccess: __ => onRegistrationSucceeded(
                            DisplaySuccess: () => ShowMessage(
                                text: "Faculty Registered.",
                                title: "Registration Succeded.",
                                button: MessageBoxButton.OK,
                                owner: this),
                            ClearInformations: () =>
                            {
                                empId_textBox.Clear();
                                password_passwordBox.Clear();
                                confirmPassword_passwordBox.Clear();
                            })); /* end QueryAsync. */
                }); /* end click subscription. */
        } /* end signup subscription. */

        /// <summary>
        /// Serves as the template upon success registration.
        /// First, It takes an action to display the success message.
        /// Second, It takes an action to clear the informations.
        /// </summary>
        /// <param name="DisplaySuccess">Action Display.</param>
        /// <param name="ClearInformations">Action for Clearing Info.</param>
        void onRegistrationSucceeded (Action DisplaySuccess, Action ClearInformations)
        {
            DisplaySuccess();
            ClearInformations();
        }
    }
}