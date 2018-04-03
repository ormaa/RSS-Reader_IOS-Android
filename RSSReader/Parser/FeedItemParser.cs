﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using RSSReader.Model;
using Xamarin.Forms;

namespace RSSReader.Parser
{
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

            XDocument doc = XDocument.Parse(response);
            List<FeedItem> feeds = new List<FeedItem>();
            foreach (var item in doc.Descendants("item"))
            {
                FeedItem feed = new FeedItem();

                feed.link = item.Element("link").Value.ToString();
                feed.pubdate = item.Element("pubDate").Value.ToString();
                feed.guid = item.Element("guid").Value.ToString();

                // title is html. can contain escaped characters
                feed.title = item.Element("title").Value.ToString(); // Regex.Unescape(item.Element("title").Value.ToString());

                // Desvcription is html also
                string str = item.Element("description").Value.ToString();

                // by default, there is no image
                feed.image = "";
                if (str.Contains("<img"))
                {
                    // the description contain at least one image :  we will get the first one as description thumbnail
                    // TODO : the problem here is that the image could be an advertising !. what to do to manage it better ???
                    var desc = str;
                    var index = desc.IndexOf("<img src=");
                    if ( index != -1 ) {
                        var index2 = desc.IndexOf(".jpg", index);
                        if ( index2 != -1 && index2 > index )
                        {
                            var img = desc.Substring(index + 10, index2 - index - 6);
                            feed.image = img; //item.Element("description").Element("figure").Element("img").Value.ToString();
                        }
                    }
                }

                // Remove the tags <img ... />
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

                feed.description = str;

                
                feeds.Add(feed);
            }

            return feeds;
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
