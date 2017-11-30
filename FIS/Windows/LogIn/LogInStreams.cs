using FirstFloor.ModernUI.Windows.Controls;
using System;

namespace FIS.Windows.LogIn
{
    struct LogInStreams
    {
        internal IObservable<ModernWindow> SPasswordMatch { get; private set; }
        internal IObservable<ModernWindow> SPasswordNotMatch { get; private set; }
        internal IObservable<ModernWindow> SQueryHasNoRows { get; private set; }

        LogInStreams (
            IObservable<ModernWindow> SPasswordMatch,
            IObservable<ModernWindow> SPasswordNotMatch,
            IObservable<ModernWindow> SQueryHasNoRows)
        {
            this.SPasswordMatch = SPasswordMatch;
            this.SPasswordNotMatch = SPasswordNotMatch;
            this.SQueryHasNoRows = SQueryHasNoRows;
        }

        internal static LogInStreams Create(
            IObservable<ModernWindow> SPasswordMatch,
            IObservable<ModernWindow> SPasswordNotMatch,
            IObservable<ModernWindow> SQueryHasNoRows) =>
        new LogInStreams(SPasswordMatch, SPasswordNotMatch, SQueryHasNoRows);
    }
}