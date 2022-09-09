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
            StartCoroutine(UpdatePlayers());
            StartCoroutine(UpdateVehicles());
            StartCoroutine(UpdateItems());
        }
        IEnumerator UpdateItems()
        {
            for (; ; )
            {
                try
                {
                    foreach (BaseItem bi in Globals.ItemList)
                    {
                        if (bi == null || bi.Entity == null)
                            continue;
                        //    if (vh.Entity.isDead)
                        //      continue;

                        bi.W2S = Globals.WorldPointToScreenPoint(bi.Entity.transform.position);
                        bi.Name = bi.Entity.asset.itemName;
                        bi.Distance = (int)Vector3.Distance(bi.Entity.transform.position, Globals.MainCamera.transform.position);
                       

                        bi.Colour = ColourHelper.GetColour("Items Colour");
                     
                    }
                }
                catch { }
                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator UpdateVehicles()
        {
            for (; ; )
            {
                try
                {
                    foreach (BaseVehicle vh in Globals.VehicleList)
                    {
                        if (vh == null || vh.Entity == null)
                            continue;
                    //    if (vh.Entity.isDead)
                      //      continue;
                        vh.Locked = vh.Entity.isLocked;
                        vh.W2S = Globals.WorldPointToScreenPoint(vh.Entity.transform.position);
                        vh.Name = vh.Entity.asset.vehicleName;
                        vh.Distance = (int)Vector3.Distance(vh.Entity.transform.position, Globals.MainCamera.transform.position);
                        vh.IsDriven = vh.Entity.isDriven;
                        if ((vh.Entity.lockedGroup == Player.player.quests.groupID || vh.Entity.lockedOwner.m_SteamID == Player.player.channel.owner.playerID.steamID.m_SteamID) && vh.Locked && vh.Entity.lockedGroup != null && vh.Entity.lockedOwner != null)
                            vh.OwnedByYou = true;
                        else
                            vh.OwnedByYou = false;
                        vh.Colour = ColourHelper.GetColour("Vehicles Colour");
                        if(!vh.Locked)
                            vh.Colour = ColourHelper.GetColour("Vehicles Unlocked Colour");
                        if (vh.OwnedByYou && vh.Locked)
                            vh.Colour = ColourHelper.GetColour("Vehicles Owned By You Colour");
                    }
                }
                catch { }
                yield return new WaitForEndOfFrame();
                }
        }
        IEnumerator UpdatePlayers()
        {
            for (; ; )
            {
                try
                {
                    foreach (BasePlayer bp in Globals.PlayerList)
                    {
                        Player player = bp.Entity;
                        if (player == null)
                            continue;
                        if (Globals.Config.Friends.FriendsList.ContainsKey(bp.SteamPlayer.playerID.steamID.m_SteamID))
                            bp.Friendly = true;
                        if (Globals.Config.Friends.SteamFriends)
                        {
                            for (int i = 0; i < Steamworks.SteamFriends.GetFriendCount(Steamworks.EFriendFlags.k_EFriendFlagImmediate); i++)
                                if (bp.SteamPlayer.playerID.steamID.m_SteamID == (Steamworks.SteamFriends.GetFriendByIndex(i, Steamworks.EFriendFlags.k_EFriendFlagImmediate).m_SteamID))
                                    bp.Friendly = true;
                        }
                        if (bp.Entity.quests.isMemberOfSameGroupAs(Player.player))
                            bp.Friendly = true;

                        Vector3 skull = Globals.GetLimbPosition(player.transform, "Skull");
                        bp.Distance = (int)Vector3.Distance(player.transform.position, Globals.LocalPlayer.transform.position);
                        bp.Name = bp.SteamPlayer.playerID.playerName;
                        bp.Colour = ColourHelper.GetColour("Player Text Colour");
                        if (bp.Friendly)
                            bp.Colour = ColourHelper.GetColour("Friendly-Player Text Colour");
                        bp.W2S = WorldPointToScreenPoint(player.transform.position);
                        bp.HeadW2S = WorldPointToScreenPoint(skull);
                        bp.Alive = !player.life.isDead;
                        bp.Weapon = player.equipment.asset != null ? player.equipment?.asset?.itemName : "Empty";
                        if (bp.Distance > Globals.Config.Player.MaxDistance)
                            continue;
                        bp.Visible = RaycastHelper.IsPointVisible(player,skull);
                        if (!bp.Friendly)
                        {
                            if (bp.Visible)
                            {
                                bp.BoxColour = ColourHelper.GetColour("Player Visible Box Colour");
                                bp.FilledBoxColour = ColourHelper.GetColour("Player Visible Filled Box Colour");
                            }
                            else
                            {
                                bp.BoxColour = ColourHelper.GetColour("Player Invisible Box Colour");
                                bp.FilledBoxColour = ColourHelper.GetColour("Player Invisible Filled Box Colour");
                            }
                        }
                        else
                        {
                            if (bp.Visible)
                            {
                                bp.BoxColour = ColourHelper.GetColour("Friendly-Player Visible Box Colour");
                                bp.FilledBoxColour = ColourHelper.GetColour("Friendly-Player Visible Filled Box Colour");
                            }
                            else
                            {
                                bp.BoxColour = ColourHelper.GetColour("Friendly-Player Invisible Box Colour");
                                bp.FilledBoxColour = ColourHelper.GetColour("Friendly-Player Invisible Filled Box Colour");
                            }
                        }
                        Vector3 spine = Globals.GetLimbPosition(player.transform, "Spine");
                        Vector3 lefthand = Globals.GetLimbPosition(player.transform, "Left_Hand");
                        Vector3 righthand = Globals.GetLimbPosition(player.transform, "Right_Hand");
                        Vector3 leftarm = Globals.GetLimbPosition(player.transform, "Left_Arm");
                        Vector3 rightarm = Globals.GetLimbPosition(player.transform, "Right_Arm");
                        Vector3 leftleg = Globals.GetLimbPosition(player.transform, "Left_Leg");
                        Vector3 rightleg = Globals.GetLimbPosition(player.transform, "Right_Leg");
                        Vector3 leftfoot = Globals.GetLimbPosition(player.transform, "Left_Foot");
                        Vector3 rightfoot = Globals.GetLimbPosition(player.transform, "Right_Foot");
                        bp.Bounds = new Bounds(bp.Entity.transform.position + new Vector3(0, 1.1f, 0), bp.Entity.transform.localScale + new Vector3(0, .95f, 0));
                        bp.BoundPoints[0] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x + bp.Bounds.extents.x, skull.y + (bp.Bounds.extents.y / 2), bp.Bounds.center.z + bp.Bounds.extents.z));
                        bp.BoundPoints[1] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x + bp.Bounds.extents.x, skull.y + (bp.Bounds.extents.y / 2), bp.Bounds.center.z - bp.Bounds.extents.z));
                        bp.BoundPoints[2] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x + bp.Bounds.extents.x, bp.Bounds.center.y - bp.Bounds.extents.y, bp.Bounds.center.z + bp.Bounds.extents.z));
                        bp.BoundPoints[3] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x + bp.Bounds.extents.x, bp.Bounds.center.y - bp.Bounds.extents.y, bp.Bounds.center.z - bp.Bounds.extents.z));
                        bp.BoundPoints[4] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x - bp.Bounds.extents.x, skull.y + (bp.Bounds.extents.y / 2), bp.Bounds.center.z + bp.Bounds.extents.z));
                        bp.BoundPoints[5] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x - bp.Bounds.extents.x, skull.y + (bp.Bounds.extents.y / 2), bp.Bounds.center.z - bp.Bounds.extents.z));
                        bp.BoundPoints[6] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x - bp.Bounds.extents.x, bp.Bounds.center.y - bp.Bounds.extents.y, bp.Bounds.center.z + bp.Bounds.extents.z));
                        bp.BoundPoints[7] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x - bp.Bounds.extents.x, bp.Bounds.center.y - bp.Bounds.extents.y, bp.Bounds.center.z - bp.Bounds.extents.z));
                     //   bp.Entity.animator.
                        bp.BonePosition[0] = Globals.WorldPointToScreenPoint(rightfoot);
                        bp.BonePosition[1] = Globals.WorldPointToScreenPoint(leftfoot);
                        bp.BonePosition[2] = Globals.WorldPointToScreenPoint(rightleg);
                        bp.BonePosition[3] = Globals.WorldPointToScreenPoint(leftleg);
                        bp.BonePosition[4] = Globals.WorldPointToScreenPoint(rightarm);
                        bp.BonePosition[5] = Globals.WorldPointToScreenPoint(leftarm);
                        bp.BonePosition[6] = Globals.WorldPointToScreenPoint(righthand);
                        bp.BonePosition[7] = Globals.WorldPointToScreenPoint(lefthand);
                        bp.BonePosition[8] = Globals.WorldPointToScreenPoint(spine);
                        bp.BonePosition[9] = Globals.WorldPointToScreenPoint(skull);
                        bp.BonePosition[10] = new Vector3(bp.BonePosition[8].x, bp.BonePosition[5].y, bp.BonePosition[5].z);

                        bp.WorldBonePosition[0] = rightfoot; 
                        bp.WorldBonePosition[1] = leftfoot;
                        bp.WorldBonePosition[2] = rightleg;
                        bp.WorldBonePosition[3] = leftleg;
                        bp.WorldBonePosition[4] = rightarm;
                        bp.WorldBonePosition[5] = leftarm;
                        bp.WorldBonePosition[6] = righthand;
                        bp.WorldBonePosition[7] = lefthand;
                        bp.WorldBonePosition[8] = spine;
                        bp.WorldBonePosition[9] = skull;
                        bp.WorldBonePosition[10] = new Vector3(bp.WorldBonePosition[8].x, bp.WorldBonePosition[5].y, bp.WorldBonePosition[8].z);
                      
                            if (!bp.Friendly)
                            {
                                for (int i = 0; i < bp.BonePosition.Count(); i++)
                                {
                                    if (RaycastHelper.IsPointVisible(bp.Entity, bp.WorldBonePosition[i]))
                                        bp.BoneColour[i] = ColourHelper.GetColour("Player Bone Visible Colour");
                                    else
                                        bp.BoneColour[i] = ColourHelper.GetColour("Player Bone Invisible Colour");
                                }
                            }
                            else
                            {
                                for (int i = 0; i < bp.BonePosition.Count(); i++)
                                {
                                    if (RaycastHelper.IsPointVisible(bp.Entity, bp.WorldBonePosition[i]))
                                        bp.BoneColour[i] = ColourHelper.GetColour("Friendly-Player Bone Visible Colour");
                                    else
                                        bp.BoneColour[i] = ColourHelper.GetColour("Friendly-Player Bone Invisible Colour");
                                }
                            }
                        
                    }
                    }
                catch (Exception ex) {  }
                yield return new WaitForEndOfFrame();
            }
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
                        Vector3 skull = Globals.GetLimbPosition(zombie.transform, "Skull");
                        bz.HeadW2S = WorldPointToScreenPoint(skull);
                      //  bz.FootW2S = WorldPointToScreenPoint(Globals.GetLimbPosition(zombie.transform, "Left_Foot"));
                        bz.Alive = !zombie.isDead;
                        bz.Visible = RaycastHelper.IsPointVisible(zombie, skull);
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
                        if (bz.Distance > Globals.Config.Zombie.MaxDistance)
                            continue;
                        Vector3 spine = Globals.GetLimbPosition(zombie.transform, "Spine");
                        Vector3 lefthand = Globals.GetLimbPosition(zombie.transform, "Left_Hand");
                        Vector3 righthand = Globals.GetLimbPosition(zombie.transform, "Right_Hand");
                        Vector3 leftarm = Globals.GetLimbPosition(zombie.transform, "Left_Arm");
                        Vector3 rightarm = Globals.GetLimbPosition(zombie.transform, "Right_Arm");
                        Vector3 leftleg = Globals.GetLimbPosition(zombie.transform, "Left_Leg");
                        Vector3 rightleg = Globals.GetLimbPosition(zombie.transform, "Right_Leg");
                        Vector3 leftfoot = Globals.GetLimbPosition(zombie.transform, "Left_Foot");
                        Vector3 rightfoot = Globals.GetLimbPosition(zombie.transform, "Right_Foot");

                        bz.BoundPoints[0] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x + bz.Bounds.extents.x, skull.y + (bz.Bounds.extents.y / 2), bz.Bounds.center.z + bz.Bounds.extents.z));
                        bz.BoundPoints[1] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x + bz.Bounds.extents.x,  skull.y + (bz.Bounds.extents.y / 2), bz.Bounds.center.z - bz.Bounds.extents.z));
                        bz.BoundPoints[2] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x + bz.Bounds.extents.x, bz.Bounds.center.y - bz.Bounds.extents.y, bz.Bounds.center.z + bz.Bounds.extents.z));
                        bz.BoundPoints[3] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x + bz.Bounds.extents.x, bz.Bounds.center.y - bz.Bounds.extents.y, bz.Bounds.center.z - bz.Bounds.extents.z));
                        bz.BoundPoints[4] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x - bz.Bounds.extents.x, skull.y + (bz.Bounds.extents.y / 2), bz.Bounds.center.z + bz.Bounds.extents.z));
                        bz.BoundPoints[5] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x - bz.Bounds.extents.x, skull.y + (bz.Bounds.extents.y / 2), bz.Bounds.center.z - bz.Bounds.extents.z));
                        bz.BoundPoints[6] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x - bz.Bounds.extents.x, bz.Bounds.center.y - bz.Bounds.extents.y, bz.Bounds.center.z + bz.Bounds.extents.z));
                        bz.BoundPoints[7] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bz.Bounds.center.x - bz.Bounds.extents.x, bz.Bounds.center.y - bz.Bounds.extents.y, bz.Bounds.center.z - bz.Bounds.extents.z));

                        bz.BonePosition[0] = Globals.WorldPointToScreenPoint(rightfoot);
                        bz.BonePosition[1] = Globals.WorldPointToScreenPoint(leftfoot);
                        bz.BonePosition[2] = Globals.WorldPointToScreenPoint(rightleg);
                        bz.BonePosition[3] = Globals.WorldPointToScreenPoint(leftleg);
                        bz.BonePosition[4] = Globals.WorldPointToScreenPoint(rightarm);
                        bz.BonePosition[5] = Globals.WorldPointToScreenPoint(leftarm);
                        bz.BonePosition[6] = Globals.WorldPointToScreenPoint(righthand);
                        bz.BonePosition[7] = Globals.WorldPointToScreenPoint(lefthand);
                        bz.BonePosition[8] = Globals.WorldPointToScreenPoint(spine);
                        bz.BonePosition[9] = Globals.WorldPointToScreenPoint(skull);
                        bz.BonePosition[10] = new Vector3(bz.BonePosition[8].x, bz.BonePosition[5].y, bz.BonePosition[5].z);

                        bz.WorldBonePosition[0] = rightfoot;
                        bz.WorldBonePosition[1] = leftfoot;
                        bz.WorldBonePosition[2] = rightleg;
                        bz.WorldBonePosition[3] = leftleg;
                        bz.WorldBonePosition[4] = rightarm;
                        bz.WorldBonePosition[5] = leftarm;
                        bz.WorldBonePosition[6] = righthand;
                        bz.WorldBonePosition[7] = lefthand;
                        bz.WorldBonePosition[8] = spine;
                        bz.WorldBonePosition[9] = skull;
                        bz.WorldBonePosition[10] = new Vector3(bz.WorldBonePosition[8].x, bz.WorldBonePosition[5].y, bz.WorldBonePosition[8].z);
                        Vector2 vector = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
                        int num = (int)Vector2.Distance(bz.HeadW2S, vector); // fov check
                        bz.test = num;
                       
                            for (int i = 0; i < bz.BonePosition.Count(); i++)
                            {
                                if (RaycastHelper.IsPointVisible(bz.Entity, bz.WorldBonePosition[i]))
                                    bz.BoneColour[i] = ColourHelper.GetColour("Zombie Bone Visible Colour");
                                else
                                    bz.BoneColour[i] = ColourHelper.GetColour("Zombie Bone Invisible Colour");
                            }
                        
                    }
                       
                }
                catch { }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
