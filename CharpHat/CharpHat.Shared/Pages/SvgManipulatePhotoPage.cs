using CharpHat.Controls;
using CharpHat.Helpers;
using CharpHat.ViewModels;
using NControl.Abstractions;
using NGraphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using CharpHat.Services;

namespace CharpHat.Pages
{
    public class SvgManipulatePhotoPage : BasePage
    {

		Graphic _originalCharpHat;
        SvgStickerView _stickerSurface;
        Slider rotationSlider;
        Slider scaleSlider;
        StackLayout controlsStack;
        Button continueButton;
        Grid mainLayout;

        public ManipulatePhotoViewModel ViewModel { get { return BindingContext as ManipulatePhotoViewModel; } }

        public SvgManipulatePhotoPage(byte[] image)
        {
            if (image != null)
                BindingContext = new ManipulatePhotoViewModel(image);

            NavigationPage.SetHasNavigationBar(this, false);

            LoadResources();
            SetupUI();
            SetUpEvents();
        }

        private void SetUpEvents()
        {

            rotationSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                _stickerSurface.RotationFactor = (float)e.NewValue;
            };

            scaleSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                _stickerSurface.ScaleFactor = (float)e.NewValue;
            };

			continueButton.Clicked += ContinueButton_Clicked;
        }

		void ContinueButton_Clicked(object sender, EventArgs e)
		{
			controlsStack.IsVisible = false;
			var image = DependencyService.Get<IScreenshotService>().CaptureScreen();
			DependencyService.Get<IPictureManager>().SavePictureToDisk("CharpHatPic", "CharpHat", image);
			Acr.UserDialogs.UserDialogs.Instance.ShowSuccess("Imagen guardada en la galería");
			controlsStack.IsVisible = true;
		}

        private void LoadResources()
        {
            _originalCharpHat = AppNGraphics.Read("CharpHat.svg");
        }

        private void SetupUI()
        {


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

            if (ViewModel != null)
            {
                var pic = new Image
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
					Aspect = Aspect.Fill,
                    Source = ImageSource.FromStream(() => new MemoryStream(ViewModel.Image)),
                };

                mainLayout.Children.Add(pic);
                Grid.SetRowSpan(pic, 2);

            }

            _stickerSurface = new SvgStickerView(_originalCharpHat);


            //mainLayout.Children.Add(pic);
            //Grid.SetRowSpan(pic, 2);
            mainLayout.Children.Add(_stickerSurface);
            Grid.SetRowSpan(_stickerSurface, 2);

            rotationSlider = new Slider
            {
                Maximum = 60,
                Minimum = -60,
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

            Image imageRotation = new Image { Source = AppIcons.RotateIcon };
            Image imageResize = new Image { Source = AppIcons.ResizeIcon };

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


            var stackButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };

            continueButton = new Button
            {
                Text = "Continuar",
                Style = AppStyles.BaseButtonStyle,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            stackButtons.Children.Add(continueButton);

            if (Device.OS == TargetPlatform.iOS)
            {
                // Since there is no back button on iOS...
                var cancelButton = new Button
                {
                    Text = "Cancel",
                    Style = AppStyles.CancelButtonStyle
                };
                cancelButton.Clicked += async (s, a) =>
                {
                    await App.Current.MainPage.Navigation.PopModalAsync();
                };
                stackButtons.Children.Add(cancelButton);
            }

            controlsStack = new StackLayout
            {
                Padding = 5,
                BackgroundColor = Xamarin.Forms.Color.FromRgba(0, 0, 0, 100)
            };

            //if (Device.OS != TargetPlatform.iOS) // No rotation supported for iOS yet.
                controlsStack.Children.Add(stackRotation);
            controlsStack.Children.Add(stackResize);
            controlsStack.Children.Add(stackButtons);

            mainLayout.Children.Add(controlsStack, 0, 1);

            Content = mainLayout;

            //Content = _stickerSurface;
        }
    }
}
