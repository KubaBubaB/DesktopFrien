using DesktopFrien.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopFrien.behaviours.movement
{
    internal class TargetedMovementBehaviour : IMovementBehaviour
    {
        private const double SPEED = 2.0;
        private double _x;
        private double _y;
        private Func<Point2D> targetPositionGetter;
        private Action<Window> reachMovementTargetAction;
        private Window targetWindow;


        public TargetedMovementBehaviour(Point2D currentPosition, Func<Point2D> targetPositionGetter, Action<Window> reachMovementTargetAction, Window targetWindow)
        {
            this._x = currentPosition.X;
            this._y = currentPosition.Y;
            this.targetPositionGetter = targetPositionGetter;
            this.reachMovementTargetAction = reachMovementTargetAction;
            this.targetWindow = targetWindow;
        }

        public override Point2D GetCurrentPosition()
        {
            return new Point2D(_x, _y);
        }

        public override Point2D GetNextValue()
        {
            // calculate the direction vector towards the target
            var pos = targetPositionGetter();
            double directionX = pos.X - _x;
            double directionY = pos.Y - _y;
            double distance = Math.Sqrt(directionX * directionX + directionY * directionY);

            if (distance <= 1) // close enough to the target
            {
                reachMovementTargetAction?.Invoke(targetWindow); // invoke the end movement action
                return new Point2D(_x, _y); // no movement needed
            }
            else {
                // normalize the direction vector
                directionX /= distance;
                directionY /= distance;
                // move towards the target
                _x += directionX * SPEED;
                _y += directionY * SPEED;
                // return the new position
                return new Point2D(_x, _y);
            }
        }
    }
}
