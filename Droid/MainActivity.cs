using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using Plugin.HtmlLabel.Android;

namespace RSSReader.Droid
{
    [Activity(Label = "RSSReader.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            // init htmlLabel plugin
            HtmlLabelRenderer.Initialize();
            global::Xamarin.Forms.Forms.Init(this, bundle);


            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
            this.UntrustedCertificateWorkAround();

        }
        private void UntrustedCertificateWorkAround()
        {
            ServicePointManager
            .ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => true;
        }
    }
}
