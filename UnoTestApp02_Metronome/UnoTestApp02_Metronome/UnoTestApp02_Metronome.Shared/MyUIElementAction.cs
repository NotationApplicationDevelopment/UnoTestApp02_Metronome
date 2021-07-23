using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;

namespace UnoTestApp02_Metronome
{
    public static class MyUIElementAction
    {
        class IntarnalActionInfo
        {
            public bool DoubleTaped { get; set; }
            public DateTime BeforeTapedTime { get; set; }
        }

        static readonly Dictionary<UIElement, IntarnalActionInfo> infos = new Dictionary<UIElement, IntarnalActionInfo>();

        public static void MyDoubleTapped(this UIElement element, Action<object, PointerRoutedEventArgs, double> callback)
        {
            element.PointerReleased += CallBack;

            void CallBack(object sender, PointerRoutedEventArgs e)
            {
                var now = DateTime.Now;
                if (infos.TryGetValue(element, out var info))
                {
                    var t = (now - info.BeforeTapedTime).TotalSeconds;
                    if (info.DoubleTaped)
                    {
                        info.DoubleTaped = false;
                    }
                    else if (t < 1.0)
                    {
                        callback?.Invoke(sender, e, 0);
                        info.DoubleTaped = true;
                    }
                }
                else
                {
                    info = new IntarnalActionInfo();
                    infos.Add(element, info);
                }

                info.BeforeTapedTime = now;
            }
        }

    }
}
