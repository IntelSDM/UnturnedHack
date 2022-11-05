using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
namespace Hag.Misc
{
    class World : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Misc());

        }
        IEnumerator Misc()
        {
            for (; ; )
            {
                if(Globals.Config.World.BypassLeaveTimer)
                    Provider.modeConfigData.Gameplay.Timer_Exit = 0;

                yield return new WaitForEndOfFrame();
            }
        }
    }
    }
