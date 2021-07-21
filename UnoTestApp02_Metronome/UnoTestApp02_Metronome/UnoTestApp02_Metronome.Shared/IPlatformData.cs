using System;

namespace UnoTestApp02_Metronome
{
    public interface IPlatformData
    {
        string PlatformName { get; }

        void PlaySound(Uri uri);
    }
}
