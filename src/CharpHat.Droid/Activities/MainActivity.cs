using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Graphics;

namespace CharpHat.Droid.Activities
{
    [Activity(Label = "CharpHat", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        const int REQUEST_IMAGE_CAPTURE = 1;
        Button ActionButton;
        ImageView ImageContainer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            ActionButton = FindViewById<Button>(Resource.Id.ActionButton);
            ImageContainer = FindViewById<ImageView>(Resource.Id.ImageContainer);

            ActionButton.Click += (s, a) => { DispatchIntent(); };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == Result.Ok)
            {
                var extras = data.Extras;
                var image = extras.Get("data") as Bitmap;
                ImageContainer.SetImageBitmap(image);
            }
        }

        private void DispatchIntent()
        {
            Intent takePictureIntent = new Intent(MediaStore.ActionImageCapture);
            if (takePictureIntent.ResolveActivity(PackageManager) != null)
            {
                StartActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
            }
        }

    }
}

