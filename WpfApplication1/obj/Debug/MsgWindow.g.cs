﻿#pragma checksum "..\..\MsgWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DD1F509C943F70863445BABCC2CFDB14"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5456
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApplication1 {
    
    
    /// <summary>
    /// MsgWindow
    /// </summary>
    public partial class MsgWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\MsgWindow.xaml"
        internal System.Windows.Controls.Border top;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\MsgWindow.xaml"
        internal System.Windows.Controls.ListBox listMsg;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\MsgWindow.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\MsgWindow.xaml"
        internal System.Windows.Controls.Button button1;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApplication1;component/msgwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MsgWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\MsgWindow.xaml"
            ((WpfApplication1.MsgWindow)(target)).PreviewMouseMove += new System.Windows.Input.MouseEventHandler(this.ResetCursor);
            
            #line default
            #line hidden
            return;
            case 2:
            this.top = ((System.Windows.Controls.Border)(target));
            
            #line 6 "..\..\MsgWindow.xaml"
            this.top.MouseMove += new System.Windows.Input.MouseEventHandler(this.DisplayResizeCursor);
            
            #line default
            #line hidden
            
            #line 6 "..\..\MsgWindow.xaml"
            this.top.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Resize);
            
            #line default
            #line hidden
            return;
            case 3:
            this.listMsg = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.button1 = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\MsgWindow.xaml"
            this.button1.Click += new System.Windows.RoutedEventHandler(this.button1_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
