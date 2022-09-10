using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
using System.IO;
namespace Hag.Misc
{
    class Movement : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Misc());
        }
        IEnumerator Misc()
        {
            for (; ; )
            {
                try
                {
                    if (Input.GetKeyDown(Globals.Config.Movement.FreeCamKey))
                        Globals.Config.Movement.FreeCam = !Globals.Config.Movement.FreeCam;

                    Player.player.look.isOrbiting = Globals.Config.Movement.FreeCam && !Globals.Spied;
                    Player.player.look.isTracking = Globals.Config.Movement.FreeCam && !Globals.Spied;
                }
                catch { }
                yield return new WaitForEndOfFrame();
            }
        }
     }
}
