using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaController;

namespace OverlayApp.Classes
{
    public class MediaInfo
    {
        public MediaManager.MediaSession? MediaSession;
        public Windows.Media.Control.GlobalSystemMediaTransportControlsSession? ControlSession => MediaSession?.ControlSession;
        public Windows.Media.Control.GlobalSystemMediaTransportControlsSessionMediaProperties? MediaProperties => ControlSession?.TryGetMediaPropertiesAsync().Get();
        public string? Artist => MediaProperties?.Artist;
        public string? Title => MediaProperties?.Title;
        public string? Thumbnail = string.Empty;

        public MediaInfo(MediaManager.MediaSession MediaSession)
        {
            this.MediaSession = MediaSession;
            _ = SetThumbnail();
        }

        private async Task SetThumbnail()
        {
            if (MediaProperties is null)
                return;

            using var RandomAccessImageStream = await MediaProperties.Thumbnail.OpenReadAsync();
            using var ImageStream = RandomAccessImageStream.AsStream();

            using MemoryStream MemoryStream = new();
            await ImageStream.CopyToAsync(MemoryStream);

            string Base64 = Convert.ToBase64String(MemoryStream.ToArray());
            //System.Diagnostics.Debug.WriteLine($"Image get: {Base64}");

            Thumbnail = $"data:image/jpeg;base64,{Base64}";
        }
    }
}
