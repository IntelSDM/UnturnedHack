using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hag.Helpers;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using SDG.NetTransport;

namespace Hag.Hooks
{
    class AddPlayer : MonoBehaviour
    {

        public static  DumbHook AddPlayerHook;
		void Start()
		{
			AddPlayerHook = new DumbHook();
			AddPlayerHook.Init(typeof(SDG.Unturned.Provider).GetMethod("addPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static), typeof(AddPlayer).GetMethod("addPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static));
			AddPlayerHook.Hook();
		}
		internal static object addPlayer(ITransportConnection transportConnection, NetId netId, SteamPlayerID playerID, Vector3 point, byte angle, bool isPro, bool isAdmin, int channel, byte face, byte hair, byte beard, Color skin, Color color, Color markerColor, 
            bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, string[] skinTags, string[] skinDynamicProps, 
            EPlayerSkillset skillset, string language, CSteamID lobbyID, EClientPlatform clientPlatform)
		{
            /*
             if (!Dedicator.IsDedicatedServer && playerID.steamID != Provider.client)
			{
				SteamFriends.SetPlayedWith(playerID.steamID);
			}
             */
            if (playerID.steamID == Provider.client)
            {
                clientPlatform = EClientPlatform.Linux;
                color = new Color32(255, 255, 255, 255);
                skin = new Color32(255, 255, 255, 255);
                markerColor = new Color32(255, 255, 255, 255);
                isPro = true;
            }
                //   point = Vector3.zero;
                AddPlayerHook.Unhook();


            object[] parameters = new object[]
               {
                    transportConnection,
                    netId,
                    playerID,
                    point,
                    angle,
                    isPro,
                    isAdmin,
                    channel,
                    face,
                    hair,
                    beard,
                    skin,
                    color,
                    markerColor,
                    hand,
                    shirtItem,
                    pantsItem,
                    hatItem,
                    backpackItem,

                       vestItem,
                    maskItem,
                    glassesItem,
                    skinItems,
                    skinTags,
                    skinDynamicProps,
                    skillset,
                    language,
                    lobbyID,
                       clientPlatform,
                  

               };
            object result = AddPlayerHook.OriginalMethod.Invoke(null, parameters);

            AddPlayerHook.Hook();
            return result;
        }

	}
}
