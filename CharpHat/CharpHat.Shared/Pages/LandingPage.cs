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


			var charpHatGraphic = AppNGraphics.Read ("CharpHat.svg");

			var coolGraphic = new NControlView { 
				DrawingFunction = (canvas, rect) => {

					//canvas.DrawEllipse(new NGraphics.Rect(10,10,500,100),brush: new NGraphics.SolidBrush(new NGraphics.Color(1,1,1,1)));
					charpHatGraphic.Draw(canvas);
				},
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

            
            coolImage = new Image { Source = "justCSharp.png" };

            newTakeButton = new Button { Text = "Nueva foto", Style = AppStyles.MainPageButtonStyle };
            newTakeButton.Clicked += async (sender, args) =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new CameraPage());
            };


            aboutButton = new Button { Text = "Acerca de", Style = AppStyles.MainPageButtonStyle };
            aboutButton.Clicked += async (sender, args) =>
            {
				coolGraphic.Invalidate();
                //await App.Current.MainPage.Navigation.PushAsync(new AboutPage());
            };

			Content = coolGraphic;
				new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 30,
                Children = {
                    //coolImage,
                    coolGraphic,
                    newTakeButton, 
                    aboutButton
				}
            };
        }
    }
}

