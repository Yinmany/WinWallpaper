using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommonUtils;
using WinWallpaper.Utils;
using MessageBox = System.Windows.MessageBox;

namespace WallpaperApp
{
    /// <summary>
    /// FullWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FullWindow : Window
    {
        private MediaElement myPlayer;

        public FullWindow()
        {
            InitializeComponent();

            this.ShowInTaskbar = false;

            myPlayer = media;
            myPlayer.Margin = new Thickness(0, 0, 0, 0);


            myPlayer.UnloadedBehavior = MediaState.Manual;

            this.GoFullscreen();

            this.Left = 0;
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        public void ChangeSource(Uri uri)
        {
            myPlayer.Stop();
            myPlayer.Source = uri;
            myPlayer.Play();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            IntPtr thisIntPtr = new WindowInteropHelper(this).Handle;
            Win32.User32.SetParent(thisIntPtr, WallpaperUtils.GetWorkerW());

            myPlayer.Play();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            myPlayer.Width = ActualWidth;
            myPlayer.Height = ActualHeight;
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            myPlayer.Position = TimeSpan.Zero;
            myPlayer.Play();
        }
    }
}
