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

            //mainLayout.Children.Add(pic,
            //    Constraint.Constant(0),
            //    Constraint.Constant(0),
            //    Constraint.RelativeToParent((parent) => { return parent.Width; }),
            //    Constraint.RelativeToParent((parent) => { return parent.Height; }));

            //mainLayout.Children.Add(stickerLayer,
            //    Constraint.Constant(0),
            //    Constraint.Constant(0),
            //    Constraint.RelativeToParent((parent) => { return parent.Width; }),
            //    Constraint.RelativeToParent((parent) => { return parent.Height; }));


            var rotationSlider = new Slider
            {
                Maximum = 90,
                Minimum = -90,
                Value = 0
            };
            rotationSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                stickerLayer.RotationFactor = (float)e.NewValue;
            };
            var scaleSlider = new Slider
            {
                Maximum = 2,
                Minimum = 0.5,
                Value = 1
            };
            scaleSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                stickerLayer.ScaleFactor = (float)e.NewValue;
            };
            var stackedLayout = new StackLayout
            {
                Children = { scaleSlider, rotationSlider }
            };

            //mainLayout.Children.Add()
            mainLayout.Children.Add(stackedLayout, 0, 1);

            Content = mainLayout;
        }
    }
}
