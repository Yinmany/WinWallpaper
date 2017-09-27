using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWallpaper.Utils
{
    /// <summary>
    /// 桌面壁纸工具
    /// </summary>
    public static class WallpaperUtils
    {
        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetWorkerW()
        {
            // 获取
            IntPtr windowHandle = Win32.User32.FindWindow("Progman", null);

            IntPtr zero;
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
    }
}
