using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace UnoTestApp02_Metronome
{
    public class MetroTimer
    {
        readonly Stopwatch sw;
        Task task;
        private double interval;

        public bool IsEnabled { get; private set; }
        public int TimeCount { get; private set; }
        public int MaxCount { get; private set; }

        public event Action<MetroTimer> OnTick;
        public event Action<MetroTimer, bool> OnUpdate;

        public double Interval
        {
            get => interval; 
        }

        private void SetInterval(double bpm, int maxCount, int maxSubCount)
        {
            MaxCount = maxCount;
            interval = 1000.0 * 240.0 / (bpm * maxSubCount);
        }

        public async Task SetIntervalAndResetAsync(double bpm, int maxCount, int maxSubCount)
        {
            SetInterval(bpm, maxCount, maxSubCount);
            if (IsEnabled)
            {
                await StopAsync();
                Start();
            }
        }

        public TimeSpan Elapsed => sw.Elapsed;

        public MetroTimer(double bpm, int maxCount, int maxSubCount)
        {
            TimeCount = 0;
            sw = new Stopwatch();
            SetInterval(bpm, maxCount, maxSubCount);
            return;
        }

        public void Start()
        {
            if (task != null) { return; }

            IsEnabled = true;
            task = OnTimerAsync();
        }

        public async Task StopAsync()
        {
            if (task == null) { return; }

            IsEnabled = false;
            await task;
            task = null;
        }

        private async Task OnTimerAsync()
        {
            sw.Restart();
            TimeCount = 0;

            while (IsEnabled)
            {
                double ms = sw.Elapsed.TotalMilliseconds;
                double rest = Interval - ms % Interval;

                while (IsEnabled && rest > 100.0)
                {
                    OnUpdate?.Invoke(this, false);

                    await Task.Delay(20);

                    ms = sw.Elapsed.TotalMilliseconds;
                    rest = Interval - ms % Interval;
                }
                while (IsEnabled)
                {
                    OnUpdate?.Invoke(this, true);

                    if (sw.ElapsedMilliseconds >= ms + rest)
                    {
                        break;
                    }
                    await Task.Delay(1);

                }

                TimeCount++;

                if (TimeCount >= MaxCount)
                {
                    TimeCount = 0;
                }

                OnTick?.Invoke(this);


            }

            sw.Stop();
        }
    }
}
