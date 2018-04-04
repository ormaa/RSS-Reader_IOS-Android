using System;
using System.Collections.Generic;
using System.Diagnostics;
using RSSReader.Model;
using Xamarin.Forms;

namespace RSSReader.CellViews
{
    public partial class FeedCell : FastCell
    {

        public FeedCell()
        {
            InitializeComponent();

            Debug.WriteLine("new cell ");

            //this.Appearing += async (object sender, EventArgs e) => {
            //    //await refreshData();
            //    Debug.WriteLine("new cell ");
            //};
        }

        protected override void InitializeCell()
        {
            //InitializeComponent();
        }

        protected override void SetupCell(bool isRecycled)
        {
            var mediaItem = BindingContext as FeedItem;
            if (mediaItem != null)
            {
                title.Text = mediaItem.title ?? "";
                description.Text = mediaItem.description ?? "";

                //image.Source = mediaItem.imageSource;

                //UserThumbnailView.ImageUrl = mediaItem.ImagePath ?? "";
                //ImageView.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
                //NameLabel.Text = mediaItem.Name;
                //DescriptionLabel.Text = mediaItem.Description;
            }
        }
    }
}
