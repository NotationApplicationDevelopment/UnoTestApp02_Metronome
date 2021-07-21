using System;
using Uno.Foundation;

namespace UnoTestApp02_Metronome.Wasm
{
    public class PlatformData : IPlatformData
    {
        public string PlatformName => "Web Assembly";

        public void PlaySound(Uri uri)
        {
            var text = uri.LocalPath.TrimStart('\\', '.', '/');
            _ = WebAssemblyRuntime.InvokeJS($"playSound('{text}');");
        }
    }
}
