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
using Xamarin.Forms.Platform.Android;
using CharpHat.Controls;
using Xamarin.Forms;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CharpHat.Controls.StickerableImage), typeof(CharpHat.Droid.Controls.StickerableImageRenderer))]
namespace CharpHat.Droid.Controls
{
    public class StickerableImageRenderer : ViewRenderer<StickerableImage, StickerView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StickerableImage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                SetNativeControl(new StickerView(Context));
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == StickerableImage.ScaleFactorProperty.PropertyName)
            {
                Control.AlterScaleFactor(Element.ScaleFactor);
            }
        }


    }
}