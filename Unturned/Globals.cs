using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag
{
    class Globals : MonoBehaviour
    {
        public static Camera MainCamera;

        public static Hag.Configs.Config Config = new Configs.Config();
        public static bool EndedFrame = true;

        void Start()
        {
            Helpers.ConfigHelper.CreateEnvironment();
            Helpers.ColourHelper.AddColours();
            Esp.Drawing drawing = new Esp.Drawing();
            drawing.Start();
        }
    }
}
