using System;
using Xamarin.Forms;

using CharpHat.Helpers;

namespace CharpHat.Pages
{
	public class AboutPage : BasePage
	{
		Image coolImage;
		Button authorButton;
		Button madeWithButton;
		Button openSourceButton;

		public AboutPage ()
		{

			coolImage = new Image { Source = "justCSharp.png" };

			authorButton = new Button { Text = "Por Antonio Feregrino", Style = AppStyles.MainPageButtonStyle };
			authorButton.Clicked += (sender, args) =>
			{
				Device.OpenUri(new Uri("https://twitter.com/io_exception"));
			};

			madeWithButton = new Button { Text ="Hecho con <3 y Xamarin.Forms", Style = AppStyles.MainPageButtonStyle };
			madeWithButton.Clicked += (sender, args) =>
			{
				Device.OpenUri(new Uri("http://xamarin.com/forms"));
			};

			openSourceButton = new Button { Text ="¡Código abierto!", Style = AppStyles.MainPageButtonStyle };
			openSourceButton.Clicked += (sender, args) =>
			{
				Device.OpenUri(new Uri("http://github.com/fferegrino/CharpHat/README.md"));
			};

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(0,20,0,0),
				Spacing = 30,
				Children = {
					coolImage,
					authorButton,
					madeWithButton,
					openSourceButton
				}
			};
		}
	}
}

