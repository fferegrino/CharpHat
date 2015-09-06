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

namespace CharpHat.Droid
{
	[Activity (Label = "CharpHat", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FormsApplicationActivity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            LoadApplication(new App());

            // Screenshot service
            ScreenshotService.Activity = this;
		}
	}
}


