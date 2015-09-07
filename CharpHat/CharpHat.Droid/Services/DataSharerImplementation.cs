using CharpHat.Services;
using Android.Content;
using Android.Net;

[assembly: Xamarin.Forms.Dependency(typeof(CharpHat.Droid.Services.DataSharerImplementation))]
namespace CharpHat.Droid.Services
{
	public class DataSharerImplementation : IDataSharer
	{
		public DataSharerImplementation ()
		{
			
		}


		#region IDataSharer implementation
		public void ShareImage (string routeToImage, string sharerTitle)
		{
			var ctx = Android.App.Application.Context;

			Intent shareIntent = new Intent(Intent.ActionSend);
			shareIntent.AddFlags(ActivityFlags.NewTask);
			shareIntent.SetType("image/*");
			shareIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
			var file = Uri.FromFile (new Java.IO.File (routeToImage)).ToString ();
			shareIntent.PutExtra(Intent.ExtraStream, file);
			
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat) {
				// This will open the "Complete action with" dialog if the user doesn't have a default app set.
				ctx.StartActivity(shareIntent);

			} else {
				var chooser = Intent.CreateChooser (shareIntent, sharerTitle);
				chooser.AddFlags(ActivityFlags.NewTask);
				ctx.StartActivity(Intent.CreateChooser(chooser, "Share Via"));
			}

		}
		#endregion
	}
}

