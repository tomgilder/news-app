using Plugin.Share;
using Xamarin.Forms;
using Plugin.Share.Abstractions;

namespace NewsApp
{
    public partial class ArticlePage : ContentPage
    {
        public ArticlePage(string uri)
        {
            InitializeComponent();

            WebView.Source = uri;

            var shareItem = new ToolbarItem("Share", "socialShare", Share);
            ToolbarItems.Add(shareItem);
        }

        void Share()
        {
            var url = ((UrlWebViewSource)WebView.Source).Url;
            CrossShare.Current.Share(new ShareMessage { Url = url });
        }
    }
}