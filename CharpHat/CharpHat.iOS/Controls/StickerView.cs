using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace CharpHat.iOS.Controls
{


	public class StickerView : UIView {

		CGImage originalHat;
		nfloat _x;
		nfloat _y;
		nfloat scale;
		nfloat externScale;
		nfloat externRotation;
		nint originalWidth;
		nint originalHeight;

		public StickerView (CGRect frame)
			: base(frame)
		{
			
			BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0);
			DrawPath = new CGPath ();
			originalHat = UIImage.FromFile ("CSharpHat.png").CGImage;
			scale = ((nfloat)originalHat.Height) / originalHat.Width;
			originalWidth = originalHat.Width;
			_x = Bounds.Width / 2;
			_x = Bounds.Height / 2;
			externScale = 1;
			originalHeight = originalHat.Height;
		}

		public void AlterScaleFactor (float scaleFactor, float rotationFactor)
		{
			externScale = (nfloat)scaleFactor;
			externRotation = (nfloat)rotationFactor;
			SetNeedsDisplay ();
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
				var radians = (Math.PI / 180) * externRotation;
				CGPoint center = new CGPoint (_x - (originalWidth * externScale / 2), _y - (originalHeight * externScale / 2));
				//g.RotateCTM ((nfloat)radians);
				CGRect rekt = new CGRect (center.X,center.Y,originalWidth * externScale, originalHeight * externScale);
				g.DrawImage (rekt, originalHat);
			}
		}
}
}

