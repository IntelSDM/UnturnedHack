using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
using Hag.Esp_Objects;
using System.Collections;
using Hag.Helpers;
namespace Hag.Esp
{
    class Updating : MonoBehaviour
    {
        public static Vector3 WorldPointToScreenPoint(Vector3 worldPoint)
        {
            Vector3 vector = Globals.MainCamera.WorldToScreenPoint(worldPoint);
            vector.y = (float)Screen.height - vector.y;
            return vector;
        }
        void Start()
        {
            StartCoroutine(UpdateZombies());
        }
        IEnumerator UpdateZombies()
        {
            for (; ; )
            {
                try
                {
                    foreach (BaseZombie bz in Globals.ZombieList)
                    {
                        Zombie zombie = bz.Entity;
                        if (zombie == null)
                            continue;
                        bz.Distance = (int)Vector3.Distance(zombie.transform.position, Globals.LocalPlayer.transform.position);
                        bz.Colour = ColourHelper.GetColour("Zombie Colour");
                        bz.W2S = WorldPointToScreenPoint(zombie.transform.position);
                        bz.Alive = !zombie.isDead;
                        bz.Tag = "Zombie";
                        if (zombie.isHyper)
                            bz.Tag = "Hyper Zombie";
                        if (zombie.isRadioactive)
                            bz.Tag = "Radioactive Zombie";
                        if (zombie.isMega)
                            bz.Tag = "Mega Zombie";
                        if (zombie.isBoss)
                            bz.Tag = "Zombie Boss";

                    }
                       
                }
                catch { }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
