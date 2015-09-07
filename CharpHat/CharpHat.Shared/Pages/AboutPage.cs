using CharpHat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CharpHat.Pages
{
    public class AboutPage : BasePage
    {
        public AboutPage()
        {
            BackgroundColor = AppColors.LightPurple;

            var coolImage = new Image { Source = "justCSharp.png" };
            Title = "Acerca de";
            Content = new StackLayout
            {
                Children = {
                    coolImage,
				}
            };
        }
    }
}
