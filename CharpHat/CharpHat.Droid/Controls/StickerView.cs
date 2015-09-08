using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Runtime;
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
            IWindowManager windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            Display display = windowManager.DefaultDisplay;
            fingerX = ((float)display.Width) / 2;
            fingerY = ((float)display.Height) / 4;

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

        public void AlterScaleFactor(float scaleFactor, float rotation)
        {
            //transformedHatBitmap.Recycle();
            transformedHatBitmap = null;

            // This line here is THE DEVIL:
            GC.Collect();

            Matrix matrix = new Matrix();
            // resize the bit map
            matrix.PostScale(scaleFactor, scaleFactor);
            // rotate the Bitmap
            matrix.PostRotate(rotation);

            // recreate the new Bitmap
            transformedHatBitmap = Bitmap.CreateBitmap(originalHatBitmap, 0, 0, originalHatBitmap.Width, originalHatBitmap.Height, matrix, true);

            
            
            Invalidate();
        }



        public override bool OnTouchEvent(MotionEvent e)
        {
            fingerX = e.GetX();
            fingerY = e.GetY();

            Invalidate();

            return true;
        }
    }
}

