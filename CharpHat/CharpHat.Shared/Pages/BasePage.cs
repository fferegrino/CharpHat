using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CharpHat.Pages
{
	public class BasePage : ContentPage
	{
        public ObservableCollection<ToolbarItem> ToolbarItems { get; set; }
		public BasePage ()
		{
            ToolbarItems = new ObservableCollection<ToolbarItem>();
		}
	}
}
