using Microsoft.UI.Xaml.Controls;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UnoTestApp02_Metronome
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly MetroTimer timer;
        bool playMode = false;
        readonly IPlatformData platformData;

        readonly SolidColorBrush circleFill = new SolidColorBrush(Colors.Green);
        readonly SolidColorBrush circleLine = new SolidColorBrush(Colors.DarkGreen);
        readonly SolidColorBrush CanvasBack = new SolidColorBrush(Colors.LightGray);

        public MainPage()
        {
            InitializeComponent();
            timer = new MetroTimer(120, 4, 4);
            timer.OnTick += Timer_Tick;
            timer.OnUpdate += CanvasTimer_Tick;

            mainButton.Click += MainButton_Click;
            maxCount.ValueChanged += MaxCount_ValueChanged;
            maxSubCount.ValueChanged += MaxCount_ValueChanged;
            bpm.ValueChanged += Bpm_ValueChanged;


#if WINDOWS_UWP
            platformData = new UWP.PlatformData();
#elif __WASM__
            platformData = new Wasm.PlatformData();
#endif

            bottomInfo.Text = platformData.PlatformName;
            SetInterval();


            Task.Run(async () => {

                await Task.Delay(100);

                _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    SetCanvasPosition(circleBack, 0, 0);
                    SetCanvasPosition(circle, 0, 0);

                    circle.Fill = circleFill;
                    circle.Stroke = circleLine;
                    canvas.Background = CanvasBack;
                });

            });
        }


        private void SetCanvasPosition(Ellipse circle, float x, float y)
        {
            Vector2 pos = new Vector2(x, y) + (canvas.ActualSize - circle.ActualSize) * 0.5f;

            Canvas.SetLeft(circle, pos.X);
            Canvas.SetTop(circle, pos.Y);
        }

        private void Bpm_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            SetInterval();
        }

        private void MaxCount_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            sender.Value = double.IsNaN(sender.Value) ? 4 : sender.Value;

            SetInterval();
        }

        private void SetInterval()
        {
            _ = timer.SetIntervalAndResetAsync(bpm.Value, (int)maxCount.Value, (int)maxSubCount.Value);
        }

        private void Timer_Tick(MetroTimer sender)
        {
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CountUpdate();
            });

        }
        private void CanvasTimer_Tick(MetroTimer sender, bool onTick)
        {
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var t = MathF.PI * (float)(sender.Elapsed.TotalMilliseconds / sender.Interval / maxCount.Value);
                var x = MathF.Sin(t);
                SetCanvasPosition(circle, 90.0f * x, 0);

                var x2 = onTick ? 0 : 1;

                if(sender.TimeCount == sender.MaxCount-1)
                {
                    CanvasBack.Color = ColorMix(Colors.White, Colors.Black, x2);
                }
                else
                {
                    CanvasBack.Color = Colors.Black;
                }

                circleFill.Color = ColorMix(Colors.Red, Colors.Green, x2);
                circleLine.Color = ColorMix(Colors.Pink, Colors.LightGreen, x2);

                Color ColorMix(Color c1, Color c2, float v)
                {
                    var v2 = 1.0f - v;
                    var a = c1.A * v2 + c2.A * v;
                    var r = c1.R * v2 + c2.R * v;
                    var g = c1.G * v2 + c2.G * v;
                    var b = c1.B * v2 + c2.B * v;
                    return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
                }
            });
        }


        private void CountUpdate()
        {
            if(timer.TimeCount == 0)
            {
                platformData.PlaySound(new Uri("ms-appx:///Assets/Sounds/se1.mp3"));
            }
            else
            {
                platformData.PlaySound(new Uri("ms-appx:///Assets/Sounds/se2.mp3"));
            }

            count.Text = (timer.TimeCount + 1).ToString();
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            playMode = !playMode;
            if (playMode)
            {
                mainButton.Content = "Stop";
                timer.Start();
                CountUpdate();
            }
            else
            {
                mainButton.Content = "Start";

                _ = Task.Run(async () => 
                {
                    await timer.StopAsync();
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {

                        count.Text = "-";
                        SetCanvasPosition(circle, 0, 0);
                    });
                });
            }
        }
    }
}
