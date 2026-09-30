using NAudio.CoreAudioApi;
using WindowsMediaController;
using static WindowsMediaController.MediaManager;

namespace OverlayApp.Classes
{
    internal class MediaController
    {
        public MediaManager MediaManager = new();
        public MMDevice MMDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        public event EventHandler<MediaChangedEventArgs>? MediaChanged;

        public MediaController()
        {
            MediaManager.Start();

            MediaManager.OnAnyMediaPropertyChanged += MediaManager_OnAnyMediaPropertyChanged;
            MediaManager.OnFocusedSessionChanged += MediaManager_OnFocusedSessionChanged;

            LoadCurrentMedia();
        }

        private async void LoadCurrentMedia()
        {
            if (MediaManager.CurrentMediaSessions.Count > 0)
            {
                var MediaSession = MediaManager.GetFocusedSession();
                MediaInfo NewMediaInfo = new(MediaSession);

                await Task.Delay(100);
                MediaChanged?.Invoke(this, new(NewMediaInfo));
            }
        }

        public async void Toggle()
        {
            var CurrentSession = MediaManager.GetFocusedSession();
            await CurrentSession?.ControlSession.TryTogglePlayPauseAsync();
        }

        public async void PlayPrevious()
        {
            var CurrentSession = MediaManager.GetFocusedSession();
            await CurrentSession?.ControlSession.TrySkipPreviousAsync();
        }

        public async void PlayNext()
        {
            var CurrentSession = MediaManager.GetFocusedSession();
            await CurrentSession?.ControlSession.TrySkipNextAsync();
        }
        
        public void ChangeDeviceVolume(float ScalarVolume)
        {
            MMDevice.AudioEndpointVolume.MasterVolumeLevelScalar = ScalarVolume;
        }

        private void MediaManager_OnFocusedSessionChanged(MediaSession MediaSession)
        {
            MediaInfo NewMediaInfo = new(MediaSession);
            MediaChanged?.Invoke(this, new(NewMediaInfo));
        }

        private void MediaManager_OnAnyMediaPropertyChanged(MediaSession MediaSession, Windows.Media.Control.GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
            MediaInfo NewMediaInfo = new(MediaSession);
            MediaChanged?.Invoke(this, new(NewMediaInfo));
        }
    }
}
