using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using RSSReader.Model;
using RSSReader.ViewModel;
using Xamarin.Forms;

namespace RSSReader
{
    public partial class RSSReaderPage : ContentPage
    {
        RSSFeedViewModel RSSFeedViewModelObject;
     
        public RSSReaderPage()
        {
            InitializeComponent();

            Debug.WriteLine("Starting viewModel");
            RSSFeedViewModelObject = new RSSFeedViewModel(Navigation);
            Title = "RSS Feeds";
            BindingContext = RSSFeedViewModelObject;

            // subscribe to event fired when download of RSS Feed is completed.

            MessagingCenter.Subscribe<Application>(this, "stopActivity", (sender) => {

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                     hideWaitAnimation();
                });
            });

            // subscribe to event fired when download of RSS Feed is started.

            MessagingCenter.Subscribe<Application>(this, "startActivity", (sender) => {

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    showWaitAnimation();
                });
            });

            // start the first refresh of the listview
            FeedListView.IsRefreshing = true;
            FeedListView.BeginRefresh();
        }

        // hide the activityIndicator = wait animation
        //
        public  async void hideWaitAnimation()
        {
            await Task.Yield();

            //do
            //{
            //    Task.Delay(100).Wait();
            //}
            //while (FeedListView.IsRefreshing);

            // hide the activityIndicator.
            activity.IsRunning = false;
            activity.IsVisible = false;
            activityGrid.IsVisible = false;
        }

        // show the activityIndicator = wait animation
        //
        public async void showWaitAnimation()
        {
            await Task.Yield();
            activity.IsRunning = true;
            activity.IsVisible = true;
            activityGrid.IsVisible = true;
        }


    }
}
