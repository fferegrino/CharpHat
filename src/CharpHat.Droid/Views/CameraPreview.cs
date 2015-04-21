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
using Android.Hardware;
using Java.IO;

namespace CharpHat.Droid.Views
{
    public class CameraPreview : SurfaceView, ISurfaceHolderCallback
    {
        private ISurfaceHolder mHolder;
        private Camera mCamera;

        public CameraPreview(Context context, Camera camera) 
            : base(context)
        {
            mCamera = camera;

            // Install a SurfaceHolder.Callback so we get notified when the
            // underlying surface is created and destroyed.

            mHolder = Holder;
            mHolder.AddCallback(this);
            // deprecated setting, but required on Android versions prior to 3.0
            mHolder.SetType(SurfaceType.PushBuffers);
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {

            // If your preview can change or rotate, take care of those events here.
            // Make sure to stop the preview before resizing or reformatting it.

            if (mHolder.Surface == null)
            {
                // preview surface does not exist
                return;
            }

            // stop preview before making changes
            try
            {
                mCamera.StopPreview();
            }
            catch (Exception e)
            {
                // ignore: tried to stop a non-existent preview
            }

            // set preview size and make any resize, rotate or
            // reformatting changes here

            // start preview with new settings
            try
            {
                mCamera.SetPreviewDisplay(mHolder);
                mCamera.StartPreview();

            }
            catch (Exception e)
            {
                //Log.d(TAG, "Error starting camera preview: " + e.getMessage());
            }

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            // The Surface has been created, now tell the camera where to draw the preview.
            try
            {
                mCamera.SetPreviewDisplay(holder);
                mCamera.StartPreview();
            }
            catch (IOException e)
            {
                //Log.d(TAG, "Error setting camera preview: " + e.getMessage());
            }

        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            throw new NotImplementedException();
        }
    }
}