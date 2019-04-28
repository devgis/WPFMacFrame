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
using System.Collections;
using System.Collections.ObjectModel;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : MyMacClass
    {
        ObservableCollection<People> items = new ObservableCollection<People>();

        public Window2()
        {
            InitializeComponent();
            PopulateItems();
            listPeople.ItemsSource = items;
        }

        public void PopulateItems()
        {
            items.Add(new People("Jammer","Jammer's Photo"));
            items.Add(new People("John", "John's Photo"));
            items.Add(new People("Jane", "Jane's Photo"));
            items.Add(new People("Robert", "Robert's Photo"));
            items.Add(new People("Jezzer", "Jezzer's Photo"));
        }
    }

    public class People
    {
        private string name;
        private string photo;
        public People(string _name, string _photo)
        {
            this.name = _name;
            this.photo = _photo;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }
    }
}
