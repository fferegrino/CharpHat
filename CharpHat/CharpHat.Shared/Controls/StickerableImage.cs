using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CharpHat.Controls
{
    public class StickerableImage : View
    {
        public static readonly BindableProperty ScaleFactorProperty =
            BindableProperty.Create((StickerableImage si) => si.ScaleFactor, 1f);

        public static readonly BindableProperty RotationFactorProperty =
            BindableProperty.Create((StickerableImage si) => si.RotationFactor, 1f);

        public float ScaleFactor
        {
            get
            {
                return (float)GetValue(ScaleFactorProperty);
            }
            set
            {
                SetValue(ScaleFactorProperty, value);
            }
        }
        public float RotationFactor
        {
            get
            {
                return (float)GetValue(RotationFactorProperty);
            }
            set
            {
                SetValue(RotationFactorProperty, value);
            }
        }

    }
}
