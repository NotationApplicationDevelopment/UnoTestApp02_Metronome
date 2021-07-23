using System;
using Uno.Foundation;

namespace UnoTestApp02_Metronome.Wasm
{
    public class PlatformData : IPlatformData
    {
        public string PlatformName => "Web Assembly";

        public bool LoadSound(Uri uri)
        {
            var text = uri.LocalPath.TrimStart('\\', '.', '/');
            var retJS = WebAssemblyRuntime.InvokeJS($"loadSound('{text}');");

            if(bool.TryParse(retJS, out var ret))
            {
                return ret;
            }

            return default;
        }

        public void PlaySound(Uri uri, double volume)
        {
            var text = uri.LocalPath.TrimStart('\\', '.', '/');
            _ = WebAssemblyRuntime.InvokeJS($"playSound('{text}',{volume});");
        }
    }
}
