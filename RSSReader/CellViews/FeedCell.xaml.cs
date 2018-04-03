using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace RSSReader.CellViews
{
    public partial class FeedCell : ViewCell
    {

        public FeedCell()
        {
            InitializeComponent();

            //this.Appearing += async (object sender, EventArgs e) => {
            //    //await refreshData();
            //    Debug.WriteLine("new cell ");
            //};
        }
    }
}
