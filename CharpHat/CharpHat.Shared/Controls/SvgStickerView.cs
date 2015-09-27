using NControl.Abstractions;
using NGraphics;
using System;
using System.Collections.Generic;
using System.Text;
//using Xamarin.Forms;

namespace CharpHat.Controls
{
    public class SvgStickerView : NControlView
    {
        public Graphic Sticker { get; private set; }

        double _trueY;
        double _trueX;

        double _fakeY;
        double _fakeX;

        double originalWidth;
        double originalHeight;

        public SvgStickerView(Graphic sticker)
        {
            base.HorizontalOptions = Xamarin.Forms.LayoutOptions.FillAndExpand;
            base.VerticalOptions = Xamarin.Forms.LayoutOptions.FillAndExpand;
            Sticker = sticker;
            originalHeight = sticker.Size.Height;
            originalWidth = sticker.Size.Width;


        }
        public override void Draw(ICanvas canvas, Rect rect)
        {
            base.Draw(canvas, rect);
            Sticker.Size = new Size(originalWidth * ScaleFactor, originalHeight * ScaleFactor);
            CalculateFakeValues();
            canvas.Translate(-1 * _fakeX, -1 * _fakeY);
            canvas.Rotate(RotationFactor);
            Sticker.Draw(canvas);
            //canvas.DrawText(this.ToString(), new Point(100, 100), new Font(), Colors.Black);

            //canvas.DrawText(rect.Width + ":" + rect.Height, new Point(100, 300), new Font(), Colors.Black);
        }

        public override bool TouchesBegan(IEnumerable<Point> points)
        {
            base.TouchesBegan(points);
            var enumerator = points.GetEnumerator();
            enumerator.MoveNext();
            var point = enumerator.Current;
            _trueX = point.X;
            _trueY = point.Y;
            Invalidate();
            return true;
        }

        public override bool TouchesMoved(IEnumerable<Point> points)
        {
            base.TouchesMoved(points);
            var enumerator = points.GetEnumerator();
            enumerator.MoveNext();
            var point = enumerator.Current;
            _trueX = point.X;
            _trueY = point.Y;

            Invalidate();

            return true;
        }

        private void CalculateFakeValues()
        {
            var stickerViewBox = Sticker.Size;
#if (__ANDROID__)
            _fakeX = (-2 * _trueX + (stickerViewBox.Width / 2));// *(1 / ScaleFactor);
            _fakeY = (-2 * _trueY + (stickerViewBox.Height / 2));// *(1 / ScaleFactor);
#endif
#if (__IOS__)
            _fakeX = (-1 * _trueX + (stickerViewBox.Width / 2));// *(1 / ScaleFactor);
            _fakeY = (-1 * _trueY + (stickerViewBox.Height / 2));// *(1 / ScaleFactor);
#endif

        }


        public static readonly Xamarin.Forms.BindableProperty ScaleFactorProperty =
            Xamarin.Forms.BindableProperty.Create((StickerableImage si) => si.ScaleFactor, 1f);

        public static readonly Xamarin.Forms.BindableProperty RotationFactorProperty =
            Xamarin.Forms.BindableProperty.Create((StickerableImage si) => si.RotationFactor, 1f);

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


        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == StickerableImage.ScaleFactorProperty.PropertyName || propertyName == StickerableImage.RotationFactorProperty.PropertyName)
            {
                Invalidate();
            }
        }





        public override string ToString()
        {
            return "Height:\t" + Height + "\n" +
                    "Width:\t" + Width + "\n" +
                    "X:\t" + _trueX + "\n" +
                    "Y:\t" + _trueY + "\n";
        }
    }
}
