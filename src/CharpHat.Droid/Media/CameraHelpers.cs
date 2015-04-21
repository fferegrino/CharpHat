using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Hardware;

namespace CharpHat.Droid.Media
{
    public class CameraHelpers
    {
        public bool CheckCameraHardware(Context context)
        {
            if (context.PackageManager.HasSystemFeature(PackageManager.FeatureCamera))
            {
                // this device has a camera
                return true;
            }
            else
            {
                // no camera on this device
                return false;
            }

        }

        public static Camera GetCameraInstance()
        {
            Camera c = null;
            try
            {
                c = Camera.Open(); // attempt to get a Camera instance
            }
            catch (Exception e)
            {
                // Camera is not available (in use or does not exist)
            }
            return c; // returns null if camera is unavailable
        }
    }
}