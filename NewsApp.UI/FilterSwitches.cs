using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using NewsApp.Core;
using Xamarin.Forms;

namespace NewsApp
{
    public class FilterSwitches
    {
        readonly Dictionary<Category, SwitchCell> _categories = new Dictionary<Category, SwitchCell>
        {
            { Category.UK, GetCell("UK") },
            { Category.Technology, GetCell("Technology") }
        };

        public FilterSwitches()
        {
            OnFilterChanged = 
                Observable.Merge(
                    _categories.Select(x => x.Value.OnChangedStream())
                )
                .Select(_ => Unit.Default);
        }

        public IEnumerable<SwitchCell> SwitchCells => _categories.Select(x => x.Value);
        public IObservable<Unit> OnFilterChanged { get; }

        public bool IsEnabled(Category[] categories)
        {
            categories.ThrowIfNull(nameof(categories));

            return categories.Any(x => _categories[x].On);
        }

        static SwitchCell GetCell(string label)
        {
            var cell = new SwitchCell { Text = label, On = true };
            cell.Tapped += (sender, e) => cell.On = !cell.On;
            return cell;
        }
    }
}
