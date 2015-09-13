using System;

using CharpHat.Services;
using UIKit;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(CharpHat.iOS.Services.PictureManager))]
namespace CharpHat.iOS.Services
{
	public class PictureManager : IPictureManager
	{
		public PictureManager ()
		{
		}

		#region IPictureManager implementation

		public string SavePictureToDisk (string filename, string folder, byte[] imageData)
		{
			var chartImage = new UIImage(NSData.FromArray(imageData));
			chartImage.SaveToPhotosAlbum((image, error) =>
				{
					//you can retrieve the saved UI Image as well if needed using
					var i = image as UIImage;

					if(error != null)
					{
						Console.WriteLine(error.ToString());
					}
				});
			return String.Empty;
		}

		#endregion
	}
}

