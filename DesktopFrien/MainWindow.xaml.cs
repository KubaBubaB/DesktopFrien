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
using DesktopFrien.data;

namespace DesktopFrien
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BitmapImage _frienImage;
        private DispatcherTimer movementTimer;
        private IMovementBehaviour _movementBehaviour = new SimpleMovementBehaviour();
        private DispatcherTimer statsTimer;

        private PersistentData _persData;

        private Image honeyImage;
        private bool isDraggingHoney = false;


        public MainWindow()
        {
            InitializeComponent();

            _frienImage = new BitmapImage(new Uri("pack://application:,,,/media/FrienImage.png"));
            FrienImage.Source = _frienImage;

            bool doesSaveExists = false; // Check if save exists (placeholder logic, implement actual check)
            if (doesSaveExists)
            {
                // load save file and set data
            }
            else
            {
                _persData = new PersistentData();
            }

            StartStatsTimer();
            StartMovementTimer();
        }

        //region STATS REGION

        private void StartStatsTimer()
        {
            statsTimer = new DispatcherTimer();
            statsTimer.Interval = TimeSpan.FromMinutes(1);
            statsTimer.Tick += (s, e) => UpdateStatsMenu();
            statsTimer.Start();
        }

        private void UpdateStatsMenu()
        {
            // TODO: Implement saving stats to file after update
            if (BoredomStat != null)
                BoredomStat.Header = $"Boredom: {_persData._stats._boredom}";

            if (EepynessStat != null)
                EepynessStat.Header = $"Eepyness: {_persData._stats._eepyness}";

            if (HungeerStat != null)
                HungeerStat.Header = $"Hungeer: {_persData._stats._hunger}";
        }

        // endregion

        // region MOVEMENT

        private void StartMovementTimer()
        {
            // Start animation
            movementTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(5)
            };
            movementTimer.Tick += MoveFrien;
            movementTimer.Start();
        }

        private void MoveFrien(object sender, EventArgs e)
        {
            var nextVal = _movementBehaviour.GetNextValue();

            Left = nextVal.X;
            Top = nextVal.Y;
        }

        // endregion

        // region FOOD

        private void SpawnHoney_Click(object sender, RoutedEventArgs e)
        {
            var honey = new HoneyWindow(GoTowardFood);
            honey.Show();
        }

        private void GoTowardFood(Func<Point2D> foodPositionGetter)
        {
            movementTimer.Stop();
            _movementBehaviour = new TargetedMovementBehaviour(_movementBehaviour.GetNextValue(), foodPositionGetter);
            movementTimer.Start();
        }

        // endregion

        // region SETTINGS AND EXIT

        private void EnableFrien(object sender, RoutedEventArgs e)
        {
            if (_persData._isVisible)
            {
                _persData._isVisible = false;
                this.Hide();
            }
            else
            {
                _persData._isVisible = true;
                this.Show();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            TrayIcon.Dispose(); // Cleanly remove tray icon
            Application.Current.Shutdown();
        }

        // endregion
    }
}