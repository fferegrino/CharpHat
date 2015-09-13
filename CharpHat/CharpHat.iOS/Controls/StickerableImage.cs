using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CharpHat.Controls;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;



[assembly: ExportRenderer(typeof(CharpHat.Controls.StickerableImage), typeof(CharpHat.iOS.Controls.StickerableImageRenderer))]
namespace CharpHat.iOS.Controls
{
	public class StickerableImageRenderer : ViewRenderer<StickerableImage, StickerView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<StickerableImage> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
				return;

			try {
				SetNativeControl(new StickerView(CGRect.Empty));
			} catch (Exception ex) {
			}
		}

	}

	public class StickerView : UIView {

		CGImage originalHat;
		nfloat _x;
		nfloat _y;
		nfloat scale;
		nint originalWidth;
		nint originalHeight;

		public StickerView (CGRect frame)
			: base(frame)
		{
			BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0);
			DrawPath = new CGPath ();
			originalHat =UIImage.FromFile ("CSharpHat.png").CGImage;
			scale = ((nfloat)originalHat.Height) / originalHat.Width;
			originalWidth = originalHat.Width;
			originalHeight = originalHat.Height;
		}

		private CGPoint PreviousPoint;
		private CGPath DrawPath;

		public void Clear ()
		{
			DrawPath.Dispose ();
			DrawPath = new CGPath ();
			SetNeedsDisplay ();
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			var path = new UIBezierPath
			{
			};

			var touch = (UITouch)touches.AnyObject;
			PreviousPoint = touch.PreviousLocationInView (this);


			_x = PreviousPoint.X;
			_y = PreviousPoint.Y;

			var newPoint = touch.LocationInView (this);
			path.MoveTo (newPoint);

			InvokeOnMainThread (SetNeedsDisplay);
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			var touch = (UITouch)touches.AnyObject;
			var currentPoint = touch.LocationInView (this);

			if (Math.Abs (currentPoint.X - PreviousPoint.X) >= 4 ||
				Math.Abs (currentPoint.Y - PreviousPoint.Y) >= 4) {

				
				PreviousPoint = currentPoint;
				_x = currentPoint.X;
				_y = currentPoint.Y;
			} else {
				
			}

			InvokeOnMainThread (SetNeedsDisplay);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			InvokeOnMainThread (SetNeedsDisplay);
		}

		public override void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			InvokeOnMainThread (SetNeedsDisplay);
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			using(CGContext g = UIGraphics.GetCurrentContext ()){
				CGRect rekt = new CGRect (_x- (originalWidth -2), _y-(originalHeight/2),originalWidth, originalWidth * scale);
				g.DrawImage (rekt, originalHat);
			}
		}
	}

}

