using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace CharpHat.Pages
{
	public class LandingPage : BasePage
	{
		public LandingPage ()
		{
			Content = new StackLayout {
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}
