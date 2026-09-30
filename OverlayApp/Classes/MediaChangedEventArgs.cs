using System;
using System.Collections.Generic;
using System.Text;

namespace OverlayApp.Classes
{
    public class MediaChangedEventArgs(MediaInfo NewMediaInfo) : EventArgs
    {
        public MediaInfo MediaInfo => NewMediaInfo;
    }
}
