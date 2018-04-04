using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using RSSReader.Model;
using Xamarin.Forms;

namespace RSSReader.Parser
{

    public static class Singleton {
        //public static List<ImageHTML> Images = new List<ImageHTML>();

        public static List<FeedItem> feeds = new List<FeedItem>();

    }

    //public  class ImageHTML
    //{
    //    public  string imageName = "";
    //    public ImageSource imageSource = null;
    //}



    public class FeedItemParser
    {

        public FeedItemParser()
        {
        }

        public List<FeedItem> ParseFeed(string response)
        {
            if (response == null)
            {
                return null;
            }

            Debug.WriteLine("RSS Feed received. parsing values");

            // TODO : this could crash, if the response is not complete, or if response is ean error instead of xml, or if response is json ??? (does RSS feed exist in json ???)
            // catch this ? to be confirmed : what is the best way need to be used

            XDocument doc = XDocument.Parse(response);

            foreach (var item in doc.Descendants("item"))
            {
                FeedItem feed = new FeedItem();

                // get some feed values. not really useful in this application

                feed.link = item.Element("link").Value.ToString();
                feed.pubdate = item.Element("pubDate").Value.ToString();
                feed.guid = item.Element("guid").Value.ToString();
                feed.image = "";        // by default, there is no image

                // title is html. in this case, we need to use something like HTML Label
                feed.title = item.Element("title").Value.ToString();    // Regex.Unescape(item.Element("title").Value.ToString());

                // Desvcription is html also
                string str = item.Element("description").Value.ToString();

                // check if description contains an image. if yes, we will display the first one, as thumbnail of the Feed list
                if (str.Contains("<img"))
                {
                    // the description contain at least one image :  we will get the first one as description thumbnail
                    // TODO : the problem here is that the image could be an advertising !. 
                    // what to do to manage it better ???

                    var desc = str;
                    var index = desc.IndexOf("<img src=");
                    if ( index != -1 ) {
                        var index2 = desc.IndexOf(".jpg", index);
                        if ( index2 != -1 && index2 > index )
                        {
                            // TODO : this is dangerous, because the length of desc has to be chacked before doing that.
                            // 
                            var img = desc.Substring(index + 10, index2 - index - 6);
                            feed.image = img;  
                        }
                    }
                }


                // Remove the tags <img ... /> : I don't want to have image in the description, it slow too much the HTML label.

                bool b = true;
                do {
                    b = str.Contains("<img");
                    if (b)
                    {
                        var index = str.IndexOf("<img");
                        if (index != -1)
                        {
                            var index2 = str.IndexOf("/>", index);
                            if ( index2 != -1 )
                            {
                                string str2 = str.Substring(0, index) + str.Substring(index2 + 2, str.Length - index2 -2 );
                                str = str2;
                            }
                            else {
                                var index3 = str.IndexOf("/img>", index);
                                if (index3 != -1)
                                {
                                    string str2 = str.Substring(0, index) + str.Substring(index3 + 5, str.Length - index2 - 5);
                                    str = str2;
                                }
                            }
                        }

                        // str
                    }
                } while ( b );

                // Get only 256 characters max for the description. on IOS, if html is too long, trhere is SEVER performance issue in the HTML label !!!!!!
                // TODO : we will lose the end of the rss item html code. is it really bad ? or is it working ?
                var max = 256;
                if ( str.Length < max ) {
                    max = str.Length;
                }
                feed.description = str.Substring(0, max);

                
                Singleton.feeds.Add(feed);
            }

            Debug.WriteLine("Feed parsed properly");
            Debug.WriteLine("Nb lines of feeds : " + Singleton.feeds.Count.ToString());

            Debug.WriteLine("Starting to load images in async task at : " + new DateTime().ToString());

            Task.Run(async () => { await LoadImages(); }); // Singleton.feeds ); });

            Debug.WriteLine("leaving parse Feed method at  : " + new DateTime().ToString());

            return Singleton.feeds;
        }


                    //Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(new Uri(name)));
                    //_imageSource = result.Result;


        public async Task LoadImages() //List<FeedItem> feeds) 
        {
            Debug.WriteLine("Loading images");

            //            foreach (var feed in Singleton.feeds)
            for (int index = 0; index < Singleton.feeds.Count; index++)
            {
                string name = Singleton.feeds[index].image;

                if (name != null && name != "" )
                {
                    Debug.WriteLine(name);

                    // load the imagen into a memorystream
                    MemoryStream strm = await GetImageAsync(name);

                    var array = strm.ToArray();
                    Debug.WriteLine("Stream size : " + strm.Length.ToString());

                    // reduce image size
                    var bytes = DependencyService.Get<IMediaService>().ResizeImage(array, 128, 128);

                    if ( bytes != null && bytes.Length > 0 ) {
                        
                        Debug.WriteLine("image reduced bytes size : " + bytes.Length.ToString());

                        // convert byte[] into an imagesource
                        var stream1 = new MemoryStream(bytes);
                        ImageSource source = ImageSource.FromStream(() => stream1);

                        //// create imageHTML object
                        //ImageHTML imgHtml = new ImageHTML();
                        //imgHtml.imageName = name;
                        //imgHtml.imageSource = source;
                        //Singleton.Images.Add(imgHtml);

                        // 
                        // Binding will send the update event to the UI ??? not here. view model is not proper then.
                        Singleton.feeds[index].imageSource = source;
                    }
                    else {
                        Debug.WriteLine("resized image returned null : " + name);

                    }
                }
                else {
                    Debug.WriteLine("Feed has an image name = null");
                }
            }

            Debug.WriteLine("Loading images Completed");
        }
      

        // load one image, in async mode
        // return a byte[]
        //
        public async Task<MemoryStream> GetImageAsync(string url)
        {
            var tcs = new TaskCompletionSource<MemoryStream>();

            // Get image from web url
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
                .ContinueWith(task =>
                {
                    // image is received
                    Debug.WriteLine("Image received : " + url);

                    var webResponse = (HttpWebResponse)task.Result;
                    Stream responseStream = webResponse.GetResponseStream();
                //    ImageSource img = null;
                //    if (responseStream != null)  img = ImageSource.FromStream( () => responseStream );
                //    else {
                //       Debug.WriteLine("Image null ");
                //   }

                    MemoryStream memoryStream = new MemoryStream();
                    responseStream.CopyTo(memoryStream);

                    tcs.TrySetResult(memoryStream ); 

                    webResponse.Dispose();
                    responseStream.Dispose(); 
                });

            return tcs.Task.Result;
        }





        //public static string UnescapeCodes(string src)
        //{
        //    var rx = new Regex("\\\\([0-9A-Fa-f]+)");
        //    var res = new StringBuilder();
        //    var pos = 0;
        //    foreach (Match m in rx.Matches(src))
        //    {
        //        res.Append(src.Substring(pos, m.Index - pos));
        //        pos = m.Index + m.Length;
        //        res.Append((char)Convert.ToInt32(m.Groups[1].ToString(), 16));
        //    }
        //    res.Append(src.Substring(pos));
        //    return res.ToString();
        //}


    }
}
