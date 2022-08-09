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
                        bz.Colour = ColourHelper.GetColour("Zombie Text Colour");
                        bz.W2S = WorldPointToScreenPoint(zombie.transform.position);
                        bz.HeadW2S = WorldPointToScreenPoint(Globals.GetLimbPosition(zombie.transform, "Skull"));
                        bz.FootW2S = WorldPointToScreenPoint(Globals.GetLimbPosition(zombie.transform, "Left_Foot"));
                        bz.Alive = !zombie.isDead;
                        bz.Visible = RaycastHelper.IsPointVisible(zombie, Globals.GetLimbPosition(zombie.transform, "Skull"));
                        if (bz.Visible)
                        {
                            bz.BoxColour = ColourHelper.GetColour("Zombie Visible Box Colour");
                            bz.FilledBoxColour = ColourHelper.GetColour("Zombie Visible Filled Box Colour");
                        }
                        else 
                        {
                            bz.BoxColour = ColourHelper.GetColour("Zombie Invisible Box Colour");
                            bz.FilledBoxColour = ColourHelper.GetColour("Zombie Invisible Filled Box Colour");
                        }
                            bz.Bounds = new Bounds(bz.Entity.transform.position + new Vector3(0, 1.1f, 0), bz.Entity.transform.localScale + new Vector3(0, .95f, 0));
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
                        bz.BoundPoints[0] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x + bz.Bounds.extents.x, Globals.GetLimbPosition(zombie.transform, "Skull").y + (bz.Bounds.extents.y / 2), bz.Bounds.center.z + bz.Bounds.extents.z));
                        bz.BoundPoints[1] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x + bz.Bounds.extents.x,  Globals.GetLimbPosition(zombie.transform, "Skull").y + (bz.Bounds.extents.y / 2), bz.Bounds.center.z - bz.Bounds.extents.z));
                        bz.BoundPoints[2] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x + bz.Bounds.extents.x, bz.Bounds.center.y - bz.Bounds.extents.y, bz.Bounds.center.z + bz.Bounds.extents.z));
                        bz.BoundPoints[3] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x + bz.Bounds.extents.x, bz.Bounds.center.y - bz.Bounds.extents.y, bz.Bounds.center.z - bz.Bounds.extents.z));
                        bz.BoundPoints[4] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x - bz.Bounds.extents.x, Globals.GetLimbPosition(zombie.transform, "Skull").y + (bz.Bounds.extents.y / 2), bz.Bounds.center.z + bz.Bounds.extents.z));
                        bz.BoundPoints[5] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x - bz.Bounds.extents.x, Globals.GetLimbPosition(zombie.transform, "Skull").y + (bz.Bounds.extents.y / 2), bz.Bounds.center.z - bz.Bounds.extents.z));
                        bz.BoundPoints[6] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x - bz.Bounds.extents.x, bz.Bounds.center.y - bz.Bounds.extents.y, bz.Bounds.center.z + bz.Bounds.extents.z));
                        bz.BoundPoints[7] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x - bz.Bounds.extents.x, bz.Bounds.center.y - bz.Bounds.extents.y, bz.Bounds.center.z - bz.Bounds.extents.z));

                        bz.BonePosition[0] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Right_Foot"));
                        bz.BonePosition[1] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Left_Foot"));
                        bz.BonePosition[2] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Right_Leg"));
                        bz.BonePosition[3] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Left_Leg"));
                        bz.BonePosition[4] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Right_Front"));
                        bz.BonePosition[5] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Left_Front"));
                        bz.BonePosition[6] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Right_Back"));
                        bz.BonePosition[7] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Left_Back"));
                        bz.BonePosition[8] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Right_Arm"));
                        bz.BonePosition[9] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Left_Arm"));
                        bz.BonePosition[10] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Right_Hand"));
                        bz.BonePosition[11] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Left_Hand"));
                        bz.BonePosition[12] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Spine"));
                        bz.BonePosition[13] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Skull"));
                    }
                       
                }
                catch { }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
