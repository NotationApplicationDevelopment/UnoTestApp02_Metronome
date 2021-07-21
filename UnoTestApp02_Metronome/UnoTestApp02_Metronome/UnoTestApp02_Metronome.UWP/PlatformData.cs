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

        public void PlaySound(Uri uri)
        {
            if(mediaPlayers.TryGetValue(uri.AbsoluteUri, out MediaPlayer mp))
            {
                mp.PlaybackSession.Position = TimeSpan.Zero;
            }
            else
            {
                mp = new MediaPlayer
                {
                    Source = MediaSource.CreateFromUri(uri),
                    Volume = 0.7
                };
                mediaPlayers.Add(uri.AbsoluteUri, mp);
            }

            mp.Play();
        }
    }
}
