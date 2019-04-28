using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApplication1
{
    public class MyMacClass:Window
    {

        private const int WM_SYSCOMMAND = 0x112;
        public const int WM_LBUTTONUP = 0x0202;
        private HwndSource hs;
        IntPtr retInt = IntPtr.Zero;
        public double relativeClip = 10;

        public MyMacClass()
        {
            this.Loaded += delegate
            {
                InitializeEvent();
            };
            this.SourceInitialized +=new EventHandler(MyMacClass_SourceInitialized);
        }

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

        #region 这一部分用于得到元素相对于窗体的位置
        public Size GetElementPixelSize(UIElement element)
        {
            Matrix transformToDevice;
            var source = PresentationSource.FromVisual(element);
            if (source != null)
                transformToDevice = source.CompositionTarget.TransformToDevice;
            else
                using (var source1 = new HwndSource(new HwndSourceParameters()))
                    transformToDevice = source1.CompositionTarget.TransformToDevice;

            if (element.DesiredSize == new Size())
                element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return (Size)transformToDevice.Transform((Vector)element.DesiredSize);
        }
        #endregion

        #region 这一部分是四个边加上四个角
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
        #endregion

        #region 用于改变窗体大小
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(hs.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
        }
        #endregion

        #region 为元素注册事件
        private void InitializeEvent()
        {
            ControlTemplate baseWindowTemplate = (ControlTemplate)App.Current.Resources["MacWindowTemplate"];
            //set up background image
            Button skinBtn = (Button)baseWindowTemplate.FindName("SkinBtn", this);
            skinBtn.Click += delegate
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    ImageBrush brush = (ImageBrush)baseWindowTemplate.FindName("MyBgImg",this);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(dlg.FileName, UriKind.Absolute);
                    bitmap.EndInit();
                    brush.ImageSource = bitmap;
                }
            };

            Button closeBtn = (Button)baseWindowTemplate.FindName("CloseBtn", this);
            closeBtn.Click += delegate
            {
                this.Close();
            };

            Button minBtn = (Button)baseWindowTemplate.FindName("MinBtn", this);
            minBtn.Click += delegate
            {
                this.WindowState = WindowState.Minimized;
            };

            Button maxBtn = (Button)baseWindowTemplate.FindName("MaxBtn", this);
            maxBtn.Click += delegate
            {
                this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
            };

            Border borderClip = (Border)baseWindowTemplate.FindName("MacWindowFrame", this);

            borderClip.MouseMove += delegate
            {
                DisplayResizeCursor(null,null);
            };

            borderClip.PreviewMouseDown += delegate
            {
                Resize(null,null);
            };

            borderClip.MouseLeftButtonDown += delegate
            {
                DragMove();
            };

            this.PreviewMouseMove += delegate
            {
                ResetCursor(null,null);
            };
        }
        #endregion

        #region 重写的DragMove，以便解决利用系统自带的DragMove出现Exception的情况
        public new void DragMove()
        {
            if (this.WindowState == WindowState.Normal)
            {
                SendMessage(hs.Handle, WM_SYSCOMMAND, (IntPtr)0xf012, IntPtr.Zero);
                SendMessage(hs.Handle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
            }
        }
        #endregion

        #region 显示拖拉鼠标形状
        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            Point pos = Mouse.GetPosition(this);
            double x = pos.X;
            double y = pos.Y;
            double w = this.ActualWidth;  //注意这个地方使用ActualWidth,才能够实时显示宽度变化
            double h = this.ActualHeight;

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
        #endregion

        #region  还原鼠标形状
        private void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        #endregion

        #region 判断区域，改变窗体大小
        private void Resize(object sender, MouseButtonEventArgs e)
        {
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
        #endregion
    }
}
