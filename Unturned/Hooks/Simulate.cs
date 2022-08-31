using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hag.Helpers;
using UnityEngine;
namespace Hag.Hooks
{
    class Simulate : MonoBehaviour
    {
        private static DumbHook MovementSimulate;
        void Start()
        {
      //      MovementSimulate = new DumbHook();
        //    MovementSimulate.Init(typeof(SDG.Unturned.PlayerMovement).GetMethod("simulate", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance), typeof(Simulate).GetMethod("simulate", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
          //  MovementSimulate.Hook();
        }
        public void simulate(uint simulation, int recov, int input_x, int input_y, float look_x, float look_y, bool inputJump, bool inputSprint, float deltaTime)
        {
            return;
        }
    }
}
