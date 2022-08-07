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
                        bz.HeadW2S = WorldPointToScreenPoint(Globals.GetLimbPosition(zombie.transform, "Skull"));
                        bz.Alive = !zombie.isDead;
                        bz.Visible = RaycastHelper.IsPointVisible(zombie, Globals.GetLimbPosition(zombie.transform, "Skull"));
                        bz.BoxColour = ColourHelper.GetColour("Zombie Box Colour");
                        bz.FilledBoxColour = ColourHelper.GetColour("Zombie Filled Box Colour");
                        switch (zombie.speciality)
                        {
                            case EZombieSpeciality.ACID:
                                bz.Tag = "Acid Zombie";
                                break;
                            case EZombieSpeciality.SPRINTER:
                                bz.Tag = "Sprinter Zombie";
                                break;
                            case EZombieSpeciality.SPIRIT:
                                bz.Tag = "Spirit Zombie";
                                break;
                            case EZombieSpeciality.MEGA:
                                bz.Tag = "Mega Zombie";
                                break;
                            case EZombieSpeciality.BURNER:
                                bz.Tag = "Fire Zombie";
                                break;
                            case EZombieSpeciality.CRAWLER:
                                bz.Tag = "Crawler Zombie";
                                break;
                            case EZombieSpeciality.FLANKER_STALK:
                                bz.Tag = "Flanker Zombie";
                                break;
                            case EZombieSpeciality.FLANKER_FRIENDLY:
                                bz.Tag = "Friendly Flanker Zombie";
                                break;
                            case EZombieSpeciality.DL_BLUE_VOLATILE:
                            case EZombieSpeciality.DL_RED_VOLATILE:
                                bz.Tag = "Dying Light Zombie";
                                break;
                            case EZombieSpeciality.BOSS_ALL:
                                bz.Tag = "Boss Zombie";
                                break;
                            case EZombieSpeciality.BOSS_ELECTRIC:
                                bz.Tag = "Electric Boss Zombie";
                                break;
                            case EZombieSpeciality.BOSS_ELVER_STOMPER:
                                bz.Tag = "Stomper Boss Zombie";
                                break;
                            case EZombieSpeciality.BOSS_FIRE:
                                bz.Tag = "Fire Boss Zombie";
                                break;
                            case EZombieSpeciality.BOSS_KUWAIT:
                                bz.Tag = "Kuwait Boss Zombie";
                                break;
                            case EZombieSpeciality.BOSS_MAGMA:
                                bz.Tag = "Magma Boss Zombie";
                                break;
                            case EZombieSpeciality.BOSS_NUCLEAR:
                                bz.Tag = "Nuclear Boss Zombie";
                                break;
                            case EZombieSpeciality.BOSS_SPIRIT:
                                bz.Tag = "Spirit Boss Zombie";
                                break;
                            case EZombieSpeciality.BOSS_WIND:
                                bz.Tag = "Wind Boss Zombie";
                                break;
                            case EZombieSpeciality.NORMAL:
                            case EZombieSpeciality.NONE:
                            default:
                                bz.Tag = "Zombie";
                                break;
                        }


                    }
                       
                }
                catch { }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
