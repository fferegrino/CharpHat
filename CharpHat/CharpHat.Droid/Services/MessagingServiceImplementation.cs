 using CharpHat.Services;
using Android.Content;
using Android.Net;

[assembly: Xamarin.Forms.Dependency(typeof(CharpHat.Droid.Services.MessagingServiceImplementation))]
namespace CharpHat.Droid.Services
{
	public class MessagingServiceImplementation : IMessagingService
	{
		public MessagingServiceImplementation ()
		{
			
		}

		#region IMessagingService implementation
		public void ShowMessage (string message)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}
}

