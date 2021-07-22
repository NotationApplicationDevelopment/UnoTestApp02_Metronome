using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace UnoTestApp02_Metronome
{
    public class MetroTimer
    {
        private readonly Stopwatch sw;
        private Task task;
        private double ms;
        private double rest;

        public int TimeCount { get; private set; }
        public int MaxCount { get; private set; }

        public double Interval { get; private set; }
        public TimeSpan Elapsed => sw.Elapsed;
        public double Bar => sw.Elapsed.TotalMilliseconds / Interval / MaxCount;

        public event Func<MetroTimer, Task> OnTick;
        public event Func<MetroTimer, bool, Task> OnUpdate;

        public MetroTimer(double bpm, int maxCount, int maxSubCount)
        {
            TimeCount = 0;
            sw = new Stopwatch();
            SetTempo(bpm, maxCount, maxSubCount);
            return;
        }

        private void SetTempo(double bpm, int maxCount, int maxSubCount)
        {
            MaxCount = maxCount;
            Interval = 1000.0 * 240.0 / (bpm * maxSubCount);
        }

        public void SetTempoAndReset(double bpm, int maxCount, int maxSubCount)
        {
            SetTempo(bpm, maxCount, maxSubCount);
            Restart();
        }

        private void CalcTimes()
        {
            ms = sw.Elapsed.TotalMilliseconds;
            rest = Interval - ms % Interval;
        }


        public void Start()
        {
            if (sw.IsRunning) { return; }

            task = OnTimerAsync();
        }

        public async Task StopAsync()
        {
            if (sw.IsRunning)
            {
                sw.Stop();
                await task;
            }
        }

        public void Restart()
        {
            if (sw.IsRunning)
            {
                sw.Restart();
                TimeCount = 0;
                CalcTimes();
            }
        }

        private async Task OnTimerAsync()
        {
            sw.Restart();
            TimeCount = 0;

            while (sw.IsRunning)
            {
                await OnTick?.Invoke(this);

                CalcTimes();

                while (sw.IsRunning && rest > 100.0)
                {
                    await OnUpdate?.Invoke(this, false);
                    await Task.Delay(20);

                    CalcTimes();
                }

                while (sw.IsRunning && sw.ElapsedMilliseconds < ms + rest)
                {
                    await OnUpdate?.Invoke(this, true);
                    await Task.Delay(1);
                }

                TimeCount++;

                if (TimeCount >= MaxCount)
                {
                    TimeCount = 0;
                }
            }
        }
    }
}
