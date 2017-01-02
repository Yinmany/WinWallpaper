using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

using WinWallpaper.Utils;
using WinWallpaper.View;
namespace WinWallpaper
{
    public partial class MainForm : Form
    {
        AboutForm aboutForm = null;
        PlayerForm playerForm = null;
        public MCIPlayer player { get; set; }
        public MainForm()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr hwnd = Win32.User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", "Program Manager");
            if (playerForm == null || playerForm.IsDisposed)
            {
                if (player==null || !File.Exists(player.FilePath))
                {
                    MessageBox.Show("视频文件不存在!");
                    return;
                }
                else
                {
                    playerForm = new PlayerForm(player.FilePath);
                    IntPtr child = playerForm.Handle;
                    if (IntPtr.Zero == Win32.User32.SetParent(child, hwnd))
                    {
                        MessageBox.Show("error", "error");
                    }
                }
            }
            else
            {
                if (!player.FilePath.Equals(playerForm.p.FilePath))
                {
                    playerForm.p.Post(MCIPlayer.Cmd.close);
                    playerForm.p.Replace(player.FilePath);
                    playerForm.p.Post(MCIPlayer.Cmd.play);
                    playerForm.p.Post(MCIPlayer.Cmd.loops);
                }
            }
            playerForm.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "视频文件 (*.wmv;*.mp4)|*.wmv;*.mp4";
            DialogResult res = fileDialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                this.Text += " ["+filePath+"]";
                if (player != null)
                {
                    player.Post(MCIPlayer.Cmd.close);
                    player = null;
                }

                player = new MCIPlayer(filePath, "pre", pictureBox1.Handle, pictureBox1.DisplayRectangle);
                player.Post(MCIPlayer.Cmd.play);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 取消关闭窗口
            e.Cancel = true;

            // 隐藏窗体
            this.Hide();

            notifyIcon1.Visible = true;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            System.Environment.Exit(0);
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutForm == null || aboutForm.IsDisposed)
            {
                aboutForm = new AboutForm();
            }
            aboutForm.Show();
        }
    }
}
