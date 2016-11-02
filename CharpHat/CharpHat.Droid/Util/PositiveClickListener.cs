using System;
using Android.Content;
//using Android.Support.V13.App;
using Android;
using CharpHat.Droid.Pages;
using Android.Support.V4.App;

namespace CharpHat.Util
{
    internal class PositiveClickListener : Java.Lang.Object, IDialogInterfaceOnClickListener
    {
        private ConfirmationDialog confirmationDialog;

        public PositiveClickListener(ConfirmationDialog confirmationDialog)
        {
            this.confirmationDialog = confirmationDialog;
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            ActivityCompat.RequestPermissions(confirmationDialog.Activity,
                new[] { Manifest.Permission.Camera }, 
                CameraPageRenderer.RequestCameraPermissionCode);
        }
    }
}