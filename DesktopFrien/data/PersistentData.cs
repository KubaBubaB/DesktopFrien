using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFrien.data
{
    internal class PersistentData
    {
        public Boolean _isVisible {get; set;}
        public Stats _stats {get; set;}

        public PersistentData()
        {
            _isVisible = true; // Default visibility
            _stats = new Stats(); // Initialize stats
        }

        public PersistentData(bool isVisible, Stats stats)
        {
            _isVisible = isVisible;
            _stats = stats;
        }
    }
}
