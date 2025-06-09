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

        public string _speedOfStats;

        public PersistentData()
        {
            _isVisible = true; // Default visibility
            _stats = new Stats(); // Initialize stats
            _speedOfStats = "normal"; // Default speed of stats
        }

        public PersistentData(bool isVisible, Stats stats, string speedOfStats)
        {
            _isVisible = isVisible;
            _stats = stats;
            if (speedOfStats != "slow" && speedOfStats != "fast")
            {
                speedOfStats = "normal"; // Default to normal if invalid input
            }
            _speedOfStats = speedOfStats;
        }
    }
}
