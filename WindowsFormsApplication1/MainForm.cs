using System;
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
            IntPtr HWND = GetWorkerW();
            Player(HWND);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private IntPtr GetWorkerW()
        {
            // 获取
            IntPtr windowHandle = Win32.User32.FindWindow("Progman", null);

            IntPtr zero = IntPtr.Zero;
            // 重要消息 生成一个WorkerW 顶级窗口 桌面列表会随之搬家
            Win32.User32.SendMessageTimeout(windowHandle, 0x52c, new IntPtr(0), IntPtr.Zero, Win32.User32.SendMessageTimeoutFlags.SMTO_NORMAL, 0x3e8, out zero);
            IntPtr workerw = IntPtr.Zero;
            // 消息会生成两个WorkerW 顶级窗口 所以要枚举不包含“SHELLDLL_DefView”这个的 WorkerW 窗口 隐藏掉。
            Win32.User32.EnumWindows(delegate (IntPtr tophandle, IntPtr topparamhandle)
            {
                if (Win32.User32.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    workerw = Win32.User32.FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                }
                return true;
            }, IntPtr.Zero);
            Win32.User32.ShowWindow(workerw, Win32.User32.SW_HIDE);
            return windowHandle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "视频文件 (*.wmv;*.mp4)|*.wmv;*.mp4";
            DialogResult res = fileDialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                this.Text = "WinWallpaper ["+filePath+"]";
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

        private void Player(IntPtr parentHwnd)
        {
            // 播放窗口 没有被创建
            if (playerForm == null || playerForm.IsDisposed)
            {
                // 播放器没有创建 或 播放文件不存在
                if (player == null || !File.Exists(player.FilePath))
                {
                    MessageBox.Show("视频文件不存在!");
                    return;
                }
                else // 可以载入播放窗口
                {
                    playerForm = new PlayerForm(player.FilePath);
                    // 获取播放窗口的句柄
                    IntPtr child = playerForm.Handle;
                    // 设置播放窗口
                    if (IntPtr.Zero == Win32.User32.SetParent(child, parentHwnd))
                    {
                        MessageBox.Show("error", "error");
                    }
                }
            }
            else
            {
                // 切换预览视频
                if (!player.FilePath.Equals(playerForm.p.FilePath))
                {
                    playerForm.p.Post(MCIPlayer.Cmd.close);
                    playerForm.p.Replace(player.FilePath);
                    playerForm.p.Post(MCIPlayer.Cmd.play);
                    playerForm.p.Post(MCIPlayer.Cmd.loops);
                }
            }
            // 显示播放窗口
            playerForm.Show();
        }
    }
}
