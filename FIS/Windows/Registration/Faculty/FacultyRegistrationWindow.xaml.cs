using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Reactive.Linq;
using static System.Reactive.Linq.Observable;
using static FirstFloor.ModernUI.Windows.Controls.ModernDialog;
using static FIS.Lib.DatabaseQuery;
using static FIS.Lib.Util;
using static System.Convert;
using static FIS.Lib.Option<string>;
using FIS.Records;
using FIS.Extensions;

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

            var sRegisterFields = from _ in signupButton.StreamClickEvent()
                                  let username = empId_textBox.Text
                                  let password = password_passwordBox.Password
                                  let passwordConfirmation = confirmPassword_passwordBox.Password
                                  let isUsernameEmpty = string.IsNullOrEmpty(username.Trim())
                                  let isPasswordEmpty = string.IsNullOrEmpty(password.Trim())
                                  let isPasswordConfirmationEmpty = string.IsNullOrEmpty(passwordConfirmation)
                                  let isPasswordConfirmationDidNotMatch = !password.Equals(passwordConfirmation)
                                  let errorField = isUsernameEmpty ? Some("Username is required.") :
                                                    isPasswordEmpty ? Some("Password is required.") :
                                                    isPasswordConfirmationEmpty ? Some("Password Confirmation is required.") :
                                                    isPasswordConfirmationDidNotMatch ? Some("Password Confirmation doesn't match.") : None
                                  select new
                                  {
                                      Username = username,
                                      Password = password,
                                      PasswordConfirmation = passwordConfirmation,
                                      Error = errorField
                                  };

            ( from fields in sRegisterFields
              where fields.Error.IsSome
              select fields.Error.Value )
                           .Subscribe(error => ShowMessage(error, string.Empty, MessageBoxButton.OK, this));

            ( from fields in sRegisterFields
              where fields.Error.IsNone
              select fields )
                .Subscribe(fields =>
                {
                    var passwordHash = DataHash(fields.Password);
                    var passwordStringHash = ToBase64String(passwordHash.Item1);
                    var saltString = passwordHash.Item2.ToString("N");

                    AddUserAccount(
                        userAccount: UserAccount.Create(fields.Username, passwordStringHash, saltString),
                        added: added => MessageBox.Show(added? "Account Successfully Registered" : "Account failed to registered"));

                }); /* end click subscription. */
        } /* end signup subscription. */
    }
}