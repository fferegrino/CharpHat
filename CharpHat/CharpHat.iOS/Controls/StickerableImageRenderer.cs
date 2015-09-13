using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CharpHat.Controls;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using System.ComponentModel;



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



		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == StickerableImage.ScaleFactorProperty.PropertyName || e.PropertyName == StickerableImage.RotationFactorProperty.PropertyName)
			{
				Control.AlterScaleFactor(Element.ScaleFactor, Element.RotationFactor);
			}
		}

	}


}

