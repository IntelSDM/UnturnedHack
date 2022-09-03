using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
using System.Collections;
using Hag.Esp_Objects;
using Hag.Helpers;
using System.Reflection;
using System.Runtime.InteropServices;
namespace Hag.Misc
{
    class Weapon :MonoBehaviour
    {
        public static Dictionary<ushort, float[]> ValueLog = new Dictionary<ushort, float[]>();
        public static MethodInfo UpdateCrosshair = typeof(UseableGun).GetMethod("updateCrosshair", BindingFlags.Instance | BindingFlags.NonPublic);
        void Start()
        {
            StartCoroutine(Weapons());
            
        }
       
        IEnumerator Weapons()
        {
            for (; ; )
            {
                if ((Player.player?.equipment?.asset is ItemGunAsset))
                {
                    ItemGunAsset Weapon = (ItemGunAsset)Player.player?.equipment?.asset;
                    if (!ValueLog.ContainsKey(Weapon.id))
                    {

                        float[] Backups = new float[]
                         {
                                Weapon.recoilAim,
                                Weapon.recoilMax_x,
                                Weapon.recoilMax_y,
                                Weapon.recoilMin_x,
                                Weapon.recoilMin_y,
                                Weapon.spreadAim,
                                Weapon.spreadHip,
                                Weapon.spreadCrouch,
                                Weapon.spreadProne,
                                Weapon.spreadSprint
                         };
                        ValueLog.Add(Weapon.id, Backups);
                        Backups = null;
                    }
                    else
                    {
                        if (Weapon != null && ! Globals.Spied)
                        {
                            if (Globals.Config.Weapon.NoRecoil)
                            {
                                Weapon.recoilMin_x = 0;
                                Weapon.recoilMin_y = 0;
                                Weapon.recoilMax_x = Globals.Config.Weapon.RecoilxAmount;
                                Weapon.recoilMax_y = Globals.Config.Weapon.RecoilyAmount;

                            }
                            else
                            {
                                Weapon.recoilMin_x = ValueLog[Weapon.id][3];
                                Weapon.recoilMin_y = ValueLog[Weapon.id][4];
                                Weapon.recoilMax_x = ValueLog[Weapon.id][1];
                                Weapon.recoilMax_y = ValueLog[Weapon.id][2];

                            }
                            if (Globals.Config.Weapon.NoSpread)
                            {
                       //         Weapon.spreadAim = Globals.Config.Weapon.NoSpreadAim;
                       //         Weapon.spreadHip = Globals.Config.Weapon.NoSpreadHip;
                       //         Weapon.spreadCrouch = Globals.Config.Weapon.NoSpreadCrouch;
                       //        Weapon.spreadProne = Globals.Config.Weapon.NoSpreadProne;
                        //        Weapon.spreadSprint = Globals.Config.Weapon.NoSpreadSprint;

                            }
                            UpdateCrosshair.Invoke(Player.player.equipment.useable, null);
                        }
                        }

                }

                    yield return new WaitForSeconds(0.5f);
            }
        }
        }
    }
