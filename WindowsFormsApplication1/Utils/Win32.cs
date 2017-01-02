using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinWallpaper.Utils
{
    // win32api
    public class Win32
    {


        public class User32
        {
            /// <summary>
            ///    查找子窗口句柄
            /// </summary>
            /// <param name="hwndParent">要查找窗口的父句柄</param>
            /// <param name="hwndChildAfter">从这个窗口后开始查找</param>
            /// <param name="className">窗口类名</param>
            /// <param name="title">窗口标题</param>
            /// <returns>找到返回窗口句柄，没找到返回0</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string title);

            /// <summary>
            /// 改变指定子窗口的父窗口
            /// </summary>
            /// <param name="hwndChild">子窗口句柄</param>
            /// <param name="newParent">新的父窗口句柄 如果该参数是NULL，则桌面窗口就成为新的父窗口</param>
            /// <returns>如果函数成功，返回值为子窗口的原父窗口句柄；如果函数失败，返回值为NULL</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr SetParent(IntPtr hwndChild, IntPtr newParent);

            [DllImport("user32.dll")]  //使用user32.dll中提供的两个函数实现显示和激活  
            public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
            public const int WS_SHOWNORMAL = 1; 

            // 激活窗口
            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
            
        }
        public class Winmm
        {
            /// <summary>
            ///  mciSendString是用来播放多媒体文件的API指令，可以播放MPEG,AVI,WAV,MP3,等等
            /// </summary>
            /// <param name="lpszCommand">要发送的命令字符串。字符串结构是:[命令][设备别名][命令参数]</param>
            /// <param name="lpszReturnString">返回信息的缓冲区,为一指定了大小的字符串变量</param>
            /// <param name="cchReturn">缓冲区的大小,就是字符变量的长度</param>
            /// <param name="hwndCallback">回调方式，一般设为零</param>
            /// <returns>函数执行成功返回零，否则返回错误代码</returns>
            [DllImport(("winmm.dll "), EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
            public static extern int mciSendString(string lpszCommand, string lpszReturnString,
                        uint cchReturn, int hwndCallback);
        }

        public class Kernel32
        {
            /// <summary>
            /// 获取短路径
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="short_path">返回短路径的缓冲区</param>
            /// <param name="short_len">缓冲区长度</param>
            /// <returns></returns>
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetShortPathName(
                [MarshalAs(UnmanagedType.LPTStr)]string path,
                [MarshalAs(UnmanagedType.LPTStr)]StringBuilder short_path,
                int short_len
                ); 
        }

    }



}
