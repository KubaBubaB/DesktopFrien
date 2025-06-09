using DesktopFrien.behaviours.movement;
using DesktopFrien.data;
using System.Media;
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
        private double probabilityOfStatsIncreaseEverySecond = 9e-4; // full stats in aprox 3 hours
        private PersistentData _persData;

        private Image honeyImage;
        private bool isDraggingHoney = false;

        private List<Window> _windows = new List<Window>();

        private Random random = new Random();


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

            SetSettings();
            StartStatsTimer();
            StartMovementTimer();
        }

        //region STATS REGION

        private void StartStatsTimer()
        {
            statsTimer = new DispatcherTimer();
            statsTimer.Interval = TimeSpan.FromSeconds(1);
            statsTimer.Tick += (s, e) => UpdateStatsMenu();
            statsTimer.Start();
        }

        private void UpdateStatsMenu()
        {
            if (_persData == null || _persData._stats == null) return; // Ensure data is loaded
            if (random.NextDouble() <= probabilityOfStatsIncreaseEverySecond)
            {
                _persData._stats.UpdateStats();
            }
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

        private void SetMovement(IMovementBehaviour movementBehaviour)
        {
            movementTimer.Stop();
            _movementBehaviour = movementBehaviour;
            movementTimer.Start();
        }

        // endregion

        // region FOOD

        private void SpawnHoney_Click(object sender, RoutedEventArgs e)
        {
            var honey = new HoneyWindow(GoTowardFood);
            _windows.Add(honey);
            honey.Show();
        }

        private void GoTowardFood(Func<Point2D> foodPositionGetter, Window foodWindow)
        {
            SetMovement(new TargetedMovementBehaviour(_movementBehaviour.GetNextValue(), foodPositionGetter, EatFood, foodWindow));
        }

        private void EatFood(Window foodWindow)
        {
            movementTimer.Stop();
            foodWindow.Close(); 
            _movementBehaviour = new SimpleMovementBehaviour(_movementBehaviour.GetCurrentPosition());
            SoundPlayer player = new SoundPlayer("media/honeyChew.wav");
            player.Load();
            player.Play();
            movementTimer.Start();
            _persData._stats._hunger -= 10;
        }

        // endregion

        // region SETTINGS AND EXIT

        private void SpeedRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi)
            {
                switch (mi.Tag)
                {
                    case "Slow":
                        SpeedSlowRadio.IsChecked = true;
                        probabilityOfStatsIncreaseEverySecond = 3e-4; // full stats in aprox 8 hours
                        break;
                    case "Normal":
                        SpeedNormalRadio.IsChecked = true;
                        probabilityOfStatsIncreaseEverySecond = 9e-4; // full stats in aprox 3 hours
                        break;
                    case "Fast":
                        SpeedFastRadio.IsChecked = true;
                        probabilityOfStatsIncreaseEverySecond = 0.003; // full stats in aprox 1 hour
                        break;
                }
            }
            else if (sender is RadioButton radioButt)
            {
                switch (radioButt.Tag)
                {
                    case "Slow":
                        SpeedSlowRadio.IsChecked = true;
                        probabilityOfStatsIncreaseEverySecond = 3e-4; // full stats in aprox 8 hours
                        break;
                    case "Normal":
                        SpeedNormalRadio.IsChecked = true;
                        probabilityOfStatsIncreaseEverySecond = 9e-4; // full stats in aprox 3 hours
                        break;
                    case "Fast":
                        SpeedFastRadio.IsChecked = true;
                        probabilityOfStatsIncreaseEverySecond = 0.003; // full stats in aprox 1 hour
                        break;
                }
            }
        }

        private void SetSettings()
        {
            SetSpeed(_persData._speedOfStats);
        }

        private void SetSpeed(string speed)
        {
            switch (speed)
            {
                case "slow":
                    SpeedSlowRadio.IsChecked = true;
                    break;
                
                case "fast":
                    SpeedFastRadio.IsChecked = true;
                    break;
                default:
                    SpeedNormalRadio.IsChecked = true;
                    break;
            }
        }

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