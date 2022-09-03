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


                        bp.Distance = (int)Vector3.Distance(player.transform.position, Globals.LocalPlayer.transform.position);
                        bp.Name = bp.SteamPlayer.playerID.playerName;
                        bp.Colour = ColourHelper.GetColour("Player Text Colour");
                        if(bp.Friendly)
                            bp.Colour = ColourHelper.GetColour("Friendly-Player Text Colour");
                        bp.W2S = WorldPointToScreenPoint(player.transform.position);
                        bp.HeadW2S = WorldPointToScreenPoint(Globals.GetLimbPosition(player.transform, "Skull"));
                        bp.FootW2S = WorldPointToScreenPoint(Globals.GetLimbPosition(player.transform, "Left_Foot"));
                        bp.Alive = !player.life.isDead;
                        bp.Weapon = player.equipment.asset != null ? player.equipment?.asset?.itemName : "Empty";
                        bp.Visible = RaycastHelper.IsPointVisible(player, Globals.GetLimbPosition(player.transform, "Skull"));
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
                        bp.Bounds = new Bounds(bp.Entity.transform.position + new Vector3(0, 1.1f, 0), bp.Entity.transform.localScale + new Vector3(0, .95f, 0));
                        bp.BoundPoints[0] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x + bp.Bounds.extents.x, Globals.GetLimbPosition(player.transform, "Skull").y + (bp.Bounds.extents.y / 2), bp.Bounds.center.z + bp.Bounds.extents.z));
                        bp.BoundPoints[1] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x + bp.Bounds.extents.x, Globals.GetLimbPosition(player.transform, "Skull").y + (bp.Bounds.extents.y / 2), bp.Bounds.center.z - bp.Bounds.extents.z));
                        bp.BoundPoints[2] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x + bp.Bounds.extents.x, bp.Bounds.center.y - bp.Bounds.extents.y, bp.Bounds.center.z + bp.Bounds.extents.z));
                        bp.BoundPoints[3] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x + bp.Bounds.extents.x, bp.Bounds.center.y - bp.Bounds.extents.y, bp.Bounds.center.z - bp.Bounds.extents.z));
                        bp.BoundPoints[4] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x - bp.Bounds.extents.x, Globals.GetLimbPosition(player.transform, "Skull").y + (bp.Bounds.extents.y / 2), bp.Bounds.center.z + bp.Bounds.extents.z));
                        bp.BoundPoints[5] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x - bp.Bounds.extents.x, Globals.GetLimbPosition(player.transform, "Skull").y + (bp.Bounds.extents.y / 2), bp.Bounds.center.z - bp.Bounds.extents.z));
                        bp.BoundPoints[6] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x - bp.Bounds.extents.x, bp.Bounds.center.y - bp.Bounds.extents.y, bp.Bounds.center.z + bp.Bounds.extents.z));
                        bp.BoundPoints[7] = Globals.WorldPointToScreenPoint(new UnityEngine.Vector3(bp.Bounds.center.x - bp.Bounds.extents.x, bp.Bounds.center.y - bp.Bounds.extents.y, bp.Bounds.center.z - bp.Bounds.extents.z));

                        bp.BonePosition[0] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Right_Foot"));
                        bp.BonePosition[1] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Left_Foot"));
                        bp.BonePosition[2] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Right_Leg"));
                        bp.BonePosition[3] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Left_Leg"));
                        bp.BonePosition[4] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Right_Arm"));
                        bp.BonePosition[5] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Left_Arm"));
                        bp.BonePosition[6] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Right_Hand"));
                        bp.BonePosition[7] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Left_Hand"));
                        bp.BonePosition[8] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Spine"));
                        bp.BonePosition[9] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bp.Entity.transform, "Skull"));
                        bp.BonePosition[10] = new Vector3(bp.BonePosition[8].x, bp.BonePosition[5].y, bp.BonePosition[5].z);

                        bp.WorldBonePosition[0] = (Globals.GetLimbPosition(bp.Entity.transform, "Right_Foot"));
                        bp.WorldBonePosition[1] = (Globals.GetLimbPosition(bp.Entity.transform, "Left_Foot"));
                        bp.WorldBonePosition[2] = (Globals.GetLimbPosition(bp.Entity.transform, "Right_Leg"));
                        bp.WorldBonePosition[3] = (Globals.GetLimbPosition(bp.Entity.transform, "Left_Leg"));
                        bp.WorldBonePosition[4] = (Globals.GetLimbPosition(bp.Entity.transform, "Right_Arm"));
                        bp.WorldBonePosition[5] = (Globals.GetLimbPosition(bp.Entity.transform, "Left_Arm"));
                        bp.WorldBonePosition[6] = (Globals.GetLimbPosition(bp.Entity.transform, "Right_Hand"));
                        bp.WorldBonePosition[7] = (Globals.GetLimbPosition(bp.Entity.transform, "Left_Hand"));
                        bp.WorldBonePosition[8] = (Globals.GetLimbPosition(bp.Entity.transform, "Spine"));
                        bp.WorldBonePosition[9] = (Globals.GetLimbPosition(bp.Entity.transform, "Skull"));
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
                catch(Exception ex) { System.IO.File.WriteAllText("test2454.txt", ex.Message); }
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
                        bz.BonePosition[4] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Right_Arm"));
                        bz.BonePosition[5] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Left_Arm"));
                        bz.BonePosition[6] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Right_Hand"));
                        bz.BonePosition[7] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Left_Hand"));
                        bz.BonePosition[8] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Spine"));
                        bz.BonePosition[9] = Globals.WorldPointToScreenPoint(Globals.GetLimbPosition(bz.Entity.transform, "Skull"));
                        bz.BonePosition[10] = new Vector3(bz.BonePosition[8].x, bz.BonePosition[5].y, bz.BonePosition[5].z);

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
