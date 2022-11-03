using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hag.Helpers;
using UnityEngine;
using SDG.Unturned;
using System.Reflection;
using SDG.NetTransport;

namespace Hag.Hooks
{
    class Fire : MonoBehaviour
    {

		public DumbHook FireHook;
		void Start()
		{
			FireHook = new DumbHook();
			FireHook.Init(typeof(SDG.Unturned.UseableGun).GetMethod("fire", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance), typeof(Fire).GetMethod("fire", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
			FireHook.Hook();
		}
        public static event UseableGun.BulletSpawnedHandler onBulletSpawned;
        private static readonly ClientInstanceMethod<Vector3, Vector3, ushort, ushort> SendPlayProject = ClientInstanceMethod<Vector3, Vector3, ushort, ushort>.Get(typeof(UseableGun), "ReceivePlayProject");
        private static readonly ClientInstanceMethod<byte> SendPlayChamberJammed = ClientInstanceMethod<byte>.Get(typeof(UseableGun), "ReceivePlayChamberJammed");
        private static readonly float SHAKE_CROUCH = 0.85f;

		// Token: 0x04002D15 RID: 11541
		private static readonly float SHAKE_PRONE = 0.7f;

		// Token: 0x04002D16 RID: 11542
		private static readonly float SWAY_CROUCH = 0.85f;

		// Token: 0x04002D17 RID: 11543
		private static readonly float SWAY_PRONE = 0.7f;

        private float applySpreadModifiers(float value)
        {
            UseableGun useablegun = (UseableGun)Player.player.equipment.useable;
            if (Player.player.stance.stance == EPlayerStance.SPRINT)
            {
                if (Globals.Config.Weapon.NoSpread)
                    value = Globals.Config.Weapon.NoSpreadSprint;
                else
                    value = useablegun.equippedGunAsset.spreadSprint;
     
                return value;
            }
            if (Player.player.stance.stance == EPlayerStance.CROUCH)
            {
                if (Globals.Config.Weapon.NoSpread)
                    value = Globals.Config.Weapon.NoSpreadCrouch;
                else
                    value = useablegun.equippedGunAsset.spreadCrouch;
                return value;
            }
            if (Player.player.stance.stance == EPlayerStance.PRONE)
            {
                if (Globals.Config.Weapon.NoSpread)
                    value = Globals.Config.Weapon.NoSpreadProne;
                else
                    value = useablegun.equippedGunAsset.spreadProne;
                return value;
            }
            return 1;
        }
        private float applyRecoilMagnitudeModifiers( float value)
        {
            UseableGun useablegun = (UseableGun)Player.player.equipment.useable;
            if (Player.player.stance.stance == EPlayerStance.SPRINT)
            {
                value = useablegun.equippedGunAsset.recoilSprint;
                return value;
            }
            if (Player.player.stance.stance == EPlayerStance.CROUCH)
            {
                value = useablegun.equippedGunAsset.recoilCrouch;
                return value;
            }
            if (Player.player.stance.stance == EPlayerStance.PRONE)
            {
                value *= useablegun.equippedGunAsset.recoilProne;
                return value;
            }
            return 1;
        }
        private void fire()
        {
            // UseableGun useablegun = (UseableGun)Player.player.equipment.useable;
            //       FireHook.Unhook();
            //    object result =  FireHook.OriginalMethod.Invoke(this, null);
            //      FireHook.Hook();
            //     return result;
            try
            {
              
                    MethodInfo project = typeof(UseableGun).GetMethod("project", BindingFlags.Instance | BindingFlags.NonPublic);
                    MethodInfo shoot = typeof(UseableGun).GetMethod("shoot", BindingFlags.Instance | BindingFlags.NonPublic);
                    MethodInfo updateinfo = typeof(UseableGun).GetMethod("updateInfo", BindingFlags.Instance | BindingFlags.NonPublic);
                    ItemGunAsset Weapon = (ItemGunAsset)Player.player?.equipment?.asset;
                    UseableGun useablegun = (UseableGun)Player.player.equipment.useable;
                    Attachments attachment = Player.player.equipment.thirdModel.gameObject.GetComponent<Attachments>();
                    byte ammo = useablegun.GetPrivateField<byte>("ammo");
                    byte aimAccuracy = useablegun.GetPrivateField<byte>("aimAccuracy");
                    bool shouldEnableTacticalStats = attachment.tacticalAsset != null && ((!attachment.tacticalAsset.isLaser && !attachment.tacticalAsset.isLight && !attachment.tacticalAsset.isRangefinder) || useablegun.GetPrivateField<bool>("interact")); 
                    List<BulletInfo> bullets = useablegun.GetPrivateField<List<BulletInfo>>("bullets");
                    float num = (float)Player.player.equipment.quality / 100f;
                    if (attachment.magazineAsset == null)
                    {
                        return;
                    }
                    if (!useablegun.equippedGunAsset.infiniteAmmo)
                    {
                        if (ammo < useablegun.equippedGunAsset.ammoPerShot)
                        {
                            throw new Exception("Insufficient ammo");
                        }
                    useablegun.SetPrivateField("ammo", ammo -= useablegun.equippedGunAsset.ammoPerShot);
                    
                        if (useablegun.equippedGunAsset.action != EAction.String)
                        {
                            Player.player.equipment.state[10] = ammo;
                            Player.player.equipment.updateState();
                        }
                    }
                    if (Player.player.channel.isOwner && ammo < useablegun.equippedGunAsset.ammoPerShot)
                    {
                        PlayerUI.message(EPlayerMessage.RELOAD, "", 2f);
                    }
                    if (!useablegun.isAiming)
                    {
                        Player.player.equipment.uninspect();
                    }
                    if (Player.player.channel.isOwner)
                    {
                        float num2 = (num < 0.5f) ? (1f + (1f - num * 2f)) : 1f;
                        if (useablegun.isAiming)
                        {
                        if (Globals.Config.Weapon.NoSpread)
                            num2 *= Mathf.Lerp(1f, Globals.Config.Weapon.NoSpreadAim, (float)aimAccuracy / 10f);
                        else
                            num2 *= Mathf.Lerp(1f, useablegun.equippedGunAsset.spreadAim, (float)aimAccuracy / 10f);
                    }
                        num2 *= 1f - Player.player.skills.mastery(0, 1) * 0.5f;
                        if (attachment.sightAsset != null && useablegun.isAiming && (!attachment.sightAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
                        {
                            num2 *= Mathf.Lerp(1f, attachment.sightAsset.spread, (float)aimAccuracy / 10f);
                        }
                        if (attachment.tacticalAsset != null && shouldEnableTacticalStats && (!attachment.tacticalAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
                        {
                            num2 *= attachment.tacticalAsset.spread;
                        }
                        if (attachment.gripAsset != null && (!attachment.gripAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
                        {
                            num2 *= attachment.gripAsset.spread;
                        }
                        if (attachment.barrelAsset != null && (!attachment.barrelAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
                        {
                            num2 *= attachment.barrelAsset.spread;
                        }
                        if (attachment.magazineAsset != null && (!attachment.magazineAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
                        {
                            num2 *= attachment.magazineAsset.spread;
                        }
                        num2 *= applySpreadModifiers(num2);
                        if (!Player.player.look.isCam && Player.player.look.perspective == EPlayerPerspective.THIRD)
                        {
                            RaycastHit raycastHit;
                            Physics.Raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out raycastHit, 512f, RayMasks.DAMAGE_CLIENT);
                            if (raycastHit.transform != null)
                            {
                                if (Vector3.Dot(raycastHit.point - Player.player.look.aim.position, MainCamera.instance.transform.forward) > 0f)
                                {
                                    Player.player.look.aim.rotation = Quaternion.LookRotation(raycastHit.point - Player.player.look.aim.position);
                                }
                            }
                            else
                            {
                                Player.player.look.aim.rotation = Quaternion.LookRotation(MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 512f - Player.player.look.aim.position);
                            }
                        }
                        if (useablegun.equippedGunAsset.projectile == null)
                        {
                            byte pellets = attachment.magazineAsset.pellets;
                            for (byte b = 0; b < pellets; b += 1)
                            {
                                Vector3 vector = Player.player.look.aim.forward;
                            if (Globals.Config.Weapon.NoSpread)
                            {
                                vector += Player.player.look.aim.right * UnityEngine.Random.Range(-Globals.Config.Weapon.NoSpreadHip, Globals.Config.Weapon.NoSpreadHip) * num2;
                                vector += Player.player.look.aim.up * UnityEngine.Random.Range(-Globals.Config.Weapon.NoSpreadHip, Globals.Config.Weapon.NoSpreadHip) * num2;
                            }
                            else 
                            {
                                vector += Player.player.look.aim.right * UnityEngine.Random.Range(-useablegun.equippedGunAsset.spreadHip, useablegun.equippedGunAsset.spreadHip) * num2;
                                vector += Player.player.look.aim.up * UnityEngine.Random.Range(-useablegun.equippedGunAsset.spreadHip, useablegun.equippedGunAsset.spreadHip) * num2;
                            }
                                vector.Normalize();
                                BulletInfo bulletInfo = new BulletInfo();
                                bulletInfo.origin = Player.player.look.aim.position;
                                bulletInfo.pos = bulletInfo.origin;
                                bulletInfo.dir = vector;
                                bulletInfo.pellet = b;
                                bulletInfo.quality = num;
                                bulletInfo.barrelAsset = attachment.barrelAsset;
                                bulletInfo.magazineAsset = attachment.magazineAsset;
                                bullets.Add(bulletInfo);
                                int num3;
                                if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num3))
                                {
                                    Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num3 + 1);
                                }
                            }
                        }
                        else
                        {
                            Vector3 forward = Player.player.look.aim.forward;
                            RaycastInfo raycastInfo = DamageTool.raycast(new Ray(Player.player.look.aim.position, forward), 512f, RayMasks.DAMAGE_CLIENT, Player.player);
                            if (raycastInfo.transform != null)
                            {
                                Player.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Gun);
                            }
                            Vector3 vector2 = Player.player.look.aim.position;
                            RaycastHit raycastHit2;
                            if (!Physics.Raycast(new Ray(vector2, forward), out raycastHit2, 1f, RayMasks.DAMAGE_SERVER))
                            {
                                vector2 += forward;
                            }
                         
                            object[] param = new object[] { vector2, forward, attachment.barrelAsset, attachment.magazineAsset };
                            project.Invoke(useablegun, param);
                            //   this.project();
                            int num4;
                            if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num4))
                            {
                                Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num4 + 1);
                            }
                        }
                        float num5 = UnityEngine.Random.Range(useablegun.equippedGunAsset.recoilMin_x, useablegun.equippedGunAsset.recoilMax_x) * ((num < 0.5f) ? (1f + (1f - num * 2f)) : 1f);
                        float num6 = UnityEngine.Random.Range(useablegun.equippedGunAsset.recoilMin_y, useablegun.equippedGunAsset.recoilMax_y) * ((num < 0.5f) ? (1f + (1f - num * 2f)) : 1f);
                        float num7 = UnityEngine.Random.Range(useablegun.equippedGunAsset.shakeMin_x, useablegun.equippedGunAsset.shakeMax_x);
                        float num8 = UnityEngine.Random.Range(useablegun.equippedGunAsset.shakeMin_y, useablegun.equippedGunAsset.shakeMax_y);
                        float num9 = UnityEngine.Random.Range(useablegun.equippedGunAsset.shakeMin_z, useablegun.equippedGunAsset.shakeMax_z);
                        num5 *= 1f - Player.player.skills.mastery(0, 1) * 0.5f;
                        num6 *= 1f - Player.player.skills.mastery(0, 1) * 0.5f;
                        if (attachment.sightAsset != null)
                        {
                            if (useablegun.isAiming && useablegun.equippedGunAsset.useRecoilAim && attachment.sightAsset.zoom > 2f)
                            {
                                num5 *= Mathf.Lerp(1f, 1f / attachment.sightAsset.zoom * useablegun.equippedGunAsset.recoilAim, (float)aimAccuracy / 10f * ((Player.player.look.perspective == EPlayerPerspective.FIRST) ? 1f : 0.5f));
                                num6 *= Mathf.Lerp(1f, 1f / attachment.sightAsset.zoom * useablegun.equippedGunAsset.recoilAim, (float)aimAccuracy / 10f * ((Player.player.look.perspective == EPlayerPerspective.FIRST) ? 1f : 0.5f));
                            }
                            num5 *= attachment.sightAsset.recoil_x;
                            num6 *= attachment.sightAsset.recoil_y;
                            num7 *= attachment.sightAsset.shake;
                            num8 *= attachment.sightAsset.shake;
                            num9 *= attachment.sightAsset.shake;
                        }
                        if (attachment.tacticalAsset != null && shouldEnableTacticalStats)
                        {
                            num5 *= attachment.tacticalAsset.recoil_x;
                            num6 *= attachment.tacticalAsset.recoil_y;
                            num7 *= attachment.tacticalAsset.shake;
                            num8 *= attachment.tacticalAsset.shake;
                            num9 *= attachment.tacticalAsset.shake;
                        }
                        if (attachment.gripAsset != null && (!attachment.gripAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
                        {
                            num5 *= attachment.gripAsset.recoil_x;
                            num6 *= attachment.gripAsset.recoil_y;
                            num7 *= attachment.gripAsset.shake;
                            num8 *= attachment.gripAsset.shake;
                            num9 *= attachment.gripAsset.shake;
                        }
                        if (attachment.barrelAsset != null)
                        {
                            num5 *= attachment.barrelAsset.recoil_x;
                            num6 *= attachment.barrelAsset.recoil_y;
                            num7 *= attachment.barrelAsset.shake;
                            num8 *= attachment.barrelAsset.shake;
                            num9 *= attachment.barrelAsset.shake;
                        }
                        if (attachment.magazineAsset != null)
                        {
                            num5 *= attachment.magazineAsset.recoil_x;
                            num6 *= attachment.magazineAsset.recoil_y;
                            num7 *= attachment.magazineAsset.shake;
                            num8 *= attachment.magazineAsset.shake;
                            num9 *= attachment.magazineAsset.shake;
                        }
                        num5 *= applyRecoilMagnitudeModifiers(num5);
                        num5 *= applyRecoilMagnitudeModifiers(num6);
                        if (Player.player.stance.stance == EPlayerStance.CROUCH)
                        {
                            num7 *= SHAKE_CROUCH;
                            num8 *= SHAKE_CROUCH;
                            num9 *= SHAKE_CROUCH;
                        }
                        else if (Player.player.stance.stance == EPlayerStance.PRONE)
                        {
                            num7 *= SHAKE_PRONE;
                            num8 *= SHAKE_PRONE;
                            num9 *= SHAKE_PRONE;
                        }
                        Player.player.look.recoil(num5, num6, useablegun.equippedGunAsset.recover_x, useablegun.equippedGunAsset.recover_y);
                        Player.player.animator.AddRecoilViewmodelCameraOffset(num7, num8, num9);
                    Player.player.animator.AddRecoilViewmodelCameraRotation(num5, num6);
                    updateinfo.Invoke(useablegun, null);
                        if (useablegun.equippedGunAsset.projectile == null)
                        {
                            shoot.Invoke(useablegun, null);
                        }
                    }
                    if (Provider.isServer)
                    {

                        if (!Player.player.channel.isOwner && attachment.barrelAsset != null && attachment.barrelAsset.durability > 0)
                        {
                            if (attachment.barrelAsset.durability > Player.player.equipment.state[16])
                            {
                                Player.player.equipment.state[16] = 0;
                            }
                            else
                            {
                                byte[] state = Player.player.equipment.state;
                                int num10 = 16;
                                state[num10] -= attachment.barrelAsset.durability;
                            }
                            Player.player.equipment.updateState();
                        }
                        useablegun.equippedGunAsset.GrantShootQuestRewards(Player.player);
                        if (useablegun.equippedGunAsset.projectile == null)
                        {
                            byte pellets2 = attachment.magazineAsset.pellets;
                            for (byte b2 = 0; b2 < pellets2; b2 += 1)
                            {
                                BulletInfo bulletInfo2;
                                if (Player.player.channel.isOwner)
                                {
                                    bulletInfo2 = bullets[bullets.Count - (int)pellets2 + (int)b2];
                                }
                                else
                                {
                                    bulletInfo2 = new BulletInfo();
                                    bulletInfo2.origin = Player.player.look.aim.position;
                                    bulletInfo2.pos = bulletInfo2.origin;
                                    bulletInfo2.pellet = b2;
                                    bulletInfo2.quality = num;
                                    bulletInfo2.barrelAsset = attachment.barrelAsset;
                                    bulletInfo2.magazineAsset = attachment.magazineAsset;
                                    bullets.Add(bulletInfo2);
                                    if (onBulletSpawned != null)
                                    {
                                        onBulletSpawned(useablegun, bulletInfo2);
                                    }
                                }
                                if (attachment.magazineAsset != null && attachment.magazineAsset.isExplosive)
                                {
                                    if (useablegun.equippedGunAsset.action == EAction.String)
                                    {
                                        Player.player.equipment.state[8] = 0;
                                        Player.player.equipment.state[9] = 0;
                                        Player.player.equipment.state[10] = 0;
                                        Player.player.equipment.state[17] = 0;
                                        Player.player.equipment.sendUpdateState();
                                    }
                                }
                                else if (useablegun.equippedGunAsset.action == EAction.String)
                                {
                                    if (Player.player.equipment.state[17] > 0)
                                    {
                                        if (Player.player.equipment.state[17] > attachment.magazineAsset.stuck)
                                        {
                                            byte[] state2 = Player.player.equipment.state;
                                            int num11 = 17;
                                            state2[num11] -= attachment.magazineAsset.stuck;
                                        }
                                        else
                                        {
                                            Player.player.equipment.state[17] = 0;
                                        }
                                        bulletInfo2.dropID = attachment.magazineID;
                                        bulletInfo2.dropAmount = Player.player.equipment.state[10];
                                        bulletInfo2.dropQuality = Player.player.equipment.state[17];
                                    }
                                    Player.player.equipment.state[8] = 0;
                                    Player.player.equipment.state[9] = 0;
                                    Player.player.equipment.state[10] = 0;
                                    Player.player.equipment.sendUpdateState();
                                }
                            }
                        }
                        else
                        {
                            ItemBarrelAsset itemBarrelAsset = (Player.player.equipment.state[16] > 0) ? attachment.barrelAsset : null;
                            ItemMagazineAsset magazineAsset = attachment.magazineAsset;
                            if (Player.player.input.hasInputs())
                            {
                                InputInfo input = Player.player.input.getInput(false, ERaycastInfoUsage.Gun);
                                if (input != null && input.transform != null)
                                {
                                    Player.player.look.aim.LookAt(input.point);
                                }
                            }
                            if (ammo == 0 && useablegun.equippedGunAsset.shouldDeleteEmptyMagazines)
                            {
                                Player.player.equipment.state[8] = 0;
                                Player.player.equipment.state[9] = 0;
                                Player.player.equipment.state[10] = 0;
                                Player.player.equipment.sendUpdateState();
                            }
                            if (!Player.player.channel.isOwner)
                            {
                                Vector3 vector3 = Player.player.look.aim.position;
                                Vector3 forward2 = Player.player.look.aim.forward;
                                RaycastHit raycastHit3;
                                if (!Physics.Raycast(new Ray(vector3, forward2), out raycastHit3, 1f, RayMasks.DAMAGE_SERVER))
                                {
                                    vector3 += forward2;
                                }
                                object[] param2 = new object[] { vector3, forward2, itemBarrelAsset, magazineAsset };
                                project.Invoke(useablegun, param2);
                                SendPlayProject.Invoke(Player.player.GetNetId(), ENetReliability.Unreliable, (ITransportConnection)(IEnumerable<ITransportConnection>)Player.player.channel.EnumerateClients_RemoteNotOwner(), vector3, forward2, (ushort)((itemBarrelAsset != null) ? itemBarrelAsset.id : 0), (ushort)((magazineAsset != null) ? magazineAsset.id : 0));
                            }
                            Player.player.life.markAggressive(false, true);
                        }
                    }
                    if (useablegun.equippedGunAsset.canEverJam && Provider.isServer && num < useablegun.equippedGunAsset.jamQualityThreshold)
                    {
                        float t = 1f - num / useablegun.equippedGunAsset.jamQualityThreshold;
                        float num12 = Mathf.Lerp(0f, useablegun.equippedGunAsset.jamMaxChance, t);
                        if (UnityEngine.Random.value < num12)
                        {
                            SendPlayChamberJammed.InvokeAndLoopback(Player.player.GetNetId(), ENetReliability.Reliable, Provider.EnumerateClients_Remote(), ammo);
                        }
                    }
               
            }
            catch (Exception ex) { System.IO.File.WriteAllText("degg.txt", ex.ToString()); }
            }

        }
    }
