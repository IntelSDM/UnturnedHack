using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using UnityEngine;
namespace Hag.Esp_Objects
{
    class BaseVehicle
    {
        public BaseVehicle(InteractableVehicle vehicle)
        {
            Entity = vehicle;
        }
        public void TeleportToCar()
        {

          //  Entity.forceRemoveAllPlayers();
            Entity.addPlayer(0, Player.player.channel.owner.playerID.steamID);// FUCKING TELEPORT INTO LOCKED CARS

        }
        public void LeaveCar()
        {
         
                    Entity.removePlayer(0, Vector3.zero, (byte)0, true);
            
     //   Entity.removePlayer
        }
        public InteractableVehicle Entity;
        public string Name;

        public int Distance;

        public bool Locked;
        public bool IsDriven;
        public bool OwnedByYou;

        public List<BasePlayer> Players;

        public Vector3 W2S;

        public Color32 Colour;
    }
}
