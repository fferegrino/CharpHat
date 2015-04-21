using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using CharpHat.Droid.Views;
using Android.Hardware;
using CharpHat.Droid.Media;

namespace CharpHat.Droid.Activities
{
    [Activity(Label = "CharpHat", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //const int REQUEST_IMAGE_CAPTURE = 1;
        Button ActionButton;
        FrameLayout FrameCameraPreview;

        private Camera mCamera;
        private CameraPreview mPreview;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            ActionButton = FindViewById<Button>(Resource.Id.ActionButton);
            FrameCameraPreview = FindViewById<FrameLayout>(Resource.Id.FrameCameraPreview);
            mCamera = CameraHelpers.GetCameraInstance();
            mPreview = new CameraPreview(this, mCamera);
            FrameCameraPreview.AddView(mPreview);
            //ActionButton.Click += (s, a) => { DispatchPictureIntent(); };
        }

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == Result.Ok)
        //    {
        //        var extras = data.Extras;
        //        var image = extras.Get("data") as Bitmap;
        //        ImageContainer.SetImageBitmap(image);
        //    }
        //}

        //private void DispatchPictureIntent()
        //{
        //    Intent takePictureIntent = new Intent(MediaStore.ActionImageCapture);
        //    if (takePictureIntent.ResolveActivity(PackageManager) != null)
        //    {
        //        StartActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
        //    }
        //}

    }
}

