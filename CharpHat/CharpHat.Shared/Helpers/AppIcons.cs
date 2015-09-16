using System;
using System.Collections.Generic;
using System.Text;

namespace CharpHat.Helpers
{
    public static class AppIcons
    {
#if __ANDROID__
        public static string RotateIcon = "ic_rotate_left_white_24dp";
        public static string ResizeIcon = "ic_photo_size_select_large_white_24dp";
#endif
#if __IOS__
        public static string RotateIcon = "ic_rotate_left_white";
        public static string ResizeIcon = "ic_photo_size_select_large_white";
#endif
#if WINDOWS_PHONE_APP
        public static string RotateIcon = "ic_rotate_left_white";
        public static string ResizeIcon = "ic_photo_size_select_large_white";
#endif
    }
}
