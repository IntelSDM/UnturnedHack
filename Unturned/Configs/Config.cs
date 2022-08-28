using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Config
    {
        
        public Colours Colours = new Colours();
        public Hag.Configs.Menu Menu = new Hag.Configs.Menu();
        public Hag.Configs.Zombie Zombie = new Hag.Configs.Zombie();
        public Hag.Configs.Player Player = new Hag.Configs.Player();
        public Hag.Configs.EntityAimbot ZombieAimbot = new Hag.Configs.EntityAimbot();
        public Hag.Configs.Aimbot Aimbot = new Hag.Configs.Aimbot();
    }
}
