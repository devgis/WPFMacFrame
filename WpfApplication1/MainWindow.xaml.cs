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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : MyMacClass
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void showWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var anotherWindow = new AnotherWindow();
            anotherWindow.ShowDialog();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Window2 win = new Window2();
            win.Show();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Brushes.White, 1);
            Rect rect = new Rect(20, 20, 500, 600);
            drawingContext.DrawRectangle(null, pen, rect);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SpashWindow w = new SpashWindow();
            w.Show();
        }
    }
}
