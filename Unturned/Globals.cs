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
        public static List<BasePlayer> PlayerList = new List<BasePlayer>();
        public static bool IsScreenPointVisible(Vector3 screenPoint)
        {
            return screenPoint.z > 0.01f && screenPoint.x > -5f && screenPoint.y > -5f && screenPoint.x < (float)Screen.width && screenPoint.y < (float)Screen.height;
        }
        public static Vector3 GetLimbPosition(Transform target, string objName)
        {
            var componentsInChildren = target.transform.GetComponentsInChildren<Transform>();
            var result = Vector3.zero;

            if (componentsInChildren == null) return result;

            foreach (var transform in componentsInChildren)
            {
                if (transform.name.Trim() != objName) continue;
                if (objName == "Skull")
                    result = transform.position + new Vector3(0f, 0.4f, 0f);
                else
                    result = transform.position;
                break;
            }

            return result;
        }
        public static Vector3 WorldPointToScreenPoint(Vector3 worldPoint)
        {
            Vector3 vector = Globals.MainCamera.WorldToScreenPoint(worldPoint);
            vector.y = (float)Screen.height - vector.y;
            return vector;
        }
        void Start()
        {
            
            Helpers.ConfigHelper.CreateEnvironment();
            Helpers.ColourHelper.AddColours();
            Esp.Drawing drawing = new Esp.Drawing();
            drawing.Start();
            MainCamera = Camera.main;
            ControlsSettings.bindings[(int)ControlsSettings.SCREENSHOT].key = KeyCode.Delete; // change insert screenshots
        }
    }
}
