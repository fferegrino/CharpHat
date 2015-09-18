using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
//using Xamarin.Forms;
namespace CharpHat.ViewModels
{
#if WINDOWS_PHONE_APP
    public class BaseViewModel : INotifyPropertyChanged
#else
        
    public class BaseViewModel : INotifyPropertyChanged, INotifyPropertyChanging
#endif
    {
        private string title = string.Empty;
        private string subTitle = string.Empty;
        private string icon = null;
        private bool isBusy;

        public const string TitlePropertyName = "Title";
        public const string SubtitlePropertyName = "Subtitle";
        public const string IconPropertyName = "Icon";
        public const string IsBusyPropertyName = "IsBusy";

#if WINDOWS_PHONE_APP
        public event Xamarin.Forms.PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
#else
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

#endif

        public BaseViewModel()
        {

        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value, TitlePropertyName); }
        }

        public string Subtitle
        {
            get { return subTitle; }
            set { SetProperty(ref subTitle, value, SubtitlePropertyName); }
        }

        public string Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value, IconPropertyName); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value, IsBusyPropertyName); }
        }

        protected void SetProperty<T>(ref T backingStore, T value, string propertyName, Action onChanged = null, Action<T> onChanging = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null)
                return;

			#if (__ANDROID__ || __IOS__)
#else
            PropertyChanging(this, new Xamarin.Forms.PropertyChangingEventArgs(propertyName));
#endif
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
