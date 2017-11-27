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
    /// Represents THe Reactive Engine for LogInWindow
    /// </summary>
    class LogInReactiveEngine
    {
        internal LogInControls LogInControls { get; private set; }
        IObservable<EventPattern<EventArgs>> registerStreamClickEvent { get; set; }
        IObservable<EventPattern<EventArgs>> logInStreamClickEvent { get; set; }

        LogInReactiveEngine (LogInControls LogInControls)
        {
            this.LogInControls = LogInControls;
            registerStreamClickEvent = LogInControls.RegisterButton.StreamClickEvent();
            logInStreamClickEvent = LogInControls.LogInButton.StreamClickEvent();
        }

        /// <summary>
        /// LogInReactiveEngine Factory Method
        /// </summary> 
        /// <param name="LogInControls">LogInControls</param>
        /// <returns>new LogInReactiveEngine</returns>
        internal static LogInReactiveEngine Create (LogInControls LogInControls)
            => new LogInReactiveEngine(LogInControls);

        /// <summary>
        /// Register Stream Click Event Mapped to Empty String
        /// </summary>
        /// <returns>IObservable: EmptyString</returns>
        IObservable<string> RegisterStreamClickEventToEmptyString () =>
            from evt in registerStreamClickEvent select string.Empty;

        /// <summary>
        /// Register Stream Click Event Mapped To FacultyRegistrationWindow
        /// </summary>
        /// <returns>IObservable<Window>: FacultyRegistrationWindow</returns>
        IObservable<Window> RegisterStreamClickEventToFacultyRegistrationWindow () =>
            from evt in registerStreamClickEvent select new FacultyRegistrationWindow();

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
            from _ in logInStreamClickEvent
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
           let sQueryAccountUsername = FromAsync(task => QueryAsync(Procedure: "get_faculty_account",
                   paramValue: Tuple.Create("username", fields.Username as object),
                   onFail: ex => MessageBox.Show(ex.Message)))
           let sQueryAccountUsernameDataTable = from command in sQueryAccountUsername
                                 select new DataTable().FillWithCommand(command)
           let sQueryAccountUsernameHasRows = from dataTable in sQueryAccountUsernameDataTable
                               where dataTable.HasRows()
                               select dataTable
           let sQueryAccountToPasswordMatch = from dataTable in sQueryAccountUsernameHasRows
                                       let firstRow = dataTable.Rows[0]
                                       let strPasswordHash = encodedPasswordHash(firstRow, fields.Password, SaltColIndex: 3)
                                       let storedPasswordHash = retrievePasswordHash(firstRow, PasswordColIndex: 2)
                                       let isPasswordHashMatch = strPasswordHash.Equals(storedPasswordHash)
                                       select isPasswordHashMatch
           let sPasswordMatch = from isPasswordMatch in sQueryAccountToPasswordMatch
                                where isPasswordMatch
                                select LogInControls.MainWindow
           let sPasswordNotMatch = from isPasswordMatch in sQueryAccountToPasswordMatch
                                   where !isPasswordMatch
                                   select LogInControls.LogInWindow
           let sQueryHasNoRows = from dataTable in sQueryAccountUsernameDataTable
                                 where !dataTable.HasRows()
                                 select LogInControls.LogInWindow
           select LogInStreams.Create(
               SPasswordMatch: sPasswordMatch,
               SPasswordNotMatch: sPasswordNotMatch,
               SQueryHasNoRows: sQueryHasNoRows);

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
            var logInStream = LogInToStreams();

            logInStream.Subscribe(streams =>
                streams.SPasswordMatch
                .Subscribe(mainWindow_ => HideAndShow(LogInControls.LogInWindow)(new MainWindow())));

            logInStream.Subscribe(streams =>
                streams.SPasswordNotMatch
                .Subscribe(
                    @this => ShowMessage(text: "Your password is incorrect",
                    title: "Incorrect Password",
                    button: MessageBoxButton.OK,
                    owner: @this)));

            logInStream.Subscribe(streams =>
                streams.SQueryHasNoRows.Subscribe(
                    @this => ShowMessage(text: "Username is not registered.",
                    title: "unregistered username",
                    button: MessageBoxButton.OK,
                    owner: @this)));

            return this;
        }

        /// <summary>
        /// Encodes the given password to Password Hash.
        /// </summary>
        /// <param name="dataRow">The DataRow in which the SaltIndex can be retrieved.</param>
        /// <param name="password">The given Password to be hashed.</param>
        /// <param name="SaltColIndex">The index coloumn of the DataRow in which the salt can be retrieved.</param>
        /// <returns>Encoded: Password Hash</returns>
        string encodedPasswordHash (DataRow dataRow, string password, int SaltColIndex)
        {
            var salt = new Guid(dataRow[SaltColIndex].ToString());
            var passwordHash = DataHash(data: password, salt: salt);
            var strPasswordHash = ToBase64String(inArray: passwordHash.Item1);

            return strPasswordHash;
        }

        /// <summary>
        /// Retrieves the Stored Hash Password in the DataRow given the PasswordColIndex.
        /// </summary>
        /// <param name="dataRow">The given DataRow in which the Password Hash can be found.</param>
        /// <param name="PasswordColIndex">The Coloumn Index of the given Row.</param>
        /// <returns>Retrieved: Password Hash</returns>
        string retrievePasswordHash (DataRow dataRow, int PasswordColIndex)
        {
            var storedPasswordHash = dataRow[PasswordColIndex].ToString();

            return storedPasswordHash;
        }
    }
}