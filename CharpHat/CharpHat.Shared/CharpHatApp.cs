using CharpHat.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CharpHat
{
    public class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new LandingPage());
        }
    }
}
