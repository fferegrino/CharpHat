using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CharpHat.Pages
{
	public class BasePage : ContentPage
	{
        public ObservableCollection<ToolbarItem> BaseToolbarItems { get; set; }
		public BasePage ()
		{
            BaseToolbarItems = new ObservableCollection<ToolbarItem>();
		}
	}
}
