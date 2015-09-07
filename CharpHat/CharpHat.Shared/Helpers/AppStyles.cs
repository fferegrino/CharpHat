using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CharpHat.Helpers
{
    public static class AppStyles
    {
        public static Style BaseButtonStyle = new Style(typeof(Button))
        {
            Setters =
            {
                new Setter { Property = Button.BorderColorProperty, Value = AppColors.DarkPurple },
                new Setter { Property = Button.TextColorProperty, Value = AppColors.DarkPurple },
                new Setter { Property = Button.BackgroundColorProperty, Value = AppColors.LightPurple }
            }
        };

        public static Style MainPageButtonStyle = new Style(typeof(Button))
        {
            BasedOn = BaseButtonStyle,
            Setters = {
                new Setter { Property = Button.FontSizeProperty, Value = 20 }
            }
        };
    }
}
