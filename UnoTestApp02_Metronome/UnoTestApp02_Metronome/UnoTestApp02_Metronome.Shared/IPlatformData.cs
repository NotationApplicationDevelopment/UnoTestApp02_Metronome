using System;

namespace UnoTestApp02_Metronome
{
    public interface IPlatformData
    {
        string PlatformName { get; }

        bool LoadSound(Uri uri);

        void PlaySound(Uri uri, double volume);
    }
}
