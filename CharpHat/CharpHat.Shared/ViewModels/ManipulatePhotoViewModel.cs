using System;
using System.Collections.Generic;
using System.Text;

namespace CharpHat.ViewModels
{
    public class ManipulatePhotoViewModel : BaseViewModel
    {
        
        public ManipulatePhotoViewModel(byte[] image)
        {
            Image = image;
        }

        public byte[] Image { get; set; }
    }
}
