using CsharpHat.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CsharpHat.Pages
{
    public class EditPhotoPage : ContentPage
    {
        public EditPhotoPage()
        {
            Content = BuildGrid();
        }

        private Grid BuildGrid()
        {
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Star) },
                    new RowDefinition { Height = GridLength.Auto }
                },
                Children =
                {
                    {
                        new Label 
                        {
                            Text ="C# hat top",
                            FontSize =  50,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                        }, 0, 0
                    },
                    {
                        new CustomImage
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            BackgroundColor = Color.White
                        }, 0, 1
                    },
                    {
                        new Label 
                        {
                            Text ="C# hat bottom",
                            FontSize =  50,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                        }, 0, 2
                    }
                }
            };

            return grid;
        }
    }
}
