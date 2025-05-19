using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DesktopFrien.data;

namespace DesktopFrien.behaviours.movement
{
    internal class SimpleMovementBehaviour : IMovementBehaviour
    {
        private const double SPEED = 2.0;            // constant movement speed
        private const double MAX_TURN_RATE = 0.1;    // radians per step
        private double _x = 200;
        private double _y = 200;
        private double _angle;
        private readonly double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private readonly double _screenHeight = SystemParameters.PrimaryScreenHeight;
        private Random _random = new Random();

        public SimpleMovementBehaviour()
        {
            // initialize with a random heading
            _angle = _random.NextDouble() * 2 * Math.PI;
        }

        public override Point2D GetNextValue()
        {
            // jitter the heading slightly for smooth nonlinear path
            double delta = (_random.NextDouble() * 2 - 1) * MAX_TURN_RATE;
            _angle += delta;

            // compute next position at constant speed
            _x += SPEED * Math.Cos(_angle);
            _y += SPEED * Math.Sin(_angle);

            // if out of bounds, wrap around or reflect
            if (_x < 0) _x += _screenWidth;
            if (_x >= _screenWidth) _x -= _screenWidth;
            if (_y < 0) _y += _screenHeight;
            if (_y >= _screenHeight) _y -= _screenHeight;

            return new Point2D((int)Math.Round(_x), (int)Math.Round(_y));
        }
    }
}
