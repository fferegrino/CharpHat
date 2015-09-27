using Android.App;
using Android.Graphics;
using Android.Hardware;
using Android.Views;
using Android.Widget;
using CharpHat.Pages;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CharpHat.Pages.CameraPage), typeof(CharpHat.Droid.Pages.CameraPage))]
namespace CharpHat.Droid.Pages
{
    /*
     * Display Camera Stream: http://developer.xamarin.com/recipes/android/other_ux/textureview/display_a_stream_from_the_camera/
     * Camera Rotation: http://stackoverflow.com/questions/3841122/android-camera-preview-is-sideways
     */
    public class CameraPage : PageRenderer, TextureView.ISurfaceTextureListener
    {
        global::Android.Hardware.Camera camera;
        global::Android.Widget.Button takePhotoButton;
        global::Android.Widget.Button toggleFlashButton;
        global::Android.Widget.Button cancelCameraButton;
        global::Android.Widget.Button switchCameraButton;

        global::Android.Graphics.Drawables.Drawable rearCameraIcon;
        global::Android.Graphics.Drawables.Drawable frontCameraIcon;

        Activity activity;
        CameraFacing cameraType;
        TextureView textureView;
        SurfaceTexture surfaceTexture;
        global::Android.Views.View view;

        decimal aspectRatio;

        byte[] imageBytes;

        public CameraPage()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;

            activity = this.Context as Activity;
            view = activity.LayoutInflater.Inflate(Resource.Layout.CameraLayout, this, false);
            cameraType = CameraFacing.Back;

            textureView = view.FindViewById<TextureView>(Resource.Id.textureView);
            textureView.SurfaceTextureListener = this;

            takePhotoButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.takePhotoButton);
            takePhotoButton.Click += TakePhotoButtonTapped;

            switchCameraButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.switchCameraButton);
            switchCameraButton.Click += SwitchCameraButtonTapped;

            cancelCameraButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.cancelPhotoButton);
            cancelCameraButton.Click += CancelCameraButtonTapped;

            frontCameraIcon = Context.Resources.GetDrawable(Resource.Drawable.ic_camera_front_white_24dp);
            rearCameraIcon = Context.Resources.GetDrawable(Resource.Drawable.ic_camera_rear_white_24dp);

            AddView(view);
        }

        async void CancelCameraButtonTapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }


        bool _isCameraFront;
        void SwitchCameraButtonTapped(object sender, EventArgs e)
        {
            if (cameraType == CameraFacing.Front)
            {
                switchCameraButton.Background = frontCameraIcon;
                cameraType = CameraFacing.Back;
                camera.StopPreview();
                camera.Release();
                camera = global::Android.Hardware.Camera.Open((int)cameraType);
                camera.SetPreviewTexture(surfaceTexture);
                PrepareAndStartCamera();
            }
            else
            {
                switchCameraButton.Background = rearCameraIcon;
                cameraType = CameraFacing.Front;
                camera.StopPreview();
                camera.Release();
                camera = global::Android.Hardware.Camera.Open((int)cameraType);
                camera.SetPreviewTexture(surfaceTexture);
                PrepareAndStartCamera();
            }
        }


        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }

        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            camera = global::Android.Hardware.Camera.Open((int)cameraType);
            textureView.LayoutParameters = new FrameLayout.LayoutParams(width, height);

            aspectRatio = ((decimal)height) / ((decimal)width);
            decimal difference = Decimal.MaxValue;
            var parameters = camera.GetParameters();
            var supportedSizes = parameters.SupportedPreviewSizes;
            Android.Hardware.Camera.Size previewSize = null;
            foreach (var size in supportedSizes)
            {
                decimal current = size.Width / (decimal)size.Height;
                Console.WriteLine(size.Width + "\t" + size.Height);
                if (Math.Abs(current - aspectRatio) < difference)
                {
                    previewSize = size;
                    difference = Math.Abs(current - aspectRatio);
                }
            }

            parameters.SetPreviewSize(previewSize.Width, previewSize.Height);
            camera.SetParameters(parameters);

            surfaceTexture = surface;

            camera.SetPreviewTexture(surface);
            PrepareAndStartCamera();

        }

        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
        {
            return TurnOffCamera();
        }

        private bool TurnOffCamera()
        {
            camera.StopPreview();
            camera.Release();
            return true;
        }

        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            PrepareAndStartCamera();
        }

        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
        {
            //throw new System.NotImplementedException();
        }


        private void PrepareAndStartCamera()
        {
            camera.StopPreview();

            var display = activity.WindowManager.DefaultDisplay;
            if (display.Rotation == SurfaceOrientation.Rotation0)
            {
                camera.SetDisplayOrientation(90);
            }

            if (display.Rotation == SurfaceOrientation.Rotation270)
            {
                camera.SetDisplayOrientation(180);
            }

            camera.StartPreview();
        }


        private async void TakePhotoButtonTapped(object sender, EventArgs e)
        {
            camera.StopPreview();

            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Tomando foto");
            //DialogService.ShowLoading("Capturing Every Pixel");

            var aspectRation = ((decimal) Height) / Width;

            var image = Bitmap.CreateBitmap(textureView.Bitmap, 0, 0, textureView.Bitmap.Width, (int)(textureView.Bitmap.Width * aspectRation));
            using (var imageStream = new MemoryStream())
            {
                await image.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, imageStream);
                image.Recycle();
                imageBytes = imageStream.ToArray();
            }

            var manipulatePic = (new SvgManipulatePhotoPage(imageBytes));

            Acr.UserDialogs.UserDialogs.Instance.HideLoading();

            camera.StartPreview();
            //TurnOffCamera();
            await App.Current.MainPage.Navigation.PushAsync(manipulatePic, false);
        }

    }
}
