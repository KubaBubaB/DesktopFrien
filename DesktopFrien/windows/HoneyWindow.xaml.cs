using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using DesktopFrien.data;

namespace DesktopFrien
{
    public partial class HoneyWindow : Window, IFood
    {
        private DispatcherTimer followTimer;
        private Action<Func<Point2D>> activateFrienFollowingFood;

        public HoneyWindow(Action<Func<Point2D>> activateFrienFollowingFood)
        {
            InitializeComponent();

            this.MouseLeftButtonDown += StopFollowing;

            followTimer = new DispatcherTimer();
            followTimer.Interval = TimeSpan.FromMilliseconds(5);
            followTimer.Tick += FollowMouse;
            followTimer.Start();
            activateFrienFollowingFood(GetPoint);
        }

        private Point2D GetPoint()
        {
            return new Point2D(this.Left, this.Top);
        }

        private void FollowMouse(object sender, EventArgs e)
        {
            var mouse = Control.MousePosition;
            this.Left = mouse.X - this.Width / 2;
            this.Top = mouse.Y - this.Height / 2;
        }

        private void StopFollowing(object sender, MouseButtonEventArgs e)
        {
            followTimer.Stop();
            this.MouseLeftButtonDown -= StopFollowing;
            // Now it stays where clicked
        }
    }
}
