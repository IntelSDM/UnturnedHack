using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
using Hag.Esp_Objects;
namespace Hag
{
    class Globals : MonoBehaviour
    {
        public static Camera MainCamera;
        public static SDG.Unturned.Player LocalPlayer;

        public static Hag.Configs.Config Config = new Configs.Config();
        public static bool EndedFrame = true;

        public static List<BaseZombie> ZombieList = new List<BaseZombie>();
        public static bool IsScreenPointVisible(Vector3 screenPoint)
        {
            return screenPoint.z > 0.01f && screenPoint.x > -5f && screenPoint.y > -5f && screenPoint.x < (float)Screen.width && screenPoint.y < (float)Screen.height;
        }
        void Start()
        {
            Helpers.ConfigHelper.CreateEnvironment();
            Helpers.ColourHelper.AddColours();
            Esp.Drawing drawing = new Esp.Drawing();
            drawing.Start();
            MainCamera = Camera.main;
        }
    }
}
