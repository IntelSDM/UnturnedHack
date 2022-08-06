using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Menu;
namespace Hag
{
    public static class Loader
    {

        public static void Load()
        {
            Hackobject.AddComponent<Globals>();
            Hackobject.AddComponent<>(RenderMenu)();
        }
        private static GameObject Hackobject = new GameObject();
        // usablen.project = create shoot function
        // usablegun.fire = shoot function
        // base.player.look.aim.forward
    }
}
