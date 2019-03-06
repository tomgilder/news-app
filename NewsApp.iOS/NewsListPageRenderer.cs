using NewsApp;
using NewsApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FilterTableView), typeof(FilterTableViewRenderer))]

namespace NewsApp.iOS
{
    public class FilterTableViewRenderer : TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.CornerRadius = 10;
                Control.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            
                // Fix news list not scrolling to top when tapping status bar
                Control.ScrollsToTop = false;
            }
        }
    }
}
