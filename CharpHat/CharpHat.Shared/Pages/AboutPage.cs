using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CharpHat.Pages
{
	public class AboutPage : ContentPage
	{
		public AboutPage ()
		{
            Title = "Acerca de";
			Content = new StackLayout {
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}
