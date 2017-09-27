using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WinWallpaper.Utils;
namespace WinWallpaper.Utils
{
    public class MCIPlayer
    {
        // 播放文件路径
        public string FilePath { get; set; }

        // 播放视频的 别名(后续操作别名即可)
        public string Alias  { get; set; }

        // 播放视频的 父窗口
        public IntPtr Parent { get; set; }

        public Rectangle Size { get; set; }

        // 执行的命令
        private string cmd;

        public enum Cmd
        {
            play,// 播放
            playByFullScreen, // 全屏播放
            pause,// 暂停
            resume, // 继续
            stop, // 停止
            close, // 关闭
            step, // 前进到下一个位置
            stepByReverse, // 后退到上一个位置
            stepBy,// 前进或后退指定的位置（小于0为后退）
            loops //循环播放
        }

        public enum GetInfo
        {
            position, // 当前播放位置
            length, // 获取媒体总长度
            mode // 获取播放信息
        }

        public MCIPlayer(string path,string alias,IntPtr parent,Rectangle rect)
        {
            FilePath = path;
            Alias = alias;
            Parent = parent;
            Size = rect;
            cmd = string.Format("open {0} alias {1} parent {2} style child", path, alias, parent.ToInt32());
            SendCmd(cmd);
            SetSize(rect);
        }

        /// <summary>
        /// 替换视频文件
        /// </summary>
        /// <param name="path">视频路径</param>
        public void Replace(string path)
        {
            FilePath = path;
            cmd = string.Format("open {0} alias {1} parent {2} style child", FilePath, Alias, Parent.ToInt32());
            SendCmd(cmd);
            SetSize(Size);
        }

        /// <summary>
        /// 设置播放的范围
        /// </summary>
        /// <param name="rect">矩形范围</param>
        public void SetSize(Rectangle rect)
        {
            cmd = string.Format("put {0} window at {1} {2} {3} {4}", Alias, rect.X, rect.Y, rect.Width, rect.Height);
            SendCmd(cmd);
        }

        public void Post(Cmd cmd,params string[] args)
        {
            switch (cmd)
            {
                case Cmd.playByFullScreen:
                    SendCmd(string.Format("play {0} fullscreen", Alias));
                    break;
                case Cmd.stepByReverse:
                    SendCmd(string.Format("step {0} reverse", Alias));
                    break;
                case Cmd.stepBy:
                    SendCmd(string.Format("step {0} by {1}", Alias, args[0]));
                    break;
                case Cmd.loops:
                    SendCmd(string.Format("play {0} repeat", Alias));
                    break;
                default:
                    SendCmd(string.Format("{0} {1}", cmd.ToString(), Alias));
                    break;
            }
            
        }


        /// <summary>
        /// 获取当前播放位置
        /// </summary>
        /// <returns></returns>
        public string Get(GetInfo info)
        {
            string idx = "";
            SendCmd(string.Format("status {0} {1}", Alias, info.ToString()), idx, 100, 0);
            return idx;
        }


        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="c">命令</param>
        private void SendCmd(string lpszCommand, string lpszReturnString,
                        uint cchReturn, int hwndCallback)
        {

            Win32.Winmm.mciSendString(lpszCommand, lpszReturnString, cchReturn, hwndCallback);
        }
        private void SendCmd(string lpszCommand)
        {

            Win32.Winmm.mciSendString(lpszCommand, null, 0, 0);
        }
    }
}
