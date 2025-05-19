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
using System.Windows.Threading;
using DesktopFrien.behaviours.movement;

namespace DesktopFrien
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BitmapImage _frienImage;
        private readonly DispatcherTimer _movementTimer;
        private IMovementBehaviour _movementBehaviour = new SimpleMovementBehaviour();

        public MainWindow()
        {
            InitializeComponent();

            // Load image from resources
            _frienImage = new BitmapImage(new Uri("pack://application:,,,/media/FrienImage.png"));
            FrienImage.Source = _frienImage;

            // Start animation
            _movementTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(5) // ~60 FPS
            };
            _movementTimer.Tick += MoveFrien;
            _movementTimer.Start();
        }

        private void MoveFrien(object sender, EventArgs e)
        {
            var nextVal = _movementBehaviour.GetNextValue();

            Left = nextVal.X;
            Top = nextVal.Y;
        }
    }
}