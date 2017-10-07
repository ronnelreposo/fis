using System;
using System.Reactive;
using System.Windows.Controls;
using static System.Reactive.Linq.Observable;

namespace FIS.Extensions
{
    static class ButtonExt
    {
        internal static IObservable<EventPattern<EventArgs>> StreamClickEvent (this Button button)
            => FromEventPattern(button, "Click");
    } /* end ButtonExt */
}
