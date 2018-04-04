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
                feed.imageURL = "";        // by default, there is no image

                // title is html. in this case, we need to use something like HTML Label
                feed.title = HtmlToPlainText(item.Element("title").Value.ToString() ); //.Replace(@"\", string.Empty);    // Regex.Unescape(item.Element("title").Value.ToString());

                // Desvcription is html also
                string str = item.Element("description").Value.ToString();

                // check if description contains an image. if yes, we will display the first one, as thumbnail of the Feed list
                //
                if (str.Contains("<img"))
                {
                    // the description contain at least one image :  we will get the first one as description thumbnail
                    // TODO : the problem here is that the image could be an advertising !. 
                    // what to do to manage it better ???

                    var desc = str;
                    var i = desc.IndexOf("<img src=");
                    if ( i != -1 ) {
                        var index2 = desc.IndexOf(".jpg", i);
                        if ( index2 != -1 && index2 > i )
                        {
                            // TODO : this is dangerous, because the length of desc has to be chacked before doing that.
                            // 
                            var img = desc.Substring(i + 10, index2 - i - 6);
                            feed.imageURL = img;
                            //feed.ImageUrl = img;

                        }
                    }
                }


                // Remove the tags <img ... /> : I don't want to have image in the description, it slow too much the HTML label.

                //bool b = true;
                //do {
                //    b = str.Contains("<img");
                //    if (b)
                //    {
                //        var index = str.IndexOf("<img");
                //        if (index != -1)
                //        {
                //            var index2 = str.IndexOf("/>", index);
                //            if ( index2 != -1 )
                //            {
                //                string str2 = str.Substring(0, index) + str.Substring(index2 + 2, str.Length - index2 -2 );
                //                str = str2;
                //            }
                //            else {
                //                var index3 = str.IndexOf("/img>", index);
                //                if (index3 != -1)
                //                {
                //                    string str2 = str.Substring(0, index) + str.Substring(index3 + 5, str.Length - index2 - 5);
                //                    str = str2;
                //                }
                //            }
                //        }

                //        // str
                //    }
                //} while ( b );

                //// replace url of images by "", for the description
                //bool b = true;
                //int index = 0;
                //do
                //{
                //    index = str.IndexOf("http", index);

                //    if (index > 0)
                //    {
                //            var index2 = str.IndexOf(".jpg", index);
                //            if (index2 > 0)
                //            {
                //                string str2 = str.Substring(0, index) + str.Substring(index2 + 4, str.Length - index2 - 4);
                //                str = str2;
                //            }
                //            else
                //            {
                //                var index3 = str.IndexOf(".jpeg", index);
                //                if (index3 > 0)
                //                {
                //                    string str2 = str.Substring(0, index) + str.Substring(index3 + 5, str.Length - index3 - 5);
                //                    str = str2;
                //                }
                //                else {
                //                    var index4 = str.IndexOf(".png", index);
                //                    if (index4 > 0)
                //                    {
                //                        string str2 = str.Substring(0, index) + str.Substring(index4 + 4 , str.Length - index4 - 4);
                //                        str = str2;
                //                    }
                //                }
                //            }

                //           index += 4;

                //    }

                //} while (index > 0);

                if (str.Contains("Rendre compatible")) {

                    Debug.WriteLine("ok");

                    var text0 = HtmlToPlainText(str);
                }


                // convert html to plain text
                var text = HtmlToPlainText(str);
                // keep a short part of the description string
                var max = 200;
                if ( text.Length < max ) {
                    max = text.Length;
                }
                feed.description = text.Substring(0, max) + " ...";


                Singleton.feeds.Add(feed);
            }

            Debug.WriteLine("Feed parsed properly");
            Debug.WriteLine("Nb lines of feeds : " + Singleton.feeds.Count.ToString());

            Debug.WriteLine("Starting to load images in async task at : " + new DateTime().ToString());

            //Task.Run(async () => { await LoadImages(); }); // Singleton.feeds ); });

            Debug.WriteLine("leaving parse Feed method at  : " + new DateTime().ToString());

            return Singleton.feeds;
        }



        // co,nvert html to plain text, removing the tags
        // https://stackoverflow.com/questions/286813/how-do-you-convert-html-to-plain-text
        //
        private static string HtmlToPlainText(string html)
        {
const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);

            // olivier added : convert \n<p>... into <p>...
            text = tagWhiteSpaceRegex.Replace(text, "<");


            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }



        //Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(new Uri(name)));
        //_imageSource = result.Result;


        /* not used anymore

        public async Task LoadImages() //List<FeedItem> feeds) 
        {
            Debug.WriteLine("Loading images");

            //            foreach (var feed in Singleton.feeds)
            for (int index = 0; index < Singleton.feeds.Count; index++)
            {
                string name = Singleton.feeds[index].imageURL;

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
                        //Singleton.feeds[index].imageSource = source;
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


        */




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
