using LogProvider;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Vlc.DotNet.Wpf.Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            myControl.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            myControl.MediaPlayer.EndInit();
            myControl.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            myControl.MediaPlayer.Buffering += MediaPlayer_Buffering;
            myControl.MediaPlayer.MediaChanged += MediaPlayer_MediaChanged;
        }

        private const float minCacheToStart = 25;

        private bool playPending = false;

        private void MediaPlayer_Buffering(object sender, Core.VlcMediaPlayerBufferingEventArgs e)
        {
            Logger.Log($"Buffered: {e.NewCache}");
        }

        private bool programmaticPositionChange = false;

        private void MediaPlayer_PositionChanged(object sender, Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            Dispatcher.Invoke(((Action)(() =>
            {
                if (playPending)// && e.NewCache >= minCacheToStart)
                {
                    Logger.Log($"Buffered enough to start");
                    playPending = false;
                    myControl.MediaPlayer.Pause();
                }

                programmaticPositionChange = true;
                SeekBar.Value = e.NewPosition;
                CurrentTime.Text = TimeSpan.FromMilliseconds(myControl.MediaPlayer.Time).ToString(@"hh\:mm\:ss");
                programmaticPositionChange = false;
            })));
        }

        private void SeekBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            //if (!programmaticPositionChange)
            {
                myControl.MediaPlayer.Time += (long)TimeSpan.FromHours(1).TotalMilliseconds;
                //myControl.MediaPlayer.Time = (long)(myControl.MediaPlayer.Length * (SeekBar.Value / (SeekBar.Maximum - SeekBar.Minimum)));
            }
        }

        private void OnVlcControlNeedsLibDirectory(object sender, Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            e.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files (x86)\VideoLAN\VLC");
            //if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
            //    e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"..\..\..\lib\x86\"));
            //else
            //    e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"..\..\..\lib\x64\"));
        }

        private void OnLoadButtonClicked(object sender, RoutedEventArgs e)
        {
            Logger.Log($"Setting URL");
            playPending = true;
            myControl.MediaPlayer.Play(new Uri(MediaUrl.Text));
        }

        private void OnPlayButtonClick(object sender, RoutedEventArgs e)
        {
            myControl.MediaPlayer.Play();
            //myControl.MediaPlayer.Play(new Uri(MediaUrl.Text));
            //myControl.MediaPlayer.Play(new Uri("http://10.5.7.207/userapi/streams/27/mpd"));
            //myControl.MediaPlayer.Play(new Uri("http://10.5.5.7/q/p/userapi/streams/1/mpd"));
            //myControl.MediaPlayer.Play(new FileInfo(@"..\..\..\Vlc.DotNet\Samples\Videos\BBB trailer.mov"));
        }

        private void MediaPlayer_MediaChanged(object sender, Core.VlcMediaPlayerMediaChangedEventArgs e)
        {
            
        }

        private void OnPauseButtonClick(object sender, RoutedEventArgs e)
        {
            myControl.MediaPlayer.Pause();
        }

        private void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            myControl.MediaPlayer.Rate = 2;
        }

        private void GetLength_Click(object sender, RoutedEventArgs e)
        {
            GetLength.Content = "GetLength: " + myControl.MediaPlayer.Length + " ms";
        }

        private void GetCurrentTime_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentTime.Content = "Get Current Time: " + myControl.MediaPlayer.Time + " ms";
        }

        private void SetCurrentTime_Click(object sender, RoutedEventArgs e)
        {
            myControl.MediaPlayer.Time = 5000;
            SetCurrentTime.Content = myControl.MediaPlayer.Time + " ms";
        }
    }
}
