﻿using System;
using System.Collections.Generic;
using System.Text;


// Source http://www.goxuni.com/how-to-save-an-image-to-a-device-using-xuni-and-xamarin-forms/
namespace CharpHat.Services
{
    public interface IPictureManager
    {
        void SavePictureToDisk(string filename, byte[] imageData);
    }
}
