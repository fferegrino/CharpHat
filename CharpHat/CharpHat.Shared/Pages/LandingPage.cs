using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CharpHat.Pages
{
    public class LandingPage : BasePage
    {
        Button newTakeButton;

        public LandingPage()
        {
            newTakeButton = new Button { Text = "Nueva foto" };
            newTakeButton.Clicked += async (sender, args) =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new CameraPage());
            };

            Content = new StackLayout
            {
                Children = {
					new Label { Text = "Put a nice hat over your head" },
                    newTakeButton
				}
            };
        }
    }
}
