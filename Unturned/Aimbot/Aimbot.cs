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

namespace Hag.Aimbot
{
    class Aimbot : MonoBehaviour
    {
        public static BaseZombie TargetLegitZombie;
        void Start()
        {
            StartCoroutine(ZombieLegitbot());
        }
        public static List<BaseZombie> SortClosestToCrosshair(List<BaseZombie> p)
        {
            return (from tempPlayer in p
                    orderby Vector2.Distance(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)),tempPlayer.HeadW2S), tempPlayer.Distance
                   
                    select tempPlayer).ToList<BaseZombie>();
        }
        public static List<BasePlayer> SortClosestToCrosshair(List<BasePlayer> p)
        {
            return (from tempPlayer in p
                    orderby Vector2.Distance(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)),tempPlayer.HeadW2S), tempPlayer.Distance
                    select tempPlayer).ToList<BasePlayer>();
        }
        public static Vector3 GetAimbone(int bone,bool vischecks,BasePlayer player)
        {
            /*
                  bz.WorldBonePosition[0] = (Globals.GetLimbPosition(bz.Entity.transform, "Right_Foot"));
                        bz.WorldBonePosition[1] = (Globals.GetLimbPosition(bz.Entity.transform, "Left_Foot"));
                        bz.WorldBonePosition[2] = (Globals.GetLimbPosition(bz.Entity.transform, "Right_Leg"));
                        bz.WorldBonePosition[3] = (Globals.GetLimbPosition(bz.Entity.transform, "Left_Leg"));
                        bz.WorldBonePosition[4] = (Globals.GetLimbPosition(bz.Entity.transform, "Right_Arm"));
                        bz.WorldBonePosition[5] = (Globals.GetLimbPosition(bz.Entity.transform, "Left_Arm"));
                        bz.WorldBonePosition[6] = (Globals.GetLimbPosition(bz.Entity.transform, "Right_Hand"));
                        bz.WorldBonePosition[7] = (Globals.GetLimbPosition(bz.Entity.transform, "Left_Hand"));
                        bz.WorldBonePosition[8] = (Globals.GetLimbPosition(bz.Entity.transform, "Spine"));
                        bz.WorldBonePosition[9] = (Globals.GetLimbPosition(bz.Entity.transform, "Skull"));
                        bz.WorldBonePosition[10] = new Vector3(bz.WorldBonePosition[8].x, bz.WorldBonePosition[5].y, bz.WorldBonePosition[8].z);
            */
        
                switch (bone)
                {
                    case 0:
                    if (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[9]) && vischecks)
                        return player.WorldBonePosition[9];
                    else if(!vischecks)
                        return player.WorldBonePosition[9];
                    else
                        return Vector3.zero;
                    break;
                case 1:
                    if (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[10]) && vischecks)
                        return player.WorldBonePosition[10];
                    else if (!vischecks)
                        return player.WorldBonePosition[10];
                    else
                        return Vector3.zero;
                    break;
                case 2:
                    if (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[8]) && vischecks)
                        return player.WorldBonePosition[8];
                    else if (!vischecks)
                        return player.WorldBonePosition[8];
                    else
                        return Vector3.zero;
                    break;
                case 3:
                    if (vischecks && (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[4]) && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[5])))
                    {
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[4].x, player.WorldBonePosition[4].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.WorldBonePosition[5].x, player.WorldBonePosition[5].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[5];
                        }
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[4].x, player.WorldBonePosition[4].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.WorldBonePosition[5].x, player.WorldBonePosition[5].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[4];
                        }
                    }
                    if(vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[4]))
                         return player.WorldBonePosition[4];
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[5]))
                         return player.WorldBonePosition[5];
                    else if (!vischecks)
                    {
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[4].x, player.WorldBonePosition[4].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.WorldBonePosition[5].x, player.WorldBonePosition[5].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[5];
                        }
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[4].x, player.WorldBonePosition[4].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.WorldBonePosition[5].x, player.WorldBonePosition[5].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[4];
                        }
                    }
                    break;
                case 4:
                    if (vischecks && (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[6]) && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[7])))
                    {
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[6].x, player.WorldBonePosition[6].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.WorldBonePosition[7].x, player.WorldBonePosition[7].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[7];
                        }
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[6].x, player.WorldBonePosition[6].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.WorldBonePosition[7].x, player.WorldBonePosition[7].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[6];
                        }
                    }
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[6]))
                        return player.WorldBonePosition[6];
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[7]))
                        return player.WorldBonePosition[7];
                    else if (!vischecks)
                    {
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[6].x, player.WorldBonePosition[6].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.WorldBonePosition[7].x, player.WorldBonePosition[7].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[7];
                        }
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[6].x, player.WorldBonePosition[6].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.WorldBonePosition[7].x, player.WorldBonePosition[7].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[6];
                        }
                    }
                    break;
                case 5:
                    if (vischecks && (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[2]) && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[3])))
                    {
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[2].x, player.WorldBonePosition[2].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.WorldBonePosition[3].x, player.WorldBonePosition[3].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[3];
                        }
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[2].x, player.WorldBonePosition[2].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.WorldBonePosition[3].x, player.WorldBonePosition[3].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[2];
                        }
                    }
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[2]))
                        return player.WorldBonePosition[2];
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[3]))
                        return player.WorldBonePosition[3];
                    else if (!vischecks)
                    {
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[2].x, player.WorldBonePosition[2].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.WorldBonePosition[3].x, player.WorldBonePosition[3].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[3];
                        }
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[2].x, player.WorldBonePosition[2].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.WorldBonePosition[3].x, player.WorldBonePosition[3].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[2];
                        }
                    }
                    break;
                case 6:
                    if (vischecks && (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[0]) && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[1])))
                    {
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[0].x, player.WorldBonePosition[0].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.WorldBonePosition[1].x, player.WorldBonePosition[1].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[1];
                        }
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[0].x, player.WorldBonePosition[0].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.WorldBonePosition[1].x, player.WorldBonePosition[1].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[0];
                        }
                    }
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[0]))
                        return player.WorldBonePosition[0];
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[1]))
                        return player.WorldBonePosition[1];
                    else if (!vischecks)
                    {
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[0].x, player.WorldBonePosition[0].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.WorldBonePosition[1].x, player.WorldBonePosition[1].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[1];
                        }
                        if (Vector2.Distance(new Vector2(player.WorldBonePosition[0].x, player.WorldBonePosition[0].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.WorldBonePosition[1].x, player.WorldBonePosition[1].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[0];
                        }
                    }
                    break;
            }
            
            return Vector3.zero;
        }
        public static Vector3 GetAimbone(int bone, bool vischecks, BaseZombie player)
        {
            /*
                  bz.WorldBonePosition[0] = (Globals.GetLimbPosition(bz.Entity.transform, "Right_Foot"));
                        bz.WorldBonePosition[1] = (Globals.GetLimbPosition(bz.Entity.transform, "Left_Foot"));
                        bz.WorldBonePosition[2] = (Globals.GetLimbPosition(bz.Entity.transform, "Right_Leg"));
                        bz.WorldBonePosition[3] = (Globals.GetLimbPosition(bz.Entity.transform, "Left_Leg"));
                        bz.WorldBonePosition[4] = (Globals.GetLimbPosition(bz.Entity.transform, "Right_Arm"));
                        bz.WorldBonePosition[5] = (Globals.GetLimbPosition(bz.Entity.transform, "Left_Arm"));
                        bz.WorldBonePosition[6] = (Globals.GetLimbPosition(bz.Entity.transform, "Right_Hand"));
                        bz.WorldBonePosition[7] = (Globals.GetLimbPosition(bz.Entity.transform, "Left_Hand"));
                        bz.WorldBonePosition[8] = (Globals.GetLimbPosition(bz.Entity.transform, "Spine"));
                        bz.WorldBonePosition[9] = (Globals.GetLimbPosition(bz.Entity.transform, "Skull"));
                        bz.WorldBonePosition[10] = new Vector3(bz.WorldBonePosition[8].x, bz.WorldBonePosition[5].y, bz.WorldBonePosition[8].z);
            */
         //   return player.WorldBonePosition[9];
            switch (bone)
            {
                case 0:
                    if (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[9]) && vischecks)
                        return player.WorldBonePosition[9];
                    else if (!vischecks)
                        return player.WorldBonePosition[9];
                    else
                        return Vector3.zero;
                    break;
                case 1:
                    if (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[10]) && vischecks)
                        return player.WorldBonePosition[10];
                    else if (!vischecks)
                        return player.WorldBonePosition[10];
                    else
                        return Vector3.zero;
                    break;
                case 2:
                    if (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[8]) && vischecks)
                        return player.WorldBonePosition[8];
                    else if (!vischecks)
                        return player.WorldBonePosition[8];
                    else
                        return Vector3.zero;
                    break;
                case 3:
                    if (vischecks && (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[4]) && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[5])))
                    {
                        if (Vector2.Distance(new Vector2(player.BonePosition[4].x, player.BonePosition[4].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.BonePosition[5].x, player.BonePosition[5].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[5];
                        }
                        if (Vector2.Distance(new Vector2(player.BonePosition[4].x, player.BonePosition[4].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.BonePosition[5].x, player.BonePosition[5].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[4];
                        }
                    }
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[4]))
                        return player.WorldBonePosition[4];
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[5]))
                        return player.WorldBonePosition[5];
                    else if (!vischecks)
                    {
                        if (Vector2.Distance(new Vector2(player.BonePosition[4].x, player.BonePosition[4].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.BonePosition[5].x, player.BonePosition[5].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[5];
                        }
                        if (Vector2.Distance(new Vector2(player.BonePosition[4].x, player.BonePosition[4].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.BonePosition[5].x, player.BonePosition[5].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[4];
                        }
                    }
                    break;
                case 4:
                    if (vischecks && (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[6]) && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[7])))
                    {
                        if (Vector2.Distance(new Vector2(player.BonePosition[6].x, player.BonePosition[6].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.BonePosition[7].x, player.BonePosition[7].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[7];
                        }
                        if (Vector2.Distance(new Vector2(player.BonePosition[6].x, player.BonePosition[6].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.BonePosition[7].x, player.BonePosition[7].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[6];
                        }
                    }
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[6]))
                        return player.WorldBonePosition[6];
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[7]))
                        return player.WorldBonePosition[7];
                    else if (!vischecks)
                    {
                        if (Vector2.Distance(new Vector2(player.BonePosition[6].x, player.BonePosition[6].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.BonePosition[7].x, player.BonePosition[7].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[7];
                        }
                        if (Vector2.Distance(new Vector2(player.BonePosition[6].x, player.BonePosition[6].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.BonePosition[7].x, player.BonePosition[7].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[6];
                        }
                    }
                    break;
                case 5:
                    if (vischecks && (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[2]) && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[3])))
                    {
                        if (Vector2.Distance(new Vector2(player.BonePosition[2].x, player.BonePosition[2].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.BonePosition[3].x, player.BonePosition[3].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[3];
                        }
                        if (Vector2.Distance(new Vector2(player.BonePosition[2].x, player.BonePosition[2].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.BonePosition[3].x, player.BonePosition[3].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[2];
                        }
                    }
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[2]))
                        return player.WorldBonePosition[2];
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[3]))
                        return player.WorldBonePosition[3];
                    else if (!vischecks)
                    {
                        if (Vector2.Distance(new Vector2(player.BonePosition[2].x, player.BonePosition[2].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.BonePosition[3].x, player.BonePosition[3].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[3];
                        }
                        if (Vector2.Distance(new Vector2(player.BonePosition[2].x, player.BonePosition[2].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.BonePosition[3].x, player.BonePosition[3].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[2];
                        }
                    }
                    break;
                case 6:
                    if (vischecks && (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[0]) && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[1])))
                    {
                        if (Vector2.Distance(new Vector2(player.BonePosition[0].x, player.BonePosition[0].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.BonePosition[1].x, player.BonePosition[1].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[1];
                        }
                        if (Vector2.Distance(new Vector2(player.BonePosition[0].x, player.BonePosition[0].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.BonePosition[1].x, player.BonePosition[1].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[0];
                        }
                    }
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[0]))
                        return player.WorldBonePosition[0];
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.WorldBonePosition[1]))
                        return player.WorldBonePosition[1];
                    else if (!vischecks)
                    {
                        if (Vector2.Distance(new Vector2(player.BonePosition[0].x, player.BonePosition[0].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) > Vector2.Distance(new Vector2(player.BonePosition[1].x, player.BonePosition[1].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[1];
                        }
                        if (Vector2.Distance(new Vector2(player.BonePosition[0].x, player.BonePosition[0].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))) < Vector2.Distance(new Vector2(player.BonePosition[1].x, player.BonePosition[1].y), new Vector2((Screen.width / 2), (float)(Screen.height / 2))))
                        {
                            return player.WorldBonePosition[0];
                        }
                    }
                    break;
            }

            return Vector3.zero;
        }
    
        Vector3 GetTargetLegitZombie()
        {
            // write to the targetzombie
            // return the ideal aim position
            Vector3 worldpos = Vector3.zero;
            try
            {
              
                foreach (BaseZombie basezombie in SortClosestToCrosshair(Globals.ZombieList))
                {
                    if (!basezombie.Alive || basezombie.Entity == null)
                        continue;
                    if ((Globals.Config.Zombie.MaxDistance < basezombie.Distance))
                        continue;
                    if (Globals.Config.ZombieAimbot.LegitAimbotEnabled)
                    {
                        worldpos = GetAimbone(Globals.Config.ZombieAimbot.LegitAimbotBone, Globals.Config.ZombieAimbot.LegitVisiblityChecks, basezombie);
                        if (worldpos == Vector3.zero)
                            continue;
                        if (!Globals.IsScreenPointVisible(Globals.WorldPointToScreenPoint(worldpos)))
                            continue;
                        if ((Globals.Config.ZombieAimbot.LegitMaxDistance < basezombie.Distance))
                            continue;
                        Vector2 vector = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
                        int num = (int)Vector2.Distance(Globals.WorldPointToScreenPoint(worldpos), vector); // fov check
                        if (num > Globals.Config.Aimbot.Fov)
                            continue;
                        TargetLegitZombie = basezombie;
                        return worldpos;
                    }
                }
            }
            catch { }
            return Vector3.zero;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        public float DropCalc(Vector3 point)
        {
            // thanks bocheats
            float treturn = 0f;

            if (Vector3.Distance(point, Player.player.look.aim.position) < 35f)
                return 0f;

            ItemGunAsset firearm = (ItemGunAsset)Player.player.equipment.asset;

            Quaternion quaternion = Quaternion.LookRotation(point - Player.player.look.transform.position, Player.player.look.transform.up);
            Vector3 targetForward = quaternion * Vector3.forward;

            BulletInfo bulletInfo = new BulletInfo();
            bulletInfo.pos = Player.player.look.transform.position;
            bulletInfo.dir = targetForward.normalized;

            float num = firearm.ballisticDrop;
            bulletInfo.barrelAsset = Player.player.equipment.thirdModel.gameObject.GetComponent<Attachments>().barrelAsset;
            if (bulletInfo.barrelAsset != null)
                num *= bulletInfo.barrelAsset.ballisticDrop;

            int ticker = 0;
            while (++ticker < firearm.ballisticSteps)
            {
                bulletInfo.pos += bulletInfo.dir * firearm.ballisticTravel;
                bulletInfo.dir.y -= num;
                bulletInfo.dir.Normalize();

                if (Vector3.Distance(
                    new Vector3(point.x, 0f, point.z),
                    new Vector3(bulletInfo.pos.x, 0f, bulletInfo.pos.z))
                    < firearm.ballisticTravel
                    )
                {
                    treturn = bulletInfo.pos.y - point.y;
                    break;
                }
            }

            if (treturn < 0)
                treturn -= treturn * 2.1f;
            else
                treturn = 0f;

            return treturn;
        }
        IEnumerator ZombieLegitbot()
        {
            for (; ; )
            {
                if (Input.GetKey(Globals.Config.ZombieAimbot.LegitAimbotKey))
                {
                  
                    Vector3 TargetZombie = GetTargetLegitZombie();
                    if (TargetZombie != Vector3.zero)
                    {
                        try
                        {
                         
                            float drop = 0;
                            try
                            {
                                if (Globals.Config.ZombieAimbot.BulletDropPrediction && (Provider.mode != EGameMode.EASY  || Provider.modeConfigData.Gameplay.Ballistics))
                                {
                                    drop = DropCalc(TargetZombie);
                                }
                            }
                            catch { }
                                TargetZombie.y += drop;
                            float ScreenCenterX = (Screen.width / 2);
                            float ScreenCenterY = (Screen.height / 2);
                            float TargetX = 0;
                            float TargetY = 0;
                            float x = Globals.WorldPointToScreenPoint(TargetZombie).x; 
                            float y = Globals.WorldPointToScreenPoint(TargetZombie).y;
                            float AimSpeed = ((100 - Globals.Config.ZombieAimbot.Smoothing) + 1) * 1000;
                            if (x != 0)
                            {
                                if (x > ScreenCenterX)
                                {
                                    TargetX = -(ScreenCenterX - x);
                                    if (Globals.Config.ZombieAimbot.Smooth)
                                        TargetX /= AimSpeed;
                                    if (TargetX + ScreenCenterX > ScreenCenterX * 2) TargetX = 0;
                                }
                                if (x < ScreenCenterX)
                                {
                                    TargetX = x - ScreenCenterX;
                                    if (Globals.Config.ZombieAimbot.Smooth)
                                        TargetX /= AimSpeed;
                                    if (TargetX + ScreenCenterX < 0) TargetX = 0;
                                }
                            }
                            if (y != 0)
                            {
                                if (y > ScreenCenterY)
                                {
                                    TargetY = -(ScreenCenterY - y);
                                    if (Globals.Config.ZombieAimbot.Smooth)
                                        TargetY /= AimSpeed;
                                    if (TargetY + ScreenCenterY > ScreenCenterY * 2) TargetY = 0;
                                }
                                if (y < ScreenCenterY)
                                {
                                    TargetY = y - ScreenCenterY;
                                    if (Globals.Config.ZombieAimbot.Smooth)
                                        TargetY /= AimSpeed;
                                    if (TargetY + ScreenCenterY < 0) TargetY = 0;
                                }
                            }

                            //  mouse_event(0x0001, (uint)(TargetX), (uint)(TargetY), 0, UIntPtr.Zero);
                            if (Globals.Config.ZombieAimbot.Smooth)
                            {
                                TargetX /= 10;
                                TargetY /= 10;
                                if (Math.Abs(TargetX) < 1)
                                {
                                    if (TargetX > 0)
                                        TargetX = 1;
                                    if (TargetX < 0)
                                        TargetX = -1;
                                }
                                if (Math.Abs(TargetY) < 1)
                                {
                                    if (TargetY > 0)
                                        TargetY = 1;
                                    if (TargetY < 0)
                                        TargetY = -1;
                                }
                                mouse_event(0x0001, (uint)TargetX, (uint)TargetY, 0, UIntPtr.Zero);
                            }
                            else
                            {
                                mouse_event(0x0001, (uint)(TargetX), (uint)(TargetY), 0, UIntPtr.Zero);
                            }
                            }
                        catch { }


                    }
                    }
                    yield return new WaitForEndOfFrame();
            }
        }
    }
}
