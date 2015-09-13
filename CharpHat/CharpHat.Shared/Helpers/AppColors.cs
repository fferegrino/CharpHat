using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CharpHat.Helpers
{
    public static class AppColors
    {
        public static Color DarkPurple = Color.FromHex("a46cbc");
        public static Color LightPurple = Color.FromHex("e9d9ee");

		#if __IOS__
		public static UIKit.UIColor ToPlatformColor(this Xamarin.Forms.Color color){
			return UIKit.UIColor.FromRGB (
				(int)(color.R * 255),
				(int)(color.G * 255),
				(int)(color.B *255));
		}
		#endif

    }
}
