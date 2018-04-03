using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RSSReader.Model
{
    public class FeedItem
    {
   
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string pubdate { get; set; }
        public string guid { get; set; }
        public string image { get; set;  }


        ImageSource  _imageSource;

        public  ImageSource imageSource { 
            get {


               // Task.Run(async () => { await getImg(image); });
                if ( image != null && image != ""  && _imageSource == null) {
                    Debug.WriteLine(image);
                    Task<ImageSource> result = Task<ImageSource>.Factory.StartNew( () => ImageSource.FromUri(new Uri(image)));
                    //_imageSource = result.Result;
                }

                //ImageSource src = ImageSource.FromUri( new Uri( image ) );
                //Image img = new Image { Source = image };
                //img.Scale = 0.5;

                //Debug.WriteLine("image");
                //Debug.WriteLine(img.Width);
                //Debug.WriteLine(img.Height);

               // DependencyService.Get<IMediaService>().ResizeImage(src, 128, 128);

                return _imageSource;
            }
            //set {
                
            //}  
        }

        //async void getImg(string name) 
        //{

        //    //Task.Run(async () => { 
        //    //    ImageSource src =  ImageSource.FromUri(new Uri(name)); 
        //    //});

        //    System.Uri uri;
        //    //System.Uri.TryCreate(new Uri(name), UriKind.Absolute, out uri);
        //    Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(new Uri(name)));
        //    //_companyImage.Source = await result;

        //}


        public FeedItem()
        {
        }



    }
}
