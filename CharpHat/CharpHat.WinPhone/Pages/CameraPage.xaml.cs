using CharpHat.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// Thanks to http://msicc.net/?p=4208
namespace CharpHat.WinPhone.Pages
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CameraPage : Page
    {
        public CameraPage()
        {
            this.InitializeComponent();


            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            //appView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            previewElement.Width = appView.VisibleBounds.Width;
            previewElement.Height = appView.VisibleBounds.Height;
        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ImageEncodingProperties format = ImageEncodingProperties.CreateJpeg();
            MemoryStream photo = new MemoryStream();
            await captureManager.CapturePhotoToStreamAsync(format, photo.AsRandomAccessStream());

            var manipulatePic = (new ManipulatePhotoPage(photo.ToArray()));



            await global::CharpHat.App.Current.MainPage.Navigation.PushAsync(manipulatePic, false);

        }

        public async Task ShowPreview()
        {
            captureManager = new MediaCapture();
            await captureManager.InitializeAsync();
            await StartPreview();
        }


        public void CancelCapture()
        {

            CleanCapture();
        }

        MediaCapture captureManager;
        bool isPreviewing;
        private async Task StartPreview()
        {

            captureManager.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
            previewElement.Source = captureManager;
            await captureManager.StartPreviewAsync();

            isPreviewing = true;
        }


        private async void CleanCapture()
        {
            if (captureManager != null)
            {
                if (isPreviewing == true)
                {
                    await captureManager.StopPreviewAsync();
                    isPreviewing = false;
                }
                previewElement.Source = null;
                captureManager.Dispose();
            }
        }

    }
}
