using CharpHat.Controls;
using CharpHat.Helpers;
using CharpHat.Services;
using CharpHat.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace CharpHat.Pages
{
    public class ManipulatePhotoPage : BasePage
    {

        private Grid mainLayout;
        public ManipulatePhotoViewModel ViewModel { get { return BindingContext as ManipulatePhotoViewModel; } }

        Slider rotationSlider;
        Slider scaleSlider;
        StackLayout controlsStack;
        Button continueButton;
        // Custom control
        StickerableImage stickerLayer;


        public ManipulatePhotoPage(byte[] image)
        {
            BindingContext = new ManipulatePhotoViewModel(image);
            SetUpUi();
            SetUpEvents();
        }

        private void SetUpEvents()
        {

            rotationSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                stickerLayer.RotationFactor = (float)e.NewValue;
            };

            scaleSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                stickerLayer.ScaleFactor = (float)e.NewValue;
            };

            continueButton.Clicked += async (s, a) => {
                controlsStack.IsVisible = false;
                var image = DependencyService.Get<IScreenshotService>().CaptureScreen();
                DependencyService.Get<IPictureManager>().SavePictureToDisk("CharpHat", image);
                await App.Current.MainPage.Navigation.PopToRootAsync();
            };

        }


        private void SetUpUi()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            mainLayout = new Grid()
            {
                Padding = new Thickness(0),
                ColumnSpacing = 0,
                RowSpacing = 0,
                RowDefinitions = {
                                   new RowDefinition{ Height = new GridLength(1, GridUnitType.Star)},
                                   new RowDefinition{ Height = GridLength.Auto }
                               }
            };

            var pic = new Image
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Source = ImageSource.FromStream(() => new MemoryStream(ViewModel.Image)),
            };

            stickerLayer = new StickerableImage { };

            mainLayout.Children.Add(pic);
            Grid.SetRowSpan(pic, 2);
            mainLayout.Children.Add(stickerLayer);
            Grid.SetRowSpan(stickerLayer, 2);

            rotationSlider = new Slider
            {
                Maximum = 90,
                Minimum = -90,
                Value = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };


            scaleSlider = new Slider
            {
                Maximum = 1.7,
                Minimum = 0.3,
                Value = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };


            Image imageRotation = new Image { Source = "rotate.png" };
            Image imageResize = new Image { Source = "resize.png" };

            var stackRotation = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { imageRotation, rotationSlider },
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var stackResize = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { imageResize, scaleSlider },
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            continueButton = new Button { Text = "Continuar", Style = AppStyles.BaseButtonStyle };

            controlsStack = new StackLayout
            {
                Padding = 5,
                BackgroundColor = Color.FromRgba(0, 0, 0, 100),
                Children = { stackRotation, stackResize, continueButton }
            };

            mainLayout.Children.Add(controlsStack, 0, 1);

            Content = mainLayout;
        }
    }
}
