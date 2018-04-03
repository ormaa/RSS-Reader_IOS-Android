using System.Collections.Generic;
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

            RSSFeedViewModelObject = new RSSFeedViewModel(Navigation);
            Title = "RSS Feeds";
            BindingContext = RSSFeedViewModelObject;

            // subscribe to event fired when download of RSS Feed is completed.
            MessagingCenter.Subscribe<Application>(this, "stopActivity", (sender) => {
                // hide the activityIndicator.
                activity.IsRunning = false;
                activity.IsVisible = false;
            });

        }

       


    }
}
