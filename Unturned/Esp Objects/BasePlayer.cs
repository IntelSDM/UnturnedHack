using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
namespace Hag.Esp_Objects
{
    class BasePlayer
    {
        public BasePlayer(Player player, SteamPlayer steamplayer)
        {
            Entity = player;
            this.SteamPlayer = steamplayer;
        }
        public Player Entity;
        public SteamPlayer SteamPlayer;

        public bool Alive;
        public bool NPC;
        public bool Visible;
        public bool Friendly = false;

        public int Distance;

        public string Name;
        public string Weapon;

        public Vector3 W2S;
        public Vector3 HeadW2S;
        public Vector3 FootW2S;
        public Vector3 Velocity;

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
