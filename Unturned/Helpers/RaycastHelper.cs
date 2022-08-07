using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
namespace Hag.Helpers
{
    class RaycastHelper
    {
        private static RaycastHit RaycastHit;
        private static readonly LayerMask Mask = RayMasks.DAMAGE_CLIENT;
        public static bool IsPointVisible(Player player, Vector3 BonePos)
        {
            return Physics.Linecast(
                  Camera.main.transform.position,
                 BonePos,
                  out RaycastHit,
                  Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject;
                
        }
        public static bool IsPointVisible(Zombie zombie, Vector3 BonePos)
        {
            return Physics.Linecast(
                    Camera.main.transform.position,
                   BonePos,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == zombie.gameObject.transform.root.gameObject;
        }

    }
}
