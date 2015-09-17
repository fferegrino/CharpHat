using CharpHat.Helpers;
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
#if (__ANDROID__ || WINDOWS_PHONE_APP)
            	NavigationPage.SetHasNavigationBar(this, false);
#endif
            BaseToolbarItems = new ObservableCollection<ToolbarItem>();
        }

    }
}
