using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CharpHat.Droid.Services;
using NControl.Droid;

namespace CharpHat.Droid
{
    [Activity(Label = "CharpHat", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            LoadApplication(new App());
            Acr.UserDialogs.UserDialogs.Init(this);
            NControlViewRenderer.Init();

            // Screenshot service
            ScreenshotService.Activity = this;
        }
    }
}


