using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace UnoTestApp02_Metronome.UWP
{
    public class PlatformData : IPlatformData
    {
        readonly Dictionary<string, MediaPlayer> mediaPlayers = new Dictionary<string, MediaPlayer>();

        public string PlatformName => "Universal Windows Platform";

        public bool LoadSound(Uri uri)
        {
            if (mediaPlayers.ContainsKey(uri.AbsoluteUri)) { return true; }

            MediaSource src = MediaSource.CreateFromUri(uri);

            if(src.State == MediaSourceState.Failed) { return false; }

            mediaPlayers[uri.AbsoluteUri] = new MediaPlayer()
            {
                Source = MediaSource.CreateFromUri(uri)
            };

            return true;
        }

        public void PlaySound(Uri uri, double volume)
        {
            if (mediaPlayers.TryGetValue(uri.AbsoluteUri, out MediaPlayer mp))
            {
                mp.PlaybackSession.Position = TimeSpan.Zero;
                mp.Volume = volume * 0.01;
                mp.Play();
            }
        }
    }
}
