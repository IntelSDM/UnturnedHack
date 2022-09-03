using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
namespace Hag.Esp_Objects
{
    class BaseItem
    {
        public BaseItem(InteractableItem item)
        {
            Entity = item;
        }
        public Vector3 W2S;
        public int Distance;
        public string Name;
        public Color32 Colour;
        public InteractableItem Entity;
    }
}
