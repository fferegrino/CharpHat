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

        RelativeLayout mainLayout;

        public ManipulatePhotoPage(byte[] image)
        {
            BindingContext = new ManipulatePhotoViewModel(image);
            SetUpUi();
        }

        public ManipulatePhotoViewModel ViewModel { get { return BindingContext as ManipulatePhotoViewModel; } }

        private void SetUpUi()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            mainLayout = new RelativeLayout()
            {
                Padding = new Thickness(0)
            };

            var pic = new Image
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Source = ImageSource.FromStream(() => new MemoryStream(ViewModel.Image))
            };

            var stickerLayer = new StickerableImage { };



            mainLayout.Children.Add(pic,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            mainLayout.Children.Add(stickerLayer,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

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


            mainLayout.Children.Add(scaleSlider,
            Constraint.Constant(40),
            Constraint.RelativeToParent((parent) => { return parent.Height - 40; }),
            Constraint.RelativeToParent(p=> p.Width - 40),
            Constraint.Constant(30));

            Content = mainLayout;
        }
    }
}
