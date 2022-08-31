using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Configs
{
    class EntityAimbot
    {
        public bool LegitAimbotEnabled = true;
        public KeyCode LegitAimbotKey = KeyCode.Mouse3;
        public bool BulletDropPrediction = true;
        public int LegitAimbotBone = 0;
        public bool LegitVisiblityChecks = true;
        public int LegitMaxDistance = 400;
        public bool Smooth = true;
        public int Smoothing = 100;
    }
}
