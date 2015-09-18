using System;
using NGraphics;
using System.IO;

namespace CharpHat.Helpers
{
	public static class AppNGraphics
	{

		#if __ANDROID__
		public const string ResourcePrefix = "CharpHat.Droid.";
		#endif

		#if __IOS__
		public const string ResourcePrefix = "CharpHat.iOS.";
		#endif

		public static Graphic Read (string path)
		{
			using (var s = OpenResource (path)) {
				var r = new SvgReader (new StreamReader (s));
				return r.Graphic;
			}
		}


		public static Stream OpenResource (string path)
		{
			if (string.IsNullOrEmpty (path))
				throw new ArgumentException ("path");
			var ty = typeof(AppNGraphics);
			var assembly = ty.Assembly;
			var resources = assembly.GetManifestResourceNames ();
			return assembly.GetManifestResourceStream (ResourcePrefix+ path);
		}
	}
}

