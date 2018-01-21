using FIS.Extensions;
using FIS.Lib;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Windows;
using static FirstFloor.ModernUI.Windows.Controls.ModernDialog;
using static FIS.Lib.DatabaseQuery;
using static FIS.Lib.Option<string>;
using static FIS.Lib.Util;
using static System.Convert;
using static System.Reactive.Linq.Observable;

namespace FIS.Windows.LogIn
{
    /// <summary>
    /// Represents The Reactive Engine for LogInWindow
    /// </summary>
    class LogInReactiveEngine
    {
        /// <summary>
        /// Log In Controls Data.
        /// </summary>
        internal LogInControls LogInControls { get; private set; }

        /// <summary>
        /// Stream of Register Click Event.
        /// </summary>
        IObservable<EventPattern<EventArgs>> sRegisterClickEvent { get; set; }

        /// <summary>
        /// Stream of Log In Click Event.
        /// </summary>
        IObservable<EventPattern<EventArgs>> sLogInClickEvent { get; set; }

        LogInReactiveEngine (LogInControls LogInControls)
        {
            this.LogInControls = LogInControls;
            sRegisterClickEvent = LogInControls.RegisterButton.StreamClickEvent();
            sLogInClickEvent = LogInControls.LogInButton.StreamClickEvent();
        }

        /// <summary>
        /// LogInReactiveEngine Factory Method
        /// </summary> 
        /// <param name="LogInControls">LogInControls</param>
        /// <returns>new LogInReactiveEngine</returns>
        internal static LogInReactiveEngine Create (LogInControls LogInControls) =>
            new LogInReactiveEngine(LogInControls);

        /// <summary>
        /// Register Stream Click Event Mapped to Empty String
        /// </summary>
        /// <returns>IObservable: EmptyString</returns>
        IObservable<string> RegisterStreamClickEventToEmptyString () =>
            from evt in sRegisterClickEvent select string.Empty;

        /// <summary>
        /// Register Stream Click Event Mapped To FacultyRegistrationWindow
        /// </summary>
        /// <returns>IObservable<Window>: FacultyRegistrationWindow</returns>
        IObservable<Window> RegisterStreamClickEventToFacultyRegistrationWindow () =>
            from evt in sRegisterClickEvent select new FacultyRegistrationWindow();

        /// <summary>
        /// SetUp On Closing Stream Event
        /// - Pops up MessageDialog, upon exit confirmation the current Application will shutdown.
        /// - If the user aborts the operation, event cancel is called.
        /// </summary>
        /// <returns>LogInReactiveEngine</returns>
        internal LogInReactiveEngine SetUpOnClosing ()
        {
            ( from evt in FromEventPattern<CancelEventArgs>(LogInControls.LogInWindow, "Closing")
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

            return this;
        }

        /// <summary>
        /// LogIn Stream Click Mapped to LogInFields
        /// </summary>
        /// <returns>LogInFields</returns>
        internal IObservable<LogInFields> LogInStreamClickToLogInFields () =>
            from _ in sLogInClickEvent
            let username = LogInControls.UsernameTextBox.Text.Trim()
            let password = LogInControls.PasswordPasswordBox.Password.Trim()
            let isUsernameEmpty = string.IsNullOrEmpty(username)
            let isPasswordEmpty = string.IsNullOrEmpty(password)
            let errorField = isUsernameEmpty ? Some("Username") : isPasswordEmpty ? Some("Password") : None
            select new LogInFields(Username: username, Password: password, ErrorField: errorField);

        /// <summary>
        /// LogIn Stream Click Mapped to Some Error Field Value
        /// </summary>
        /// <returns>Stream of Error Field Value</returns>
        internal IObservable<string> LogInStreamClickToSomeError () =>
            from fields in LogInStreamClickToLogInFields()
            let errorField = fields.ErrorField
            where errorField.IsSome
            select errorField.Value;

        /// <summary>
        /// Logs In The User Credentials and returns all the Streams
        /// </summary>
        /// <returns>LogInStreams</returns>
        internal IObservable<LogInStreams> LogInToStreams () =>
            from fields in LogInStreamClickToLogInFields()
                    let errorField = fields.ErrorField
                    where errorField.IsNone
                    let sQueryOptionUserAccount = FromAsync(_ => QueryUserAccountAsync(fields.Username))
                    let sQueryOptionUserAccountExist = from queryOptionUserAccount in sQueryOptionUserAccount
                                                       where queryOptionUserAccount.IsSome
                                                       select queryOptionUserAccount
                    let sQueryAccountToPasswordMatch = from queryOptionUserAccount in sQueryOptionUserAccountExist
                                                       let queryUserAccount = queryOptionUserAccount.Value
                                                       let strPasswordHash = encodePasswordHash(fields.Password, queryUserAccount.PasswordSalt)
                                                       let isPasswordHashMatch = strPasswordHash.Equals(queryUserAccount.EncryptedPassword)
                                                       select isPasswordHashMatch
                    let sPasswordMatch = from isPasswordMatch in sQueryAccountToPasswordMatch
                                         where isPasswordMatch
                                         select new MainWindow()
                    let sPasswordNotMatch = from isPasswordMatch in sQueryAccountToPasswordMatch
                                            where !isPasswordMatch
                                            select LogInControls.LogInWindow
                    let sUsernameNotRegistered = from queryOptionUserAccount in sQueryOptionUserAccount
                                                 where queryOptionUserAccount.IsNone
                                                 select LogInControls.LogInWindow
                    select LogInStreams.Create(
                        SPasswordMatch: sPasswordMatch,
                        SPasswordNotMatch: sPasswordNotMatch,
                        SUsernameNotRegistered: sUsernameNotRegistered);

        /// <summary>
        /// On Registration Set Up
        /// </summary>
        /// <returns>LogInReactiveEngine</returns>
        internal LogInReactiveEngine SetUpOnRegistration()
        {
            var sRegisterToEmptyString = RegisterStreamClickEventToEmptyString();
            sRegisterToEmptyString.Subscribe(emptyStr => LogInControls.UsernameTextBox.Text = emptyStr);
            sRegisterToEmptyString.Subscribe(emptyStr => LogInControls.PasswordPasswordBox.Password = emptyStr);

            var sRegisterToRegistrationWindow = RegisterStreamClickEventToFacultyRegistrationWindow();
            sRegisterToRegistrationWindow.Subscribe(window => window.ShowDialog());

            return this;
        }

        /// <summary>
        /// OnLogInFieldError Set Up
        /// </summary>
        /// <returns>LogInReactiveEngine</returns>
        internal LogInReactiveEngine SetUpOnLogInFieldError()
        {
            LogInStreamClickToSomeError()
                .Subscribe(error => ShowMessage($"{ error } is required.", "Required Field", MessageBoxButton.OK, LogInControls.LogInWindow));

            return this;
        }

        /// <summary>
        /// OnLogIn Set Up
        /// </summary>
        /// <returns>LogInReactiveEngine</returns>
        internal LogInReactiveEngine SetUpOnLogIn()
        {
            LogInToStreams().Subscribe(streams =>
            {
                /* When password matches. */
                streams.SPasswordMatch
                .Subscribe(mainWindow_ =>
                    HideAndShow(LogInControls.LogInWindow)(new MainWindow()));

                /* When password does not match. */
                streams.SPasswordNotMatch
                .Subscribe(@this =>
                    ShowMessage(text: "Your password is incorrect",
                        title: "Incorrect Password",
                        button: MessageBoxButton.OK,
                        owner: @this));

                /* When Username is not registered. */
                streams.SUsernameNotRegistered
                .Subscribe(@this =>
                    ShowMessage(text: "Username is not registered.",
                        title: "unregistered username",
                        button: MessageBoxButton.OK,
                        owner: @this));

            }); /* end logInStream subscription. */

            return this;
        } /* end SetUpOnLogIn */

        /// <summary>
        /// Facade for encoding password hash.
        /// </summary>
        /// <param name="password">The given plain password.</param>
        /// <param name="passwordSalt">the given password salt.</param>
        /// <returns>Encoded password hash.</returns>
        string encodePasswordHash(string password, string passwordSalt)
        {
            var salt = new Guid(passwordSalt);
            var passwordHash = DataHash(data: password, salt: salt);
            var strPasswordHash = ToBase64String(inArray: passwordHash.Item1);

            return strPasswordHash;
        }
    }
}