using Photon.Math;
using System.Windows;

namespace Photon.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Matrix4x4 m = Matrix4x4.identity;
            Console.WriteLine(m);
        }
    }
}