using Foundation;
using UIKit;

namespace CharpHat.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate // UIApplicationDelegate
	{
		

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			Xamarin.Forms.Forms.Init ();
			LoadApplication (new App ());
		
			return base.FinishedLaunching(application,launchOptions);
		}

	}
}


