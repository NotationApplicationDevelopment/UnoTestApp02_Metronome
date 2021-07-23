using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace UnoTestApp02_Metronome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public bool IsOpening { get; private set; }
        public bool IsClosing { get; private set; }

        public double Volume
        {
            get => volumeCtrl.Value;
            set => volumeCtrl.Value = value;
        }

        public int SoundType
        {
            get => soundTypeCtrl.SelectedIndex;
            set => soundTypeCtrl.SelectedIndex = value;
        }

        public SettingsPage()
        {
            InitializeComponent();
            soundTypeCtrl.ItemsSource = new string[] { "Digital", "Real", "Dram", "Cats" };
            thumb.DragDelta += Thumb_DragDelta;
            thumb.DragCompleted += Thumb_DragCompleted;
            thumb.MyDoubleTapped(Thumb_DoubleTapped);
            thumb.DragCompleted += Thumb_DragCompleted;
            mover.Height = 130;


        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (mover.Height > 30)
            {
                _ = Close();
            }
            else
            {
                _ = Open();
            }
        }

        private void Thumb_DoubleTapped(object sender, PointerRoutedEventArgs e, double t)
        {
            _ = Close();
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            mover.Height = Math.Max(0, mover.Height + e.VerticalChange);
        }

        public async Task Close()
        {
            if (IsClosing || IsOpening) { return; }

            IsClosing = true;

            while (mover.Height < 130)
            {
                mover.Height = 130 - 0.5 * (130 - mover.Height);

                await Task.Delay(16);
            }
            IsClosing = false;
        }

        public async Task Open()
        {
            if (IsClosing || IsOpening) { return; }

            IsOpening = true;
            while (mover.Height > 0.001)
            {
                mover.Height *= 0.5;

                await Task.Delay(16);
            }
            IsOpening = false;
        }
    }
}
