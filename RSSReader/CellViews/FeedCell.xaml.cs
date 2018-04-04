using System;
using System.Collections.Generic;
using System.Diagnostics;
using RSSReader.Model;
using Xamarin.Forms;

namespace RSSReader.CellViews
{
    public partial class FeedCell : ViewCell    // FastCell
    {

        public FeedCell()
        {
            InitializeComponent();

            Debug.WriteLine("new cell ");

            //image.Source = null;

            this.Appearing += async (object sender, EventArgs e) => {

                // Called when user scroll the listview, and a cell appear on the screen !

                //await refreshData();
                Debug.WriteLine("re new cell ");

                //image.Source = null;

            };
        }


        //protected override void SetupCell(bool isRecycled)
        //{
        //    var mediaItem = BindingContext as FeedItem;
        //    if (mediaItem != null)
        //    {
        //        title.Text = mediaItem.title ?? "";
        //        description.Text = mediaItem.description ?? "";

        //        //image.Source = mediaItem.imageSource;

        //        //UserThumbnailView.ImageUrl = mediaItem.ImagePath ?? "";
        //        //ImageView.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
        //        //NameLabel.Text = mediaItem.Name;
        //        //DescriptionLabel.Text = mediaItem.Description;
        //    }
        //}


    }
}
