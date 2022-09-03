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
