﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFrien.data
{
    internal class Stats
    {
        public int _boredom { get; set; }
        public int _hunger { get; set; }
        public int _eepyness { get; set; }

        public Stats()
        {
            _boredom = 0;
            _hunger = 0;
            _eepyness = 0;

        }

        public Stats(int boredom, int hunger, int eepyness)
        {
            _boredom = boredom;
            _hunger = hunger;
            _eepyness = eepyness;
        }

        public void UpdateStats()
        {
            _boredom = Math.Min(100, _boredom + 1);
            _hunger = Math.Min(100, _hunger + 1);
            _eepyness = Math.Min(100, _eepyness + 1);
        }
    }
}
