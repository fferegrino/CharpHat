using CharpHat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using NControl.Abstractions;

namespace CharpHat.Pages
{
    public class LandingPage : BasePage
    {

        Button newTakeButton;
        Button aboutButton;
        Image coolImage;



        public LandingPage()
        {


            coolImage = new Image { Source = "justCSharp.png" };

            newTakeButton = new Button { Text = "Nueva foto", Style = AppStyles.MainPageButtonStyle };
            newTakeButton.Clicked += async (sender, args) =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new CameraPage());
            };


            aboutButton = new Button { Text = "Acerca de", Style = AppStyles.MainPageButtonStyle };
            aboutButton.Clicked += async (sender, args) =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new AboutPage());
            };

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 30,
                Children = {
                    coolImage,
                    newTakeButton, 
                    aboutButton
				}
            };
        }
    }
}

