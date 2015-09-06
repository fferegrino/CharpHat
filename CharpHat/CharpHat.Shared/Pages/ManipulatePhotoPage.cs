using CharpHat.Controls;
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

        Grid mainLayout;

        public ManipulatePhotoPage(byte[] image)
        {
            BindingContext = new ManipulatePhotoViewModel(image);
            SetUpUi();
        }

        public ManipulatePhotoViewModel ViewModel { get { return BindingContext as ManipulatePhotoViewModel; } }

        private void SetUpUi()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            mainLayout = new Grid()
            {
                Padding = new Thickness(0),
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

            var stickerLayer = new StickerableImage { };

            mainLayout.Children.Add(pic);
            Grid.SetRowSpan(pic, 2);
            mainLayout.Children.Add(stickerLayer);
            Grid.SetRowSpan(stickerLayer, 2);

            var rotationSlider = new Slider
            {
                Maximum = 90,
                Minimum = -90,
				Value = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand
            };

            rotationSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                stickerLayer.RotationFactor = (float)e.NewValue;
            };

            var scaleSlider = new Slider
            {
                Maximum = 1.7,
                Minimum = 0.3,
                Value = 1,
				HorizontalOptions = LayoutOptions.FillAndExpand
            };

            scaleSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                stickerLayer.ScaleFactor = (float)e.NewValue;
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

            var stackedLayout = new StackLayout
            {
				Padding = 5,
				BackgroundColor = Color.FromRgba(0,0,0,100),
                Children = { stackRotation, stackResize }
            };

            //mainLayout.Children.Add()
            mainLayout.Children.Add(stackedLayout, 0, 1);

            Content = mainLayout;
        }
    }
}
