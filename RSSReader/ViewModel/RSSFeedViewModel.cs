using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using RSSReader.Model;
using RSSReader.Network;
using Xamarin.Forms;


namespace RSSReader.ViewModel
{
    public class RSSFeedViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<FeedItem> FeedList
        {
            get => feedList;
            set
            {
                if (feedList != value){
                    feedList = value;
                    OnPropertyChanged("FeedList");   
                }

            }
        }

        private FeedItem selectedItem = null;
        private INavigation Navigation;
        public FeedItem SelectedItem
        {
            get => selectedItem;
            set
            {
                if(selectedItem != value){
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");  
                    OpenWebPage();
                }
            }
        }
        ObservableCollection<FeedItem> feedList = null;
        public event PropertyChangedEventHandler PropertyChanged;





        // constructor
        public RSSFeedViewModel(INavigation navigation)
        {
            this.GetNewsFeedAsync();
            Navigation = navigation;
        }



        // get rss content, async, by webservice call
        public async void GetNewsFeedAsync()
        {
            NetworkManager manager = NetworkManager.Instance;
            List<FeedItem> list = await manager.GetSyncFeedAsync();
            FeedList = new ObservableCollection<FeedItem>(list);

            // rss feed is loaded, can hide the wait animation
            MessagingCenter.Send(Application.Current, "stopActivity");
        }


        // notify a property change event
        protected  void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // click on a cell, display a web page with content details of the RSS news.
        //
        private void OpenWebPage(){
            WebPage page = new WebPage(selectedItem.guid);
            Navigation.PushAsync(page);
        }
    }
}
