using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class MsgWindow : Window
    {
        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hs;
        IntPtr retInt = IntPtr.Zero;

        public MsgWindow()
        {
            InitializeComponent();
            this.SourceInitialized += new EventHandler(WSInitialized);
        }

        void WSInitialized(object sender, EventArgs e)
        {
            hs = PresentationSource.FromVisual(this) as HwndSource;
            hs.AddHook(new HwndSourceHook(WndProc));
        }

       public double relativeClip = 10;

        public enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(hs.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        private void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Resize(object sender, MouseButtonEventArgs e)
        {
            Border clickedBorder = sender as Border;

            Point pos = Mouse.GetPosition(this);
            double x = pos.X;
            double y = pos.Y;
            double w = this.ActualWidth;
            double h = this.ActualHeight;

            if (x <= relativeClip & y <= relativeClip) // left top
            {
                this.Cursor = Cursors.SizeNWSE;
                ResizeWindow(ResizeDirection.TopLeft);
            }
            if (x >= w - relativeClip & y <= relativeClip) //right top
            {
                this.Cursor = Cursors.SizeNESW;
                ResizeWindow(ResizeDirection.TopRight);
            }

            if (x >= w - relativeClip & y >= h - relativeClip) //bottom right
            {
                this.Cursor = Cursors.SizeNWSE;
                ResizeWindow(ResizeDirection.BottomRight);
            }

            if (x <= relativeClip & y >= h - relativeClip)  // bottom left
            {
                this.Cursor = Cursors.SizeNESW;
                ResizeWindow(ResizeDirection.BottomLeft);
            }

            if ((x >= relativeClip & x <= w - relativeClip) & y <= relativeClip) //top
            {
                this.Cursor = Cursors.SizeNS;
                ResizeWindow(ResizeDirection.Top);
            }

            if (x >= w - relativeClip & (y >= relativeClip & y <= h - relativeClip)) //right
            {
                this.Cursor = Cursors.SizeWE;
                ResizeWindow(ResizeDirection.Right);
            }

            if ((x >= relativeClip & x <= w - relativeClip) & y > h - relativeClip) //bottom
            {
                this.Cursor = Cursors.SizeNS;
                ResizeWindow(ResizeDirection.Bottom);
            }

            if (x <= relativeClip & (y <= h - relativeClip & y >= relativeClip)) //left
            {
                this.Cursor = Cursors.SizeWE;
                ResizeWindow(ResizeDirection.Left);
            }
        }

        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            Border clickBorder = sender as Border;

            Point pos = Mouse.GetPosition(this);
            double x = pos.X;
            double y = pos.Y;
            double w= this.ActualWidth;
            double h= this.ActualHeight;

            this.label1.Content = x + "--" + y;

            if (x <= relativeClip & y <= relativeClip) // left top
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            if (x >= w - relativeClip & y <= relativeClip) //right top
            {
                this.Cursor = Cursors.SizeNESW;
            }

            if (x >= w - relativeClip & y >= h - relativeClip) //bottom right
            {
                this.Cursor = Cursors.SizeNWSE;
            }

            if (x <= relativeClip & y >= h - relativeClip)  // bottom left
            {
                this.Cursor = Cursors.SizeNESW;
            }

            if ((x >= relativeClip & x <= w - relativeClip) & y <= relativeClip) //top
            {
                this.Cursor = Cursors.SizeNS;
            }

            if (x >= w - relativeClip & (y >= relativeClip & y <= h - relativeClip)) //right
            {
                this.Cursor = Cursors.SizeWE;
            }

            if ((x >= relativeClip & x <= w - relativeClip) & y > h - relativeClip) //bottom
            {
                this.Cursor = Cursors.SizeNS;
            }

            if (x <= relativeClip & (y <= h - relativeClip & y >= relativeClip)) //left
            {
                this.Cursor = Cursors.SizeWE;
            }
        }

        Dictionary<int, int> messages = new Dictionary<int, int>();

        //private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    Debug.Print(string.Format("窗体消息: {0}, wParam: {1} , lParam: {2}", msg.ToString(), wParam.ToString(), lParam.ToString()));
        //    if (messages.ContainsKey(msg) == false)
        //    {
        //        messages.Add(msg, msg);
        //        // 添加接收到的WIndows信息到ListBox中
        //        listMsg.Items.Add(msg);
        //    }
        //    return new IntPtr(0);
        //}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
        }
        #region 这一部分用于最大化时不遮蔽任务栏
        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {

            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {

                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        /// <summary>
        /// POINT aka POINTAPI
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int x;
            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            /// Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// 窗体大小信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };
        /// <summary> Win32 </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            /// <summary> Win32 </summary>
            public int left;
            /// <summary> Win32 </summary>
            public int top;
            /// <summary> Win32 </summary>
            public int right;
            /// <summary> Win32 </summary>
            public int bottom;

            /// <summary> Win32 </summary>
            public static readonly RECT Empty = new RECT();

            /// <summary> Win32 </summary>
            public int Width
            {
                get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
            }
            /// <summary> Win32 </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary> Win32 </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }


            /// <summary> Win32 </summary>
            public RECT(RECT rcSrc)
            {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }

            /// <summary> Win32 </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }
            /// <summary> Return a user friendly representation of this struct </summary>
            public override string ToString()
            {
                if (this == RECT.Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }

            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }


            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            /// <summary>
            /// </summary>            
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            /// <summary>
            /// </summary>            
            public RECT rcMonitor = new RECT();

            /// <summary>
            /// </summary>            
            public RECT rcWork = new RECT();

            /// <summary>
            /// </summary>            
            public int dwFlags = 0;
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

        private void MyMacClass_SourceInitialized(object sender, EventArgs e)
        {
            hs = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            hs.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:/* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
                case 0x0086:
                    if (wParam == (IntPtr)Win32.WM_FALSE)
                    {
                        return (IntPtr)Win32.WM_TRUE;
                    }
                    break;
                case 0x0085:
                    return (System.IntPtr)0;
                    break;
                case 0x0084:
                    #region 注释部分
                    //int x = lParam.ToInt32() & 0xffff;
                    //int y = lParam.ToInt32() >> 16;
                    //double w = this.ActualWidth;
                    //double h = this.ActualHeight;

                    //if (x <= relativeClip & y <= relativeClip) // left top
                    //{
                    //    return (IntPtr)Win32.HTTOPLEFT;
                    //}

                    //else if (x >= w - relativeClip & y <= relativeClip) //right top
                    //{
                    //    return (IntPtr)Win32.HTTOPRIGHT;
                    //}

                    //else if (x >= w - relativeClip & y >= h - relativeClip) //bottom right
                    //{
                    //    return (IntPtr)Win32.HTBOTTOMRIGHT;
                    //}

                    //else if (x <= relativeClip & y >= h - relativeClip)  // bottom left
                    //{
                    //    return (IntPtr)Win32.HTBOTTOMLEFT;
                    //}

                    //else if ((x >= relativeClip & x <= w - relativeClip) & y <= relativeClip) //top
                    //{
                    //    return (IntPtr)Win32.HTTOP;
                    //}

                    //else if (x >= w - relativeClip & (y >= relativeClip & y <= h - relativeClip)) //right
                    //{
                    //    return (IntPtr)Win32.HTRIGHT;
                    //}

                    //else if ((x >= relativeClip & x <= w - relativeClip) & y > h - relativeClip) //bottom
                    //{
                    //    return (IntPtr)Win32.HTBOTTOM;
                    //}

                    //else if (x <= relativeClip & (y <= h - relativeClip & y >= relativeClip)) //left
                    //{
                    //    return (IntPtr)Win32.HTLEFT;
                    //}
                    //else
                    //{
                    //    return (IntPtr)Win32.HTCAPTION;
                    //}
                    #endregion
                    break;
                case 0x0083:
                    return (System.IntPtr)0;
                    break;
                default: break;
            }
            return (System.IntPtr)0;
        }

    }
}
