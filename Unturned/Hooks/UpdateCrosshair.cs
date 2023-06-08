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

		
	

	}
}
