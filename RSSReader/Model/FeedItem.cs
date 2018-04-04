using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RSSReader.Model
{
    public class FeedItem
    {
   
        public FeedItem()
        {
        }


        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string pubdate { get; set; }
        public string guid { get; set; }
        public string image { get; set;  }
        public  ImageSource imageSource  { get; set; }



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
