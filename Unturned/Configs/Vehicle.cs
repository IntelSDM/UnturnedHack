using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Vehicle
    {
        public bool Enabled = true;
        public bool Name = true;
        public bool Distance = true;
        public int MaxDistance = 3000;
        public bool DrawOwnVehicles = true;
        public bool DrawUnlocked = true;
        public bool DrawLocked = false;
        public bool LockedStatus = true;

        public bool IgnoreOwnedVehiclesInList = true;
    }
}
