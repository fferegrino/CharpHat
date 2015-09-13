using System;
using CharpHat.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(CharpHat.iOS.Services.ScreenshotService))]
namespace CharpHat.iOS.Services
{
	public class ScreenshotService : IScreenshotService
	{
		public ScreenshotService ()
		{
		}

		#region IScreenshotService implementation

		public byte[] CaptureScreen ()
		{
			var screenshot = UIScreen.MainScreen.Capture ();
			return screenshot.AsJPEG ().ToArray ();
		}

		#endregion
	}
}

