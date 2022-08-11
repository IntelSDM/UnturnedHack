using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Helpers
{
    class ColourHelper
    {
        #region RGBColours
        public static int Cases = 0;
        public static float R = 1.00f;
        public static float G = 0.00f;
        public static float B = 1.00f;
        #endregion
        public static void AddColours()
        {
            AddColour("Menu Primary Colour", Color.red);
            AddColour("Menu Secondary Colour", Color.white);
            AddColour("Menu Radar Colour", new Color32(10, 10, 10, 180));

            AddColour("Zombie Text Colour", Color.red);
            AddColour("Zombie Visible Filled Box Colour", new Color32(255, 0, 100, 50));
            AddColour("Zombie Visible Box Colour", Color.magenta);
            AddColour("Zombie Invisible Filled Box Colour", new Color32(0, 0, 0, 95));
            AddColour("Zombie Invisible Box Colour", Color.red);
            AddColour("Zombie Bone Invisible Colour", Color.magenta);
            AddColour("Zombie Bone Visible Colour", Color.red);

            AddColour("Player Text Colour", Color.red);
            AddColour("Player Visible Filled Box Colour", new Color32(255, 0, 100, 50));
            AddColour("Player Visible Box Colour", Color.magenta);
            AddColour("Player Invisible Filled Box Colour", new Color32(0, 0, 0, 95));
            AddColour("Player Invisible Box Colour", Color.red);
            AddColour("Player Bone Invisible Colour", Color.magenta);
            AddColour("Player Bone Visible Colour", Color.red);
        }
        public static Color32 GetColour(string identifier)
        {
            if (Globals.Config.Colours.GlobalColors.TryGetValue(identifier, out var toret))
                return toret;
            return Color.magenta;
        }

        public static void AddColour(string id, Color32 c)
        {
            if (!Globals.Config.Colours.GlobalColors.ContainsKey(id))
                Globals.Config.Colours.GlobalColors.Add(id, c);
        }

        public static void SetColour(string id, Color32 c) => Globals.Config.Colours.GlobalColors[id] = c;

        public static string ColourToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
    }
}
