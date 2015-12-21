using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MSCFB;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CompoundFile CompoundFile { get; set; }
        public MainWindow()
        {
            using (var fs = File.OpenRead("empty.msg"))
            {
                CompoundFile = new CompoundFile(fs);


            }
            InitializeComponent();
            
        }
    }
}
