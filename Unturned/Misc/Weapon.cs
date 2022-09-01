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
                        Backups[6] = Weapon.spreadHip;
                        ValueLog.Add(Weapon.id, Backups);
                        Backups = null;
                    }
                    else
                    {
                        if (Weapon != null)
                        {
                            if(Globals.Config.Weapon.NoRecoil)
                            {
                                Weapon.recoilMin_x = 0;
                                Weapon.recoilMin_y = 0;
                                Weapon.recoilMax_x = Globals.Config.Weapon.RecoilxAmount;
                                Weapon.recoilMax_y = Globals.Config.Weapon.RecoilyAmount;

                            }
                        }
                        }

                }

                    yield return new WaitForSeconds(0.5f);
            }
        }
        }
    }
