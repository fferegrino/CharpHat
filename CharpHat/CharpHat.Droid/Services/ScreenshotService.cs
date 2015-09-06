using Android.App;
using Android.Graphics;
using CharpHat.Services;
using System.IO;


// Source: http://danielhindrikes.se/xamarin/building-a-screenshotmanager-to-capture-the-screen-with-code/
[assembly: Xamarin.Forms.Dependency(typeof(CharpHat.Droid.Services.ScreenshotService))]
namespace CharpHat.Droid.Services
{
    public class ScreenshotService : IScreenshotService
    {
        public static Activity Activity { get; set; }

        public byte[] CaptureScreen()
        {
            var view = Activity.Window.DecorView;
            view.DrawingCacheEnabled = true;

            var bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }
    }
}