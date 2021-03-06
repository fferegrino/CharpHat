﻿using Android.App;
using Android.Graphics;
using Android.Hardware;
using Android.Views;
using Android.Widget;
using CharpHat.Pages;
using System;
using System.Collections.ObjectModel;
//using System.IO;
using Forms = Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Util;
using Android.Hardware.Camera2;
using Android.Media;
using Android.OS;
using CharpHat.Util;
using Java.Util.Concurrent;
using Java.IO;
using System.Collections.Generic;
using Android;
using Java.Lang;
using Android.Hardware.Camera2.Params;
using Java.Util;
using System.Linq;
using CharpHat.Listeners;
using System.IO;
using Java.Nio;
using Android.Renderscripts;

[assembly: Forms.ExportRenderer(typeof(CharpHat.Pages.CameraPage), typeof(CharpHat.Droid.Pages.CameraPageRenderer))]
namespace CharpHat.Droid.Pages
{
    /*
     * Display Camera Stream: http://developer.xamarin.com/recipes/android/other_ux/textureview/display_a_stream_from_the_camera/
     * Camera Rotation: http://stackoverflow.com/questions/3841122/android-camera-preview-is-sideways
     */
    public class CameraPageRenderer : PageRenderer,
        View.IOnClickListener,
        ActivityCompat.IOnRequestPermissionsResultCallback,
        ImageReader.IOnImageAvailableListener
    {

        View view;
        Button takePhotoButton;
        public Activity Activity => Context as Activity;

        protected override void OnElementChanged(ElementChangedEventArgs<Forms.Page> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;


            mSurfaceTextureListener = new TextureViewSurfaceTextureListener(this);
            mStateCallback = new CameraDeviceStateCallback(this);
            CaptureCallback = new CameraCaptureSessionCaptureCallback(this);



            view = Activity.LayoutInflater.Inflate(Resource.Layout.CameraLayout, this, false);
            //view.FindViewById(Resource.Id.picture).SetOnClickListener(this);
            //view.FindViewById(Resource.Id.info).SetOnClickListener(this);
            TextureView = view.FindViewById<TextureView>(Resource.Id.textureView);

            StartBackgroundThread();


            // When the screen is turned off and turned back on, the SurfaceTexture is already
            // available, and "onSurfaceTextureAvailable" will not be called. In that case, we can open
            // a camera and start preview from here (otherwise, we wait until the surface is ready in
            // the SurfaceTextureListener).
            if (TextureView.IsAvailable)
            {
                OpenCamera(TextureView.Width, TextureView.Height);
            }
            else
            {
                TextureView.SurfaceTextureListener = mSurfaceTextureListener;
            }


            takePhotoButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.takePhotoButton);
            takePhotoButton.Click += TakePhotoButtonTapped;

            //switchCameraButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.switchCameraButton);
            //switchCameraButton.Click += SwitchCameraButtonTapped;

            //cancelCameraButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.cancelPhotoButton);
            //cancelCameraButton.Click += CancelCameraButtonTapped;

            //frontCameraIcon = Context.Resources.GetDrawable(Resource.Drawable.ic_camera_front_white_24dp);
            //rearCameraIcon = Context.Resources.GetDrawable(Resource.Drawable.ic_camera_rear_white_24dp);

            //AddView(view);

            AddView(view);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }

        private async void TakePhotoButtonTapped(object sender, EventArgs e)
        {
            TakePicture();
            //await App.Current.MainPage.Navigation.PushAsync(manipulatePic, false);
        }

        private void RequestCameraPermission()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(Activity, Manifest.Permission.Camera))
            {
                new ConfirmationDialog().Show(Activity.FragmentManager, FragmentDialog);
            }
            else
            {
                ActivityCompat.RequestPermissions(Activity, new string[] { Manifest.Permission.Camera },
                        RequestCameraPermissionCode);
            }
        }

        public void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {

            if (requestCode == RequestCameraPermissionCode)
            {
                if (grantResults.Length != 1 || grantResults[0] != Permission.Granted)
                {
                    ErrorDialog.NewInstance(/*Activity.GetString(Resource.String.request_permission)*/ "Permission")
                            .Show(Activity.FragmentManager, FragmentDialog);
                }
            }
            else
            {
                //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        /// <summary>
        /// Sets up member variables related to camera
        /// </summary>
        /// <param name="width">The width of available size for camera preview</param>
        /// <param name="height">The height of available size for camera preview</param>
        private void SetUpCameraOutputs(int width, int height)
        {
            Activity activity = Activity;
            CameraManager manager = (CameraManager)activity.GetSystemService(Service.CameraService);
            try
            {
                foreach (var cameraId in manager.GetCameraIdList())
                {
                    CameraCharacteristics characteristics
                            = manager.GetCameraCharacteristics(cameraId);

                    // We don't use a front facing camera in this sample.
                    var facing = ((Integer)characteristics.Get(CameraCharacteristics.LensFacing))?.IntValue();
                    if (facing != null && facing == (int)LensFacing.Front)
                    {
                        continue;
                    }

                    var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
                    if (map == null)
                    {
                        continue;
                    }

					// For still image captures, we use the largest available size.
					var outputSizes = Arrays.AsList(map.GetOutputSizes((int)ImageFormatType.Jpeg));


					Size largest = (Size)outputSizes[outputSizes.Count / 2];//Collections.Min(outputSizes,new CompareSizesByArea());
                    mImageReader = ImageReader.NewInstance(largest.Width, largest.Height, ImageFormatType.Jpeg, 2);
                    mImageReader.SetOnImageAvailableListener(this, BackgroundHandler);

                    // Find out if we need to swap dimension to get the preview size relative to sensor
                    // coordinate.
                    var displayRotation = activity.WindowManager.DefaultDisplay.Rotation;
                    //noinspection ConstantConditions
                    mSensorOrientation = ((Integer)characteristics.Get(CameraCharacteristics.SensorOrientation)).IntValue();
                    var swappedDimensions = false;
                    switch (displayRotation)
                    {
                        case SurfaceOrientation.Rotation0:
                        case SurfaceOrientation.Rotation180:
                            if (mSensorOrientation == 90 || mSensorOrientation == 270)
                            {
                                swappedDimensions = true;
                            }
                            break;
                        case SurfaceOrientation.Rotation90:
                        case SurfaceOrientation.Rotation270:
                            if (mSensorOrientation == 0 || mSensorOrientation == 180)
                            {
                                swappedDimensions = true;
                            }
                            break;
                    }

                    Point displaySize = new Point();
                    activity.WindowManager.DefaultDisplay.GetSize(displaySize);
                    int rotatedPreviewWidth = width;
                    int rotatedPreviewHeight = height;
                    int maxPreviewWidth = displaySize.X;
                    int maxPreviewHeight = displaySize.Y;

                    if (swappedDimensions)
                    {
                        rotatedPreviewWidth = height;
                        rotatedPreviewHeight = width;
                        maxPreviewWidth = displaySize.Y;
                        maxPreviewHeight = displaySize.X;
                    }

                    if (maxPreviewWidth > MaxPreviewWidth)
                    {
                        maxPreviewWidth = MaxPreviewWidth;
                    }

                    if (maxPreviewHeight > MaxPreviewHeight)
                    {
                        maxPreviewHeight = MaxPreviewHeight;
                    }

                    // Danger, W.R.! Attempting to use too large a preview size could  exceed the camera
                    // bus' bandwidth limitation, resulting in gorgeous previews but the storage of
                    // garbage capture data.
                    mPreviewSize = ChooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))),
                        rotatedPreviewWidth, rotatedPreviewHeight, maxPreviewWidth,
                        maxPreviewHeight, largest);

                    // We fit the aspect ratio of TextureView to the size of preview we picked.
                    //var orientation = Resources.Configuration.Orientation;
                    //if (orientation == Android.Content.Res.Orientation.Landscape)
                    //{
                    //    TextureView.SetAspectRatio(mPreviewSize.Width, mPreviewSize.Height);
                    //}
                    //else
                    //{
                    //    TextureView.SetAspectRatio(mPreviewSize.Height, mPreviewSize.Width);
                    //}

                    // Check if the flash is supported.
                    var available = ((Java.Lang.Boolean)characteristics.Get(CameraCharacteristics.FlashInfoAvailable))?.BooleanValue();
                    mFlashSupported = available == null ? false : available.Value;

                    mCameraId = cameraId;
                    return;
                }
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (NullPointerException e)
            {
                // Currently an NPE is thrown when the Camera2API is used but not supported on the
                // device this code runs.
                ErrorDialog.NewInstance(/*Activity.GetString(Resource.String.camera_error)*/"Error")
                        .Show(Activity.FragmentManager, FragmentDialog);
            }
        }

        /// <summary>
        /// Opens the camera specified by mCameraId
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void OpenCamera(int width, int height)
        {
            if (ActivityCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera)
                    != Permission.Granted)
            {
                RequestCameraPermission();
                return;
            }
            SetUpCameraOutputs(width, height);
            ConfigureTransform(width, height);
            var activity = Activity;
            CameraManager manager = (CameraManager)activity.GetSystemService(Service.CameraService);
            try
            {
                if (!mCameraOpenCloseLock.TryAcquire(2500, TimeUnit.Milliseconds))
                {
                    throw new RuntimeException("Time out waiting to lock camera opening.");
                }
                manager.OpenCamera(mCameraId, mStateCallback, BackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera opening.", e);
            }
        }

        /// <summary>
        /// Closes the current CameraDevice
        /// </summary>
        private void CloseCamera()
        {
            try
            {
                mCameraOpenCloseLock.Acquire();
                if (null != CaptureSession)
                {
                    CaptureSession.Close();
                    CaptureSession = null;
                }
                if (null != CameraDevice)
                {
                    CameraDevice.Close();
                    CameraDevice = null;
                }
                if (null != mImageReader)
                {
                    mImageReader.Close();
                    mImageReader = null;
                }
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera closing.", e);
            }
            finally
            {
                mCameraOpenCloseLock.Release();
            }
        }

        /// <summary>
        /// Starts a background thread and its Handler
        /// </summary>
        private void StartBackgroundThread()
        {
            mBackgroundThread = new HandlerThread("CameraBackground");
            mBackgroundThread.Start();
            BackgroundHandler = new Handler(mBackgroundThread.Looper);
        }

        /// <summary>
        /// Stops the background thread and its Handler
        /// </summary>
        private void StopBackgroundThread()
        {
            mBackgroundThread.QuitSafely();
            try
            {
                mBackgroundThread.Join();
                mBackgroundThread = null;
                BackgroundHandler = null;
            }
            catch (InterruptedException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Creates a new CameraCaptureSession for camera preview
        /// </summary>
        public void CreateCameraPreviewSession()
        {
            try
            {
                SurfaceTexture texture = TextureView.SurfaceTexture;
                if (texture == null)
                    throw new System.Exception($"{nameof(texture)} is null");

                // We configure the size of default buffer to be the size of camera preview we want.
                texture.SetDefaultBufferSize(mPreviewSize.Width, mPreviewSize.Height);

                // This is the output Surface we need to start preview.
                Surface surface = new Surface(texture);

                // We set up a CaptureRequest.Builder with the output Surface.
                PreviewRequestBuilder = CameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
                PreviewRequestBuilder.AddTarget(surface);

                // Here, we create a CameraCaptureSession for camera preview.
                var surfaces = new List<Surface> { surface, mImageReader.Surface };
                CameraDevice.CreateCaptureSession(surfaces,
                    new CameraCaptureSessionStateCallback(this), null);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Configures the necessary Matrix transformation to TextureView.
        /// This method should be called after the camera preview size is determined in
        /// SetUpCameraOutputs and also the size of TextureView is fixed.
        /// </summary>
        /// <param name="viewWidth"></param>
        /// <param name="viewHeight"></param>
        public void ConfigureTransform(int viewWidth, int viewHeight)
        {
            var activity = Activity;
            if (null == TextureView || null == mPreviewSize || null == activity)
            {
                return;
            }
            var rotation = (int)activity.WindowManager.DefaultDisplay.Rotation;
            Matrix matrix = new Matrix();
            RectF viewRect = new RectF(0, 0, viewWidth, viewHeight);
            RectF bufferRect = new RectF(0, 0, mPreviewSize.Height, mPreviewSize.Width);
            float centerX = viewRect.CenterX();
            float centerY = viewRect.CenterY();
            if ((int)SurfaceOrientation.Rotation90 == rotation || (int)SurfaceOrientation.Rotation270 == rotation)
            {
                bufferRect.Offset(centerX - bufferRect.CenterX(), centerY - bufferRect.CenterY());
                matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);
                float scale = System.Math.Max(
                        (float)viewHeight / mPreviewSize.Height,
                        (float)viewWidth / mPreviewSize.Width);
                matrix.PostScale(scale, scale, centerX, centerY);
                matrix.PostRotate(90 * (rotation - 2), centerX, centerY);
            }
            else if ((int)SurfaceOrientation.Rotation180 == rotation)
            {
                matrix.PostRotate(180, centerX, centerY);
            }
            TextureView.SetTransform(matrix);
        }

        /// <summary>
        /// Initiate a still image capture
        /// </summary>
        private void TakePicture()
        {
            LockFocus();
        }

        /// <summary>
        /// Lock the focus as the first step for a still image capture
        /// </summary>
        private void LockFocus()
        {
            try
            {
                // This is how to tell the camera to lock focus.
                PreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger,
                    (int)ControlAFTrigger.Start);
                // Tell #mCaptureCallback to wait for the lock.
                mState = CameraState.WaitingLock;
                CaptureSession.Capture(PreviewRequestBuilder.Build(), CaptureCallback,
                        BackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Run the precapture sequence for capturing a still image. This method should be called when
        /// we get a response in CaptureCallback from LockFocus()
        /// </summary>
        private void RunPrecaptureSequence()
        {
            try
            {
                // This is how to tell the camera to trigger.
                PreviewRequestBuilder.Set(CaptureRequest.ControlAePrecaptureTrigger,
                    (int)ControlAEPrecaptureTrigger.Start);
                // Tell CaptureCallback to wait for the precapture sequence to be set.
                mState = CameraState.WaitingPrecapture;
                CaptureSession.Capture(PreviewRequestBuilder.Build(), CaptureCallback,
                        BackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Capture a still picture. This method should be called when we get a response in
        /// CaptureCallback from both LockFocus()
        /// </summary>
        public void CaptureStillPicture()
        {
            try
            {
                var activity = Activity;
                if (null == activity || null == CameraDevice)
                {
                    return;
                }
                // This is the CaptureRequest.Builder that we use to take a picture.
                CaptureRequest.Builder captureBuilder =
                        CameraDevice.CreateCaptureRequest(CameraTemplate.StillCapture);
                captureBuilder.AddTarget(mImageReader.Surface);

                // Use the same AE and AF modes as the preview.
                captureBuilder.Set(CaptureRequest.ControlAfMode,
                        (int)ControlAFMode.ContinuousPicture);
                SetAutoFlash(captureBuilder);

                // Orientation
                int rotation = (int)activity.WindowManager.DefaultDisplay.Rotation;
                captureBuilder.Set(CaptureRequest.JpegOrientation, GetOrientation(rotation));

                var captureCallback = new CameraCaptureSessionCaptureCallback2(this);

                CaptureSession.StopRepeating();
                CaptureSession.Capture(captureBuilder.Build(), captureCallback, null);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Retrieves the JPEG orientation from the specified screen rotation
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        private int GetOrientation(int rotation)
        {
            // Sensor orientation is 90 for most devices, or 270 for some devices (eg. Nexus 5X)
            // We have to take that into account and rotate JPEG properly.
            // For devices with orientation of 90, we simply return our mapping from ORIENTATIONS.
            // For devices with orientation of 270, we need to rotate the JPEG 180 degrees.
            return (Orientations.Get(rotation) + mSensorOrientation + 270) % 360;
        }

        /// <summary>
        /// Unlock the focus. This method should be called when still image capture sequence is
        /// finished
        /// </summary>
        public void UnlockFocus()
        {
            try
            {
                // Reset the auto-focus trigger
                PreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger,
                    (int)ControlAFTrigger.Cancel);
                SetAutoFlash(PreviewRequestBuilder);
                CaptureSession.Capture(PreviewRequestBuilder.Build(), CaptureCallback,
                    BackgroundHandler);
                // After this, the camera will go back to the normal state of preview.
                mState = CameraState.Preview;
                CaptureSession.SetRepeatingRequest(mPreviewRequest, CaptureCallback,
                        BackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        public void SetAutoFlash(CaptureRequest.Builder requestBuilder)
        {
            if (mFlashSupported)
            {
                requestBuilder.Set(CaptureRequest.ControlAeMode, (int)ControlAEMode.OnAutoFlash);
            }
        }

        #region Camera2 sample code
        /**
		 * Conversion from screen rotation to JPEG orientation.
		 */
        static readonly SparseIntArray Orientations = new SparseIntArray();
        public const int RequestCameraPermissionCode = 1;
        const string FragmentDialog = "dialog";

        static CameraPageRenderer()
        {
            Orientations.Append((int)SurfaceOrientation.Rotation0, 90);
            Orientations.Append((int)SurfaceOrientation.Rotation90, 0);
            Orientations.Append((int)SurfaceOrientation.Rotation180, 270);
            Orientations.Append((int)SurfaceOrientation.Rotation270, 180);
        }

        /// <summary>
        /// Tag for the Log
        /// </summary>
        public new const string Tag = nameof(CameraPageRenderer);

        // Camera states moved to an enum

        /// <summary>
        /// Max preview width that is guaranteed by Camera2 API
        /// </summary>
        const int MaxPreviewWidth = 1920;

        /// <summary>
        /// Max preview height that is guaranteed by Camera2 API
        /// </summary>
        const int MaxPreviewHeight = 1080;

        private TextureViewSurfaceTextureListener mSurfaceTextureListener;

        /// <summary>
        /// Id of the current <see cref="mCameraDevice"/>
        /// </summary>
        string mCameraId;

        /// <summary>
        /// A TextureVuew for camera preview
        /// </summary>
        internal TextureView TextureView { get; set; }

        /// <summary>
        /// A CameraCaptureSession for camera preview
        /// </summary>
        internal CameraCaptureSession CaptureSession { get; set; }

        /// <summary>
        /// A reference to the opened CameraDevice
        /// </summary>
        internal CameraDevice CameraDevice { get; set; }

        /// <summary>
        /// The Size of camera preview
        /// </summary>
        Size mPreviewSize;

        /// <summary>
        /// The CameraDeviceStateCallback is called when CameraDevice changes its state
        /// </summary>
        CameraDeviceStateCallback mStateCallback;

        /// <summary>
        /// An additional thread for running tasks that shouldn't block the UI
        /// </summary>
        HandlerThread mBackgroundThread;

        /// <summary>
        /// A Handler for running tasks in the background
        /// </summary>
        internal Handler BackgroundHandler { get; set; }

        /// <summary>
        /// An ImageReader that handlers still image capture
        /// </summary>
        ImageReader mImageReader;

        /// <summary>
        /// CaptureRequest.Builder for the camera preview
        /// </summary>
        public CaptureRequest.Builder PreviewRequestBuilder;

        /// <summary>
        /// CameraRequest generated by the PreviewRequestBuilder
        /// </summary>
        public CaptureRequest mPreviewRequest;

        /// <summary>
        /// The current state of camera state for taking pictures
        /// </summary>
        public CameraState mState = CameraState.Preview;

        /// <summary>
        /// A Semaphore to prevent the app from exiting before closing the camera
        /// </summary>
        public Semaphore mCameraOpenCloseLock = new Semaphore(1);

        /// <summary>
        /// Whether the current camera device supports Flash or not
        /// </summary>
        bool mFlashSupported;

        /// <summary>
        /// Orientation of the camera sensor
        /// </summary>
        int mSensorOrientation;

        /// <summary>
        /// A CameraCaptureSessionCaptureCallback that handles events related to JPEG capture
        /// </summary>
        public CameraCaptureSessionCaptureCallback CaptureCallback { get; private set; }

        /// <summary>
        /// Shows a Toast on the UI thread
        /// </summary>
        /// <param name="text">The message to show</param>
        public void ShowToast(string text)
        {
            var activity = Activity;
            if (activity != null)
            {
                activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(activity, text, ToastLength.Short).Show();
                });
            }
        }

        /// <summary>
        /// Given choices of Sizes supported by a camera, choose the smallest one that
        /// is at least as large as the respective texture view size, and that is at most as large as the
        /// respective max size, and whose aspect ratio matches with the specified value.If such size
        /// doesn't exist, choose the largest one that is at most as large as the respective max size,
        /// and whose aspect ratio matches with the specified value.
        /// </summary>
        /// <param name="choices">The list of sizes that the camera supports for the intended output class</param>
        /// <param name="textureViewWidth">The width of the texture view relative to sensor coordinate</param>
        /// <param name="textureViewHeight">The height of the texture view relative to sensor coordinate</param>
        /// <param name="maxWidth">The maximum width that can be chosen</param>
        /// <param name="maxHeight">The maximum height that can be chosen</param>
        /// <param name="aspectRatio">The aspect ratio</param>
        /// <returns>The optimal Size, or an arbitrary one if none were big enough</returns>
        private static Size ChooseOptimalSize(Size[] choices, int textureViewWidth,
            int textureViewHeight, int maxWidth, int maxHeight, Size aspectRatio)
        {
            // Collect the supported resolutions that are at least as big as the preview Surface
            List<Size> bigEnough = new List<Size>();
            // Collect the supported resolutions that are smaller than the preview Surface
            List<Size> notBigEnough = new List<Size>();
            int w = aspectRatio.Width;
            int h = aspectRatio.Height;
            foreach (Size option in choices)
            {
                if (option.Width <= maxWidth && option.Height <= maxHeight &&
                        option.Height == option.Width * h / w)
                {
                    if (option.Width >= textureViewWidth &&
                        option.Height >= textureViewHeight)
                    {
                        bigEnough.Add(option);
                    }
                    else
                    {
                        notBigEnough.Add(option);
                    }
                }
            }

            // Pick the smallest of those big enough. If there is no one big enough, pick the
            // largest of those not big enough.
            if (bigEnough.Any())
            {
                return (Size)Collections.Min(bigEnough, new CompareSizesByArea());
            }
            else if (notBigEnough.Any())
            {
                return (Size)Collections.Max(notBigEnough, new CompareSizesByArea());
            }
            else
            {
                Log.Error(Tag, "Couldn't find any suitable preview size");
                return choices[0];
            }
        }
        #endregion


        public async void OnImageAvailable(ImageReader reader)
        {
            var image = reader.AcquireNextImage();
            var plane = image.GetPlanes()[0];
            var buffer = plane.Buffer;
            byte[] paso = new byte[buffer.Remaining()];
            buffer.Get(paso);

            //Bitmap bitmap =await BitmapFactory.DecodeByteArrayAsync(paso, 0, paso.Length);
            

            //byte[] imageBytes = null;
            //using (var imageStream = new MemoryStream())
            //{
            //    await bitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, imageStream);
            //    bitmap.Recycle();
            //    imageBytes = imageStream.ToArray();
            //}
            //image.Close();

            var manipulatePic = (new SvgManipulatePhotoPage(paso));
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(manipulatePic, false);
            });
        }

    }
}
