using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Menu;
using Hag.Hooks;
using Hag.Aimbot;
namespace Hag
{
    public static class Loader
    {

        public static void Load()
        {
            Hackobject.AddComponent<Globals>();
            Hackobject.AddComponent<RenderMenu>();
            Hackobject.AddComponent<Hag.Esp.Caching>();
            Hackobject.AddComponent<Hag.Esp.Updating>();
            Hackobject.AddComponent<Aimbot.Aimbot>();
            Hackobject.AddComponent<Misc.Weapon>();

            Hackobject.AddComponent<TakeScreenshot>();
     //       Hackobject.AddComponent<Simulate>();
    //        Hackobject.AddComponent<UpdateCrosshair>();
            Hackobject.AddComponent<Fire>();
        //    Hackobject.AddComponent<AddPlayer>();
            GameObject.DontDestroyOnLoad(Hackobject);
        }
        private static GameObject Hackobject = new GameObject();
        // usablen.project = create shoot function
        // usablegun.fire = shoot function
        // base.player.look.aim.forward
        // ov_playerinput in thanking has spinbot
    }
}
