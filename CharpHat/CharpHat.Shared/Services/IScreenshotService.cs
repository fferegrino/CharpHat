using System;
using System.Collections.Generic;
using System.Text;

namespace CharpHat.Services
{
    public interface IScreenshotService
    {
        byte[] CaptureScreen();
    }
}
