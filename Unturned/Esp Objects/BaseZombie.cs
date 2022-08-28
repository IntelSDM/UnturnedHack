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
        public bool Visible;

        public int Distance;
        public int test;

        public string Tag;

        public Vector3 W2S;
        public Vector3 HeadW2S;
        public Vector3 FootW2S;

        public Bounds Bounds;
        public UnityEngine.Vector3[] BoundPoints = new UnityEngine.Vector3[8];
        public Vector3[] BonePosition = new Vector3[11];
        public Vector3[] WorldBonePosition = new Vector3[11];

        public Color32 Colour;
        public Color32 VisibleColour;
        public Color32 InvisibleColour;
        public Color32 BoxColour;
        public Color32 FilledBoxColour;
        public Color32[] BoneColour = new Color32[11];

    }
}
