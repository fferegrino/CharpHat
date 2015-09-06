
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CharpHat.Services;
using Java.IO;
using Android.Net;


// Source: http://www.goxuni.com/how-to-save-an-image-to-a-device-using-xuni-and-xamarin-forms/

[assembly: Xamarin.Forms.Dependency(typeof(CharpHat.Droid.Services.PictureManager))]
namespace CharpHat.Droid.Services
{
    public class PictureManager : IPictureManager
    {
        public void SavePictureToDisk(string filename, byte[] imageData)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var pictures = dir.AbsolutePath;
            //adding a time stamp time file name to allow saving more than one image… otherwise it overwrites the previous saved image of the same name
            string name = filename + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            string filePath = System.IO.Path.Combine(pictures, name);
            try
            {
                System.IO.File.WriteAllBytes(filePath, imageData);
                //mediascan adds the saved image into the gallery
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Uri.FromFile(new File(filePath)));
                Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
            }
            catch(System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }

        }
    }
}