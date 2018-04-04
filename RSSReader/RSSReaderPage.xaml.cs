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
            Title = "ORMAA RSS Feeds";
            BindingContext = RSSFeedViewModelObject;


            // subscribe to event fired when download of RSS Feed is completed.

            //MessagingCenter.Subscribe<Application>(this, "stopActivity", (sender) => {

            //    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            //    {
            //         hideWaitAnimation();
            //    });
            //});

            //// subscribe to event fired when download of RSS Feed is started.

            //MessagingCenter.Subscribe<Application>(this, "startActivity", (sender) => {

            //    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            //    {
            //        showWaitAnimation();
            //    });
            //});

            // start the first refresh of the listview
            FeedListView.IsRefreshing = true;
            FeedListView.BeginRefresh();

            // add select item event
            FeedListView.ItemSelected += (sender, e) => {
                //((ListView)sender).SelectedItem = null;

                var item = ((ListView)sender).SelectedItem;
                var feed = (FeedItem)item;

                // Create a webview
                //var browser = new WebView
                //{
                //    Source = feed.guid
                //};
                //webview.Children.Add(browser);
                webview.Source = feed.guid;
                //webview.Scale = 0.9;

                web.IsVisible = true;

            };
        }

        // close the web view button click
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            //webview.Children.Clear();
            web.IsVisible = false;
            webview.Source = "";
        }


        //void OnSelection(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (e.SelectedItem == null)
        //    {
        //        return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
        //    }
        //    DisplayAlert("Item Selected", e.SelectedItem.ToString(), "Ok");
        //    //((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        //}



        //// hide the activityIndicator = wait animation
        ////
        //public  async void hideWaitAnimation()
        //{
        //    await Task.Yield();

        //    //do
        //    //{
        //    //    Task.Delay(100).Wait();
        //    //}
        //    //while (FeedListView.IsRefreshing);

        //    // hide the activityIndicator.
        //    activity.IsRunning = false;
        //    activity.IsVisible = false;
        //    activityGrid.IsVisible = false;
        //}

        //// show the activityIndicator = wait animation
        ////
        //public async void showWaitAnimation()
        //{
        //    await Task.Yield();
        //    activity.IsRunning = true;
        //    activity.IsVisible = true;
        //    activityGrid.IsVisible = true;
        //}


    }
}
