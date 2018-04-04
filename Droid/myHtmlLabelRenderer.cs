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

                // will convert HTML text into attributed string. 
                // does not work with complex HTML code.

                var view = (myHtmlLabel)Element;
                    if (view == null) return;
                // TODO : HTML.FromHTML()... is deprecated 
                    Control.SetText(Html.FromHtml(view.Text.ToString()), TextView.BufferType.Spannable);
            }
        }



    }
}


