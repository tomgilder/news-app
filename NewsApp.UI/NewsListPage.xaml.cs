using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NewsApp.Core;
using Xamarin.Forms;

namespace NewsApp
{
    public partial class NewsListPage : ContentPage
    {
        static Feed[] _feeds =
        {
            new Feed("http://feeds.bbci.co.uk/news/uk/rss.xml", Source.BBC, new[] { Category.UK }),
            new Feed("http://feeds.bbci.co.uk/news/technology/rss.xml", Source.BBC, new[] { Category.Technology }),
            new Feed("http://feeds.reuters.com/reuters/UKdomesticNews?format=xml", Source.Reuters, new[] { Category.UK }),
            new Feed("http://feeds.reuters.com/reuters/technologyNews?format=xml", Source.Reuters, new[] { Category.Technology })
        };

        readonly TapGestureRecognizer _gestureRecognizer;
        readonly List<FeedItem> _allItems = new List<FeedItem>();
        readonly FilterSwitches _switches = new FilterSwitches();
        readonly FeedParser _feedParser;

        public NewsListPage()
        {
            InitializeComponent();

            _feedParser = new FeedParser(_feeds);
            _feedParser.OnData.Synchronize().Subscribe(items =>
            {
                _allItems.AddRange(items);
                UpdateList();
            });
            _feedParser.Update();

            _switches
                .OnFilterChanged
                .Subscribe(_ =>

                    // Delay by 1ms to force table to update on next
                    // runloop, otherwise UISwitch animation stutters
                    Task.Delay(1).ContinueWith(__ => UpdateList())
                );

            ItemsList.RefreshCommand = RefreshCommand;

            FilterTable.Root = new TableRoot { new TableSection { _switches.SwitchCells } };
            ToolbarItems.Add(new ToolbarItem("Filter", null, OnFilterTapped));

            _gestureRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (FilterTable.IsVisible)
                    {
                        HideFilter();
                    }
                })
            };
        }

        void UpdateList()
        {
            var filteredItems =
                _allItems.Where(item => _switches.IsEnabled(item.Categories))
                         .OrderByDescending(x => x.PubDate)
                         .ToList();

            Device.BeginInvokeOnMainThread(() => ItemsList.ItemsSource = filteredItems);
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ItemsList.SelectedItem = null;

            var item = (FeedItem)e.Item;
            Navigation.PushAsync(new ArticlePage(item.Link));
        }

        void OnFilterTapped()
        {
            if (FilterTable.IsVisible)
            {
                HideFilter();
            }
            else
            {
                ShowFilter();
            }
        }

        void HideFilter()
        {
            ItemsList.IsEnabled = true;
            FilterTable.IsVisible = false;
            ItemsList.FadeTo(1);
            Content.GestureRecognizers.Remove(_gestureRecognizer);
        }

        void ShowFilter()
        {
            ItemsList.IsEnabled = false;
            FilterTable.IsVisible = true;
            ItemsList.FadeTo(.5);
            Content.GestureRecognizers.Add(_gestureRecognizer);
        }

        Command RefreshCommand => new Command(async () =>
        {
            _allItems.Clear();
            await _feedParser.UpdateAsync();
            ItemsList.EndRefresh();
        });
    }
}
