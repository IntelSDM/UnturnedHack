using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
namespace Hag.Esp_Objects
{
    class BaseZombie
    {
        public BaseZombie(Zombie zombie)
        {
            Entity = zombie;
        }
        public Zombie Entity;

        public bool Alive;

        public int Distance;

        public string Tag;

        public Vector3 W2S;
        public Vector3 HeadW2S;

        public Color32 Colour;
        public Color32 VisibleColour;
        public Color32 InvisibleColour;
        public Color32 BoxColour;
        public Color32 FilledBoxColour;

    }
}
