using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using RSSReader;
using RSSReader.iOS;
using Foundation;
using System.Xml.Linq;

[assembly: ExportRenderer (typeof(myHtmlLabel), typeof(myHtmlLabelRenderer))]
namespace RSSReader.iOS
{
    public class myHtmlLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                //Control.BackgroundColor = UIColor.FromRGB(204, 153, 255);
                //Control.BorderStyle = UITextBorderStyle.Line;

                // solution working, but render is far away different on android : size, weight of font. is it normal ?
                //
                var attr = new NSAttributedStringDocumentAttributes();
                var nsError = new NSError();
                attr.DocumentType = NSDocumentType.HTML;

                var myHtmlData = NSData.FromString(Element.Text, NSStringEncoding.Unicode);
                this.Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);


                // marche à peu près. quelques charactère accentués ne fonctionnent pas !
                //
                //var view = (myHtmlLabel)Element;
                //if (view == null) return;

                //var attr = new NSAttributedStringDocumentAttributes();
                //var nsError = new NSError();
                //attr.DocumentType = NSDocumentType.HTML;

                //Control.AttributedText = new NSAttributedString(view.Text, attr, ref nsError);
            }

        }
    }
}

