using System.Windows;

namespace GraphicOverlayLabels
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MapViewModel();
            InitializeComponent();
        }
    }
}