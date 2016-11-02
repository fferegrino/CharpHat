using System;
using Android.Graphics;
using Android.Views;
using CharpHat.Droid.Pages;

namespace CharpHat
{
	public class TextureViewSurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
	{
		public CameraPageRenderer Parent { get; private set; }

		public TextureViewSurfaceTextureListener(CameraPageRenderer parent)
		{
			Parent = parent;
		}

		public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
		{
			Parent.OpenCamera(width, height);
		}

		public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
		{
			return true;
		}

		public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
		{
			Parent.ConfigureTransform(width, height);
		}

		public void OnSurfaceTextureUpdated(SurfaceTexture surface)
		{
		}
	}
}
