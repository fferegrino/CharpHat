﻿using CharpHat.Helpers;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CharpHat.Pages
{
    public class BasePage : ContentPage
    {

        public ObservableCollection<ToolbarItem> BaseToolbarItems { get; set; }
        public BasePage()
        {
            
            BackgroundColor = AppColors.LightPurple;
			if(Device.OS == TargetPlatform.Android)
            	NavigationPage.SetHasNavigationBar(this, false);
            BaseToolbarItems = new ObservableCollection<ToolbarItem>();
        }

    }
}
