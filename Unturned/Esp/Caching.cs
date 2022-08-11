using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using SDG.Unturned;
using Hag.Esp_Objects;
namespace Hag.Esp
{
    class Caching : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(CacheVars());
            StartCoroutine(Zombie());
            try
            {
                StartCoroutine(Players());
            }
            catch (Exception ex) { System.IO.File.WriteAllText("dgdfg.txt", ex.ToString()); }
        }
        IEnumerator CacheVars()
        {
            for (; ; )
            {
                Globals.MainCamera = Camera.main;
                Globals.LocalPlayer = Player.player;
                yield return new WaitForSeconds(3f);
            }
        }
        IEnumerator Zombie()
        {
            for (; ; )
            {
                if(Globals.LocalPlayer == null || !Provider.isConnected)
                yield return new WaitForSeconds(3f);
                if (!Globals.EndedFrame)
                    continue;
                Globals.ZombieList.Clear();
                foreach (Zombie zombie in FindObjectsOfType<Zombie>())
                {
                    if (zombie == null)
                        continue;
                    BaseZombie basezombie = new BaseZombie(zombie);
                    Globals.ZombieList.Add(basezombie);
                }
                yield return new WaitForSeconds(3f);
            }
        }
        IEnumerator Players()
        {
          
                for (; ; )
                {
                    if (Globals.LocalPlayer == null || !Provider.isConnected)
                        yield return new WaitForSeconds(3.5f);
                    if (!Globals.EndedFrame)
                        continue;
                    Globals.PlayerList.Clear();
                    foreach (SteamPlayer player in Provider.clients)
                    {
                        if (player == null)
                            continue;
                    if (player.player == Globals.LocalPlayer)
                        continue;

                    BasePlayer baseplayer = new BasePlayer(player.player,player);
                        Globals.PlayerList.Add(baseplayer);
                    }
                    yield return new WaitForSeconds(3.5f);
                }
         
        }
    }
  
}
