using Foundation;
using UIKit;
using CharpHat.Helpers;
using NControl.iOS;

namespace CharpHat.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate // UIApplicationDelegate
	{
		

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			UINavigationBar.Appearance.BarTintColor = AppColors.LightPurple.ToPlatformColor ();
			UINavigationBar.Appearance.TintColor = AppColors.DarkPurple.ToPlatformColor ();
			Xamarin.Forms.Forms.Init ();
			NControlViewRenderer.Init ();

			LoadApplication (new App ());
			return base.FinishedLaunching(application,launchOptions);
		}

	}
}


