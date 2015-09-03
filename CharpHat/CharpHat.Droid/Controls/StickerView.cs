using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharpHat.Droid.Controls
{
    public class StickerView : View
    {
        float fingerX, fingerY;
        public StickerView(Context context)
            : base(context)
        {
            originalHatBitmap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.CSharpHat);
            transformedHatBitmap = originalHatBitmap;
        }


        private Bitmap originalHatBitmap;
        private Bitmap transformedHatBitmap;

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);


            var drawPaint = new Paint
            {
                Color = Color.Blue,
                AntiAlias = true,
                StrokeWidth = 5f,
                TextSize = 30f
            };

            
            var middleX = transformedHatBitmap.Width / 2;
            var middleY = transformedHatBitmap.Height / 2;

            canvas.DrawBitmap(transformedHatBitmap, fingerX - middleX, fingerY - middleY, drawPaint);
        }

        public void AlterScaleFactor(float scaleFactor)
        {
            //transformedHatBitmap.Recycle();
            int scaleWidth = (int)(originalHatBitmap.Width* scaleFactor);
            int scaleHeight = (int)(originalHatBitmap.Height * scaleFactor);
            transformedHatBitmap = Bitmap.CreateScaledBitmap(originalHatBitmap, scaleWidth, scaleHeight, true);
            
            Invalidate();
        }



        public override bool OnTouchEvent(MotionEvent e)
        {
            fingerX = e.GetX();
            fingerY = e.GetY();

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    //DrawPath.MoveTo(touchX, touchY);
                    break;
                case MotionEventActions.Move:
                    //DrawPath.LineTo(touchX, touchY);
                    break;
                case MotionEventActions.Up:
                    //DrawCanvas.DrawPath(DrawPath, DrawPaint);
                    //DrawPath.Reset();
                    break;
                default:
                    return false;
            }

            Invalidate();

            return true;
        }
    }
}

