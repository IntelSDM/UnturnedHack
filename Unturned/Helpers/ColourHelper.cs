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
            AddColour("Radar Colour", new Color32(10, 10, 10, 180));

            AddColour("Zombie Colour", Color.magenta);

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
