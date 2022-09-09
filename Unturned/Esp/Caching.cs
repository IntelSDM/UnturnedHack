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
            StartCoroutine(Vehicles());
            StartCoroutine(Items());
            try
            {
                StartCoroutine(Players());
            }
            catch (Exception ex) {  }
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
        IEnumerator Items()
        {
            for (; ; )
            {
                Globals.ItemList.Clear();
                foreach (InteractableItem Item in FindObjectsOfType<InteractableItem>())
                {
                    if (Item == null)
                        continue;
                    BaseItem baseitem = new BaseItem(Item);
                    Globals.ItemList.Add(baseitem);
                }

                    yield return new WaitForSeconds(3);
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
            
                yield return new WaitForSeconds(2f);
            }
        }
        IEnumerator Vehicles()
        {
            for (; ; )
            {
                if (Globals.LocalPlayer == null || !Provider.isConnected)
                    yield return new WaitForSeconds(3f);
                if (!Globals.EndedFrame)
                    continue;
                Globals.VehicleList.Clear();
               
                foreach (InteractableVehicle vh in FindObjectsOfType<InteractableVehicle>())
                {
                    //     vh.addPlayer(0, (Steamworks.CSteamID)76561199161032099);// FUCKING TELEPORT INTO LOCKED CARS
                    //      vh.grantTrunkAccess(Globals.LocalPlayer);
                    //       vh.
                    /*          vh.dropTrunkItems(); 
                              foreach(Passenger ph in vh.passengers)
                              {
                               //   for (int i = 0; i < 10; i++)
                                //  {
                                      vh.forceRemoveAllPlayers();
                                      //vh.removePlayer(i,)
                                //  }

                                 // ph.player.playerID

                              }*/ // seems to crash some servers from time to time
                    if (vh == null)
                        continue;
                    BaseVehicle bvh = new BaseVehicle(vh);
                    Globals.VehicleList.Add(bvh);
                }
                yield return new WaitForSeconds(5f);
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
