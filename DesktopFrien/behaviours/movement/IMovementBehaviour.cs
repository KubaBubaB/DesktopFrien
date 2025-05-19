using DesktopFrien.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFrien.behaviours.movement
{
    abstract class IMovementBehaviour
    {
        public abstract Point2D GetNextValue();
    }
}
