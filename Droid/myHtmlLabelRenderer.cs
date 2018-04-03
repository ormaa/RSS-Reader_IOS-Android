using System;
using System.ComponentModel;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using RSSReader;
using RSSReader.Droid;
using Android.Content;

[assembly: ExportRenderer(typeof(myHtmlLabel), typeof(myHtmlLabelRenderer))]

namespace RSSReader.Droid
{
    public class myHtmlLabelRenderer : LabelRenderer
    {
        public myHtmlLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
//                    Control.SetBackgroundColor(global::Android.Graphics.Color.LightGreen);

                    Console.Out.WriteLine("android label : " + Element.Text);

                var view = (myHtmlLabel)Element;
                    if (view == null) return;
                    Control.SetText(Html.FromHtml(view.Text.ToString()), TextView.BufferType.Spannable);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //if (e.PropertyName == Label.TextProperty.PropertyName)
           // {
                //Console.Out.WriteLine("android label : " + Element.Text);
                //Control?.SetText(Html.FromHtml(Element.Text), TextView.BufferType.Spannable);
            //}
        }

    }
}


