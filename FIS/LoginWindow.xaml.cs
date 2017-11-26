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
using System.Reactive;

namespace FIS
{
    public partial class LoginWindow : ModernWindow
    {
        public LoginWindow ()
        {
            InitializeComponent();

            setUpReactiveEngine();

        } /* end constructor. */

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
        } /* end encodePasswordHash */

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
        } /* end retrievePasswordHash. */

        /// <summary>
        /// SignIn Reactive Engine
        /// </summary>
        /// <param name="username">The given username</param>
        /// <param name="password">The given password</param>
        /// <returns>Unit</returns>
        Unit signin (string username, string password)
        {
            var sQuery = FromAsync(task => QueryAsync(Procedure: "get_faculty_account",
                      paramValue: Create("username", username as object),
                      onFail: ex => MessageBox.Show(ex.Message)));

            var sQueryDataTable = from command in sQuery
                                  select new DataTable().FillWithCommand(command);

            var sQueryHasRows = from dataTable in sQueryDataTable
                                where dataTable.HasRows()
                                select dataTable;

            var sQueryToPasswordMatch = from dataTable in sQueryHasRows
                                        let firstRow = dataTable.Rows[0]
                                        let strPasswordHash = encodedPasswordHash(firstRow, password, SaltColIndex: 3)
                                        let storedPasswordHash = retrievePasswordHash(firstRow, PasswordColIndex: 2)
                                        let isPasswordHashMatch = strPasswordHash.Equals(storedPasswordHash)
                                        select isPasswordHashMatch;

            var sPasswordMatch = from isPasswordMatch in sQueryToPasswordMatch
                                 where isPasswordMatch
                                 select new MainWindow();
            sPasswordMatch.Subscribe(mainWindow => HideAndShow(this)(mainWindow));

            var sPasswordNotMatch = from isPasswordMatch in sQueryToPasswordMatch
                                 where !isPasswordMatch
                                 select this;
            sPasswordNotMatch.Subscribe(@this => ShowMessage(
                text: "Your password is incorrect",
                title: "Incorrect Password",
                button: MessageBoxButton.OK,
                owner: @this));

            var sQueryHasNoRows = from dataTable in sQueryDataTable
                                  where !dataTable.HasRows()
                                  select this;
            sQueryHasNoRows.Subscribe(@this => ShowMessage(text: "Username is not registered.",
                title: "unregistered username",
                button: MessageBoxButton.OK, owner: @this));

            return Unit.Default;
        } /* signin. */

        Unit setUpReactiveEngine()
        {
            var sLoginClick = loginButton.StreamClickEvent();

            var sRegisterClick = registerButton.StreamClickEvent();
            sRegisterClick
                .Do(_ => usernameTextBox.Text = string.Empty)
                .Do(_ => passwordPasswordBox.Password = string.Empty)
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
            .Subscribe(fields => signin(fields.Username, fields.Password));

            return Unit.Default;
        } /* end setUpReactiveEngine. */
    } /* end class. */
} /* end namespace. */