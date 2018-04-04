using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RSSReader.Model
{
    public class FeedItem: Image
    {
   
        public FeedItem()
        {
        }


        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string pubdate { get; set; }
        public string guid { get; set; }
        public string imageURL { get; set;  }
        //public  ImageSource imageSource  { get; set; }

        //public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create<FeedItem, string>(w => w.ImageUrl, null);

        ///// <summary>
        ///// sets the image URL.
        ///// </summary>
        ///// <value>The image URL.</value>
        //public string ImageUrl
        //{
        //    get { return (string)GetValue(ImageUrlProperty); }
        //    set
        //    {
        //        SetValue(ImageUrlProperty, value);
        //    }
        //}


        // TODO :  not really god, as it force me to use Xamarin.Forms which is a graphicval namespace !
        // better idea : save here a byte[] and create a custom control instead of <Image /> in xaml, which will bind to the yte[] and create athe imagesource from it.
        //
        //ImageSource  _imageSource = null;
        //public  ImageSource imageSource { 
        //    get {
        //        return _imageSource;
        //    }
        //    set {
        //        _imageSource = value;
        //    }  
        //}





    }
}
