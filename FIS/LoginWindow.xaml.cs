using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using FIS.Extensions;
using FIS.Lib;
using static FirstFloor.ModernUI.Windows.Controls.ModernDialog;
using static System.Convert;
using static System.Tuple;
using static System.Reactive.Linq.Observable;
using static FIS.Lib.Option<string>;
using static FIS.Lib.DatabaseQuery;
using static FIS.Lib.Util;
using System.Reactive.Linq;

namespace FIS
{
    public partial class LoginWindow : ModernWindow
    {
        public LoginWindow ()
        {
            InitializeComponent();

            var sLoginClick = loginButton.StreamClickEvent();

            registerButton
                .StreamClickEvent()
                .Subscribe(_ => new FacultyRegistrationWindow().ShowDialog());

            ( from evt in FromEventPattern<CancelEventArgs>(this, "Closing")
              let messageBoxResult =
              ShowMessage("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButton.YesNo)
              select new { Event = evt, MessageBoxResult = messageBoxResult } )
            .Subscribe(eventAndResult =>
            {
                switch ( eventAndResult.MessageBoxResult )
                {
                    case MessageBoxResult.Yes:
                        Application.Current.Shutdown();
                        break;
                    case MessageBoxResult.Cancel:
                    case MessageBoxResult.None:
                    case MessageBoxResult.OK:
                    case MessageBoxResult.No:
                    default:
                        eventAndResult.Event.EventArgs.Cancel = true;
                        break;
                } /* end pattern match. */
            }); /* end on closing subscription. */

            var sLogInFields = from _ in sLoginClick
                               let username = usernameTextBox.Text.Trim()
                               let password = passwordPasswordBox.Password.Trim()
                               let isUsernameEmpty = string.IsNullOrEmpty(username)
                               let isPasswordEmpty = string.IsNullOrEmpty(password)
                               let errorField = isUsernameEmpty ? Some("Username") : isPasswordEmpty ? Some("Password") : None
                               select new { Username = username, Password = password, ErrorField = errorField };

            ( from fields in sLogInFields
              let errorField = fields.ErrorField
              where errorField.IsSome
              select errorField.Value)
                    .Subscribe(error => ShowMessage($"{ error } is required.", "Required Field", MessageBoxButton.OK, this));

            ( from fields in sLogInFields
              let errorField = fields.ErrorField
              where errorField.IsNone
              select fields )
            .Subscribe(async fields => await QueryAsync(
                      Procedure: "get_faculty_account",
                      paramValue: Create("username", fields.Username as object),
                      onFail: ex => MessageBox.Show(ex.Message),
                      onSuccess: command =>
                      {
                          var filledDataTable = new DataTable().FillWithCommand(command);

                          if ( !filledDataTable.HasRows() )
                          {
                              ShowMessage(
                                  text: "Username is not registered.",
                                  title: "unregistered username",
                                  button: MessageBoxButton.OK,
                                  owner: this);
                              return;
                          }

                          var firstRow = filledDataTable.Rows[0];

                          var SaltRowIndex = 3;
                          var salt = new Guid(firstRow[SaltRowIndex].ToString());

                          var password_hash = DataHash(data: fields.Password, salt: salt);
                          var str_password_hash = ToBase64String(inArray: password_hash.Item1);

                          var PasswordRowIndex = 2;
                          var DBpasswordHash = firstRow[PasswordRowIndex].ToString();

                          var isPasswordHashMatch = str_password_hash.Equals(DBpasswordHash);

                          if ( !isPasswordHashMatch )
                          {
                              ShowMessage(
                                  text: "Your password is incorrect",
                                  title: "Incorrect Password",
                                  button: MessageBoxButton.OK,
                                  owner: this);
                              return;
                          }
                          HideAndShow(this)(new MainWindow());
                      })); /* end subsciption. */
        } /* end constructor. */
    } /* end class. */
} /* end namespace. */