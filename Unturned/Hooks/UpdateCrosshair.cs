using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
using System.Reflection;
using Hag.Helpers;
namespace Hag.Hooks
{
	public class UpdateCrosshair : MonoBehaviour
	{

		public DumbHook UpdateCrosshairsHook;
		void Start()
		{
		//	UpdateCrosshairsHook = new DumbHook();
		//	UpdateCrosshairsHook.Init(typeof(SDG.Unturned.UseableGun).GetMethod("updateCrosshair", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance), typeof(UpdateCrosshair).GetMethod("updateCrosshair", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
		//	UpdateCrosshairsHook.Hook();
		}
	
		private static float applySpreadModifiers(float value)
		{
			if ((Player.player?.equipment?.asset is ItemGunAsset))
			{
				ItemGunAsset Weapon = (ItemGunAsset)Player.player?.equipment?.asset;
				UseableGun useablegun = (UseableGun)Player.player.equipment.useable;
				if (Hag.Misc.Weapon.ValueLog.ContainsKey(Weapon.id) && Globals.Config.Weapon.NoSpread)
				{
					if (Player.player.stance.stance == EPlayerStance.SPRINT)
					{
						value = useablegun.equippedGunAsset.spreadSprint;
						return value;
					}
					if (Player.player.stance.stance == EPlayerStance.CROUCH)
					{
						value = useablegun.equippedGunAsset.spreadCrouch;
						return value;
					}
					if (Player.player.stance.stance == EPlayerStance.PRONE)
					{
						value = useablegun.equippedGunAsset.spreadProne;
					}
					return value;
				}
			}
			return value;
		}

		private void updateCrosshair()
		{
			if ((Player.player?.equipment?.asset is ItemGunAsset))
			{
				ItemGunAsset Weapon = (ItemGunAsset)Player.player?.equipment?.asset;
				if (Hag.Misc.Weapon.ValueLog.ContainsKey(Weapon.id) && Globals.Config.Weapon.NoSpread)
				{
					float crosshair = 0;
					UseableGun useablegun = (UseableGun)Player.player.equipment.useable;
					Attachments attachment = Player.player.equipment.thirdModel.gameObject.GetComponent<Attachments>();
					crosshair = useablegun.equippedGunAsset.spreadHip;
					crosshair *=  1f - Player.player.skills.mastery(0, 1) * 0.5f;
					if (attachment.tacticalAsset != null && useablegun.GetPrivateField<bool>("shouldEnableTacticalStats") == true && (!attachment.tacticalAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
					{
						
						crosshair *= attachment.tacticalAsset.spread;
					}
					if (attachment.gripAsset != null && (!attachment.gripAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
					{
						crosshair *= attachment.gripAsset.spread;
					}
					if (attachment.barrelAsset != null && (!attachment.barrelAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
					{
						crosshair*= attachment.barrelAsset.spread;
					}
					if (attachment.magazineAsset != null && (!attachment.magazineAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
					{
						crosshair *= attachment.magazineAsset.spread;
					}
					crosshair *= applySpreadModifiers(crosshair);

					if (useablegun.isAiming)
					{
						if (Player.player.look.perspective == EPlayerPerspective.FIRST)
						{
							crosshair *= useablegun.equippedGunAsset.spreadAim;
						}
						else
						{
							crosshair *= 0.5f;
						}
					}

					PlayerUI.updateCrosshair(crosshair);
					if ((!useablegun.equippedGunAsset.isTurret && useablegun.equippedGunAsset.action != EAction.Minigun && ((useablegun.isAiming && Player.player.look.perspective == EPlayerPerspective.FIRST && (useablegun.equippedGunAsset.action != EAction.String || attachment.sightHook != null)) || useablegun.GetPrivateField<bool>("isAttaching"))) || (Player.player.movement.getVehicle() != null && Player.player.look.perspective != EPlayerPerspective.FIRST))
					{
						PlayerUI.disableCrosshair();
						return;
					}
					PlayerUI.enableCrosshair();
				}
			}
			else
			{
				System.IO.File.WriteAllText("test55.txt", "test");
			}

		}
		public static float updateCrosshairs()
		{
			float crosshair = 0;
			if ((Player.player?.equipment?.asset is ItemGunAsset))
			{
				ItemGunAsset Weapon = (ItemGunAsset)Player.player?.equipment?.asset;
				if (Hag.Misc.Weapon.ValueLog.ContainsKey(Weapon.id) && Globals.Config.Weapon.NoSpread)
				{
					
					UseableGun useablegun = (UseableGun)Player.player.equipment.useable;
					Attachments attachment = Player.player.equipment.thirdModel.gameObject.GetComponent<Attachments>();
					crosshair = useablegun.equippedGunAsset.spreadHip;
					crosshair *= 1f - Player.player.skills.mastery(0, 1) * 0.5f;
					if (attachment.tacticalAsset != null && useablegun.GetPrivateField<bool>("shouldEnableTacticalStats") == true && (!attachment.tacticalAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
					{

						crosshair *= attachment.tacticalAsset.spread;
					}
					if (attachment.gripAsset != null && (!attachment.gripAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
					{
						crosshair *= attachment.gripAsset.spread;
					}
					if (attachment.barrelAsset != null && (!attachment.barrelAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
					{
						crosshair *= attachment.barrelAsset.spread;
					}
					if (attachment.magazineAsset != null && (!attachment.magazineAsset.ShouldOnlyAffectAimWhileProne || Player.player.stance.stance == EPlayerStance.PRONE))
					{
						crosshair *= attachment.magazineAsset.spread;
					}
					crosshair *= applySpreadModifiers(crosshair);

					if (useablegun.isAiming)
					{
						if (Player.player.look.perspective == EPlayerPerspective.FIRST)
						{
							crosshair *= useablegun.equippedGunAsset.spreadAim;
						}
						else
						{
							crosshair *= 0.5f;
						}
					}
					PlayerUI.updateCrosshair(crosshair);
					if ((!useablegun.equippedGunAsset.isTurret && useablegun.equippedGunAsset.action != EAction.Minigun && ((useablegun.isAiming && Player.player.look.perspective == EPlayerPerspective.FIRST && (useablegun.equippedGunAsset.action != EAction.String || attachment.sightHook != null)) || useablegun.GetPrivateField<bool>("isAttaching"))) || (Player.player.movement.getVehicle() != null && Player.player.look.perspective != EPlayerPerspective.FIRST))
					{
						PlayerUI.disableCrosshair();
						return crosshair;
					}
					PlayerUI.enableCrosshair();
				}
			}
			else
			{
				System.IO.File.WriteAllText("test55.txt", "test");
			}
			return crosshair;

		}

	}
}
