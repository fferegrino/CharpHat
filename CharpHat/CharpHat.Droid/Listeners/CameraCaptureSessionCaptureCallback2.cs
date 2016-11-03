using System;
using Android.Hardware.Camera2;
using Java.Lang;
using Android.Util;
using CharpHat.Droid.Pages;

namespace CharpHat
{
    public class CameraCaptureSessionCaptureCallback2 : CameraCaptureSession.CaptureCallback
    {
        public CameraPageRenderer Parent { get; private set; }

        public CameraCaptureSessionCaptureCallback2(CameraPageRenderer parent)
        {
            Parent = parent;
        }


        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            Parent.UnlockFocus();
        }
    }
}