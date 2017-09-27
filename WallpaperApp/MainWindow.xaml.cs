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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using WinWallpaper.Utils;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace WallpaperApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();

            media.UnloadedBehavior = MediaState.Manual;


            fullWindow = new FullWindow();
            fullWindow.Show();
        }

        // 壁纸窗口
        private FullWindow fullWindow;

        private string currentAudioPath;

        private static NotifyIcon trayIcon;

        private bool isPlay = true;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddTrayIcon();
        }

        private void MetroWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void RemoveTrayIcon()
        {
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
                trayIcon = null;
            }
        }
        private void AddTrayIcon()
        {
            if (trayIcon != null)
            {
                return;
            }
            trayIcon = new NotifyIcon
            {
                Icon = Properties.Resources.icon,
                Text = "NotifyIconStd"
            };
            trayIcon.Visible = true;

            trayIcon.MouseClick += TrayIcon_MouseClick;

            ContextMenu menu = new ContextMenu();

            MenuItem closeItem = new MenuItem();
            closeItem.Text = "关闭";
            closeItem.Click += new EventHandler(delegate
            {
                RemoveTrayIcon();
                Environment.Exit(0);
            });

            menu.MenuItems.Add(closeItem);

            trayIcon.ContextMenu = menu;    //设置NotifyIcon的右键弹出菜单
        }

        private void TrayIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        //---------------------------  开关事件  --------------------------------


        private void btnFull_Checked(object sender, RoutedEventArgs e)
        {
            if(fullWindow!=null)
                fullWindow.Opacity = 1;
        }

        private void btnFull_Unchecked(object sender, RoutedEventArgs e)
        {
            fullWindow.Opacity = 0;
        }


        //---------------------------------------------------------------------

        
        //-------------------------- 事件处理 -----------------------------

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(currentAudioPath != null)
                fullWindow.ChangeSource(new Uri(currentAudioPath));
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "视频|*.mp4;*.wmv";
            openFileDialog.Multiselect = false;
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                currentAudioPath = openFileDialog.FileName;
                media.Stop();
                media.Source = new Uri(currentAudioPath);
                media.Play();
                isPlay = true;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void media_MouseDown(object sender, MouseButtonEventArgs e)
        {
            media.Pause();
        }
        //---------------------------------------------------------------------
    }
}