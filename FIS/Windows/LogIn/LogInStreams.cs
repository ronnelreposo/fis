using FirstFloor.ModernUI.Windows.Controls;
using System;

namespace FIS.Windows.LogIn
{
    /// <summary>
    /// Represents Streams on LogIn
    /// </summary>
    struct LogInStreams
    {
        /// <summary>
        /// Stream of Modern Window on password match.
        /// </summary>
        internal IObservable<ModernWindow> SPasswordMatch { get; private set; }

        /// <summary>
        /// Stream of Modern Window on password does not match.
        /// </summary>
        internal IObservable<ModernWindow> SPasswordNotMatch { get; private set; }

        /// <summary>
        /// Stream of Modern Window on Username that has not been registered.
        /// </summary>
        internal IObservable<ModernWindow> SUsernameNotRegistered { get; private set; }

        LogInStreams (
            IObservable<ModernWindow> SPasswordMatch,
            IObservable<ModernWindow> SPasswordNotMatch,
            IObservable<ModernWindow> SUsernameNotRegistered)
        {
            this.SPasswordMatch = SPasswordMatch;
            this.SPasswordNotMatch = SPasswordNotMatch;
            this.SUsernameNotRegistered = SUsernameNotRegistered;
        }

        internal static LogInStreams Create(
            IObservable<ModernWindow> SPasswordMatch,
            IObservable<ModernWindow> SPasswordNotMatch,
            IObservable<ModernWindow> SUsernameNotRegistered) =>
            new LogInStreams(SPasswordMatch, SPasswordNotMatch, SUsernameNotRegistered);
    }
}