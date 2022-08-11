using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Player
    {
        public bool Enable = true;
        public bool Name = true;
        public bool Weapon = true;
        public bool Distance = true;
        public int MaxDistance = 2000;
        public bool Box = true;
        public bool Box3D = false;
        public bool FillBox = true;
        public bool Skeleton = true;
        public bool OnlyDrawVisible = false;
        public bool Chams;
        public bool Glow;
    }
}
