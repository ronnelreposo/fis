namespace FIS.Records
{
    /// <summary>
    /// Represents a single User Account
    /// </summary>
    struct UserAccount
    {
        internal string Username { get; private set; }
        internal string EncryptedPassword { get; private set; }
        internal string PasswordSalt { get; private set; }

        UserAccount(string Username, string EncryptedPassword, string PasswordSalt)
        {
            this.Username = Username;
            this.EncryptedPassword = EncryptedPassword;
            this.PasswordSalt = PasswordSalt;
        }

        internal static UserAccount Create(string Username, string EncryptedPassword, string PasswordSalt) =>
            new UserAccount(Username, EncryptedPassword, PasswordSalt);
    }
}
