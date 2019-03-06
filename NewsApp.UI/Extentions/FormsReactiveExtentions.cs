using System;
using System.Linq;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace NewsApp
{

    public static class FormsReactiveExtentions
    {
        public static IObservable<SwitchCell> OnChangedStream(this SwitchCell cell)
        {
            return Observable.FromEventPattern<ToggledEventArgs>(
                x => cell.OnChanged += x,
                x => cell.OnChanged -= x).Select(_ => cell);
                
        }
    }
}
