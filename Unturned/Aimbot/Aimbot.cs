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

namespace Hag.Aimbot
{
    class Aimbot : MonoBehaviour
    {
        public static Zombie TargetLegitZombie;
        void Start()
        {
            StartCoroutine(ZombieLegitbot());
        }
        public static List<BaseZombie> SortClosestToCrosshair(List<BaseZombie> p)
        {
            return (from tempPlayer in p
                    orderby Vector2.Distance(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), Camera.main.WorldToScreenPoint(tempPlayer.Entity.transform.position)), tempPlayer.Distance
                   
                    select tempPlayer).ToList<BaseZombie>();
        }
        public static List<BasePlayer> SortClosestToCrosshair(List<BasePlayer> p)
        {
            return (from tempPlayer in p
                    orderby Vector2.Distance(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), Camera.main.WorldToScreenPoint(tempPlayer.Entity.transform.position)), tempPlayer.Distance
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
            return player.WorldBonePosition[9];
            switch (bone)
            {
                case 0:
                    if (Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[9]) && vischecks)
                        return player.WorldBonePosition[9];
                    else if (!vischecks)
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
                    if (vischecks && Helpers.RaycastHelper.IsPointVisible(player.Entity, player.BonePosition[4]))
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
                        //      if (Vector3.Distance(Globals.MainCamera.transform.position, basezombie.Entity.transform.position) > Globals.Config.ZombieAimbot.LegitMaxDistance)
                        //      continue;
                        TargetLegitZombie = basezombie.Entity;
                        return worldpos;
                    }
                }
            }
            catch { }
            return Vector3.zero;
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


                            Player.player.transform.LookAt(TargetZombie);
                            Player.player.transform.eulerAngles = new Vector3(0f, Player.player.transform.rotation.eulerAngles.y, 0f);
                            Camera.main.transform.LookAt(TargetZombie);
                            float x = Camera.main.transform.localRotation.eulerAngles.x;
                            if (x <= 90f && x <= 270f)
                            {
                                x = Camera.main.transform.localRotation.eulerAngles.x + 90f;
                            }
                            else if (x >= 270f && x <= 360f)
                            {
                                x = Camera.main.transform.localRotation.eulerAngles.x - 270f;
                            }
                            Player.player.look.GetType().GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.player.look, x);
                            Player.player.look.GetType().GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.player.look, Player.player.transform.rotation.eulerAngles.y);

                        }
                        catch { }
                    }
                    }
                    yield return new WaitForEndOfFrame();
            }
        }
    }
}
