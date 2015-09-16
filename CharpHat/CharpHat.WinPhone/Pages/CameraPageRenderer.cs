using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.WinRT;

[assembly: ExportRenderer(typeof(CharpHat.Pages.CameraPage), typeof(CharpHat.WinPhone.Pages.CameraPageRenderer))]
namespace CharpHat.WinPhone.Pages
{
    public class CameraPageRenderer : PageRenderer
    {
        CameraPage _cameraPage;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            _cameraPage = new CameraPage();
            if (Page != null)
            {
                Page.Appearing += async (s, a) => { _cameraPage.ShowPreview(); };
                Page.Disappearing += async (s, a) => { _cameraPage.CancelCapture(); };
            }
            else
            {
                return;
            }
            (this.ContainerElement as Canvas).Children.Add(_cameraPage);
        }



    }
}
