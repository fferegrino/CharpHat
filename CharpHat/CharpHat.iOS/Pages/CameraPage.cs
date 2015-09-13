using System;
using Xamarin.Forms.Platform.iOS;


using AVFoundation;
using CoreGraphics;
using Foundation;
using UIKit;

/*
 * AVFoundation Reference: http://red-glasses.com/index.php/tutorials/ios4-take-photos-with-live-video-preview-using-avfoundation/
 * Additional Camera Settings Reference: http://stackoverflow.com/questions/4550271/avfoundation-images-coming-in-unusably-dark
 * Custom Renderers: http://blog.xamarin.com/using-custom-uiviewcontrollers-in-xamarin.forms-on-ios/
 */
using Xamarin.Forms;
using System.Threading.Tasks;
using CharpHat.Pages;

[assembly:ExportRenderer(typeof(CharpHat.Pages.CameraPage), typeof(CharpHat.iOS.Pages.CameraPage))]
namespace CharpHat.iOS.Pages
{
	public class CameraPage : PageRenderer
	{
		AVCaptureSession captureSession;
		AVCaptureDeviceInput captureDeviceInput;
		UIView liveCameraStream;
		AVCaptureStillImageOutput stillImageOutput;
		UIButton takePhotoButton;
		UIButton toggleCameraButton;
		UIButton cancelPhotoButton;

		UIImage frontCameraIcon;
		UIImage rearCameraIcon;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetupUserInterface ();
			SetupEventHandlers ();
			AuthorizeCameraUse ();
			SetupLiveCameraStream ();
		}



		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}


		private void SetupUserInterface ()
		{
			var centerButtonX = View.Bounds.GetMidX () - 35f;
			var bottomButtonY = View.Bounds.Bottom - 85;
			var topRightX = View.Bounds.Right - 65;
			var topLeftX = View.Bounds.X + 25;
			var topButtonY = View.Bounds.Top + 25;
			var buttonWidth = 70;
			var buttonHeight = 70;

			liveCameraStream = new UIView () {
				Frame = new CGRect (0f, 0f, 320f, View.Bounds.Height)
			};

			takePhotoButton = new UIButton () {
				Frame = new CGRect (centerButtonX, bottomButtonY, buttonWidth, buttonHeight)
			};

			takePhotoButton.SetBackgroundImage (UIImage.FromFile ("TakePhotoButton.png"), UIControlState.Normal);

			toggleCameraButton = new UIButton () {
				Frame = new CGRect (topRightX, topButtonY, 37, 37)
			};

			frontCameraIcon = UIImage.FromFile ("ic_camera_front_white.png");
			rearCameraIcon = UIImage.FromFile ("ic_camera_rear_white.png");

			toggleCameraButton.SetBackgroundImage (frontCameraIcon, UIControlState.Normal);

			cancelPhotoButton = new UIButton () {
				Frame = new CGRect (topLeftX, topButtonY, 37, 37)
			};
			cancelPhotoButton.SetBackgroundImage (UIImage.FromFile ("ic_cancel_white.png"), UIControlState.Normal);



			View.Add (liveCameraStream);
			View.Add (takePhotoButton);
			View.Add (toggleCameraButton);
			View.Add (cancelPhotoButton);
		}


		private void SetupEventHandlers ()
		{
			takePhotoButton.TouchUpInside += async (object sender, EventArgs e) =>  CapturePhoto ();
			toggleCameraButton.TouchUpInside +=  (object sender, EventArgs e) =>  ToggleFrontBackCamera ();
			cancelPhotoButton.TouchUpInside += async (object sender, EventArgs e) => {
				await App.Current.MainPage.Navigation.PopAsync();
			};
		}


		public void ToggleFrontBackCamera ()
		{
			var devicePosition = captureDeviceInput.Device.Position;
			if (devicePosition == AVCaptureDevicePosition.Front) {
				devicePosition = AVCaptureDevicePosition.Back;
				toggleCameraButton.SetBackgroundImage (frontCameraIcon, UIControlState.Normal);
			} else {
				devicePosition = AVCaptureDevicePosition.Front;
				toggleCameraButton.SetBackgroundImage (rearCameraIcon, UIControlState.Normal);
			}

			var device = GetCameraForOrientation (devicePosition);
			ConfigureCameraForDevice (device);

			captureSession.BeginConfiguration ();
			captureSession.RemoveInput (captureDeviceInput);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice (device);
			captureSession.AddInput (captureDeviceInput);
			captureSession.CommitConfiguration ();
		}

		public AVCaptureDevice GetCameraForOrientation (AVCaptureDevicePosition orientation)
		{
			var devices = AVCaptureDevice.DevicesWithMediaType (AVMediaType.Video);

			foreach (var device in devices) {
				if (device.Position == orientation) {
					return device;
				}
			}

			return null;
		}

		public async void CapturePhoto ()
		{
			var videoConnection = stillImageOutput.ConnectionFromMediaType (AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync (videoConnection);
			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData (sampleBuffer);
			await SendPhoto (jpegImageAsNsData.ToArray ());
		}

		public async Task SendPhoto (byte[] image)
		{
			var navigationPage = new NavigationPage (new ManipulatePhotoPage (image)) {
			};

			await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync (navigationPage, false);
		}

		public void SetupLiveCameraStream ()
		{
			captureSession = new AVCaptureSession ();

			var viewLayer = liveCameraStream.Layer;
			var videoPreviewLayer = new AVCaptureVideoPreviewLayer (captureSession) {
				Frame = liveCameraStream.Bounds
			};
			liveCameraStream.Layer.AddSublayer (videoPreviewLayer);

			var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType (AVMediaType.Video);
			ConfigureCameraForDevice (captureDevice);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice (captureDevice);

			var dictionary = new NSMutableDictionary();
			dictionary[AVVideo.CodecKey] = new NSNumber((int) AVVideoCodec.JPEG);
			stillImageOutput = new AVCaptureStillImageOutput () {
				OutputSettings = new NSDictionary ()
			};

			captureSession.AddOutput (stillImageOutput);
			captureSession.AddInput (captureDeviceInput);
			captureSession.StartRunning ();
		}


		public void ConfigureCameraForDevice (AVCaptureDevice device)
		{
			var error = new NSError ();
			if (device.IsFocusModeSupported (AVCaptureFocusMode.ContinuousAutoFocus)) {
				device.LockForConfiguration (out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration ();
			} else if (device.IsExposureModeSupported (AVCaptureExposureMode.ContinuousAutoExposure)) {
				device.LockForConfiguration (out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration ();
			} else if (device.IsWhiteBalanceModeSupported (AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance)) {
				device.LockForConfiguration (out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration ();
			}
		}

		public async void AuthorizeCameraUse ()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus (AVMediaType.Video);

			if (authorizationStatus != AVAuthorizationStatus.Authorized) {
				await AVCaptureDevice.RequestAccessForMediaTypeAsync (AVMediaType.Video);
			}
		}
	}
}

