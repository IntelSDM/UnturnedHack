using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using UnityEngine;
using SDG.NetTransport;
using SDG.NetPak;
using Hag.Helpers;
namespace Hag.Hooks
{
    class TakeScreenshot : MonoBehaviour
    {
		public static float LastSpy;
		private static readonly ServerInstanceMethod SendScreenshotRelay = ServerInstanceMethod.Get(typeof(Player), "ReceiveScreenshotRelay");
		private static DumbHook OnSpyHook;
		void Start()
		{
			OnSpyHook = new DumbHook();
			OnSpyHook.Init(typeof(Player).GetMethod("ReceiveTakeScreenshot", System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Instance), typeof(TakeScreenshot).GetMethod("ReceiveTakeScreenshot", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
			OnSpyHook.Hook();
		}
		public void ReceiveTakeScreenshot()
		{

			StartCoroutine(takeScreenshot());

		}

		private IEnumerator takeScreenshot()
		{
			if (Time.realtimeSinceStartup - LastSpy < 0.5 || Globals.Spied)
				yield break;

			Globals.Spied = true;
			LastSpy = Time.realtimeSinceStartup;

			yield return new WaitForFixedUpdate();
			yield return new WaitForEndOfFrame();

			Texture2D screenshotRaw =
		   new Texture2D(Screen.width, Screen.height, (TextureFormat)3, false)
		   {
			   name = "Screenshot_Raw",
			   hideFlags = (HideFlags)61
		   };
			if (screenshotRaw != null && (screenshotRaw.width != Screen.width || screenshotRaw.height != Screen.height))
			{
				UnityEngine.Object.DestroyImmediate(screenshotRaw);
				screenshotRaw = null;
			}
			if (screenshotRaw == null)
			{
				screenshotRaw = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
				screenshotRaw.name = "Screenshot_Raw";
				screenshotRaw.hideFlags = HideFlags.HideAndDontSave;
			}
			screenshotRaw.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);

			Texture2D screenshotFinal = new Texture2D(Screen.width, Screen.height, (TextureFormat)3, false)
			{
				name = "Screenshot_Final",
				hideFlags = (HideFlags)61
			};
			if (screenshotFinal == null)
			{
				screenshotFinal = new Texture2D(640, 480, TextureFormat.RGB24, false);
				screenshotFinal.name = "Screenshot_Final";
				screenshotFinal.hideFlags = HideFlags.HideAndDontSave;
			}
			Color[] pixels = screenshotRaw.GetPixels();
			Color[] array = new Color[screenshotFinal.width * screenshotFinal.height];
			float num = (float)screenshotRaw.width / (float)screenshotFinal.width;
			float num2 = (float)screenshotRaw.height / (float)screenshotFinal.height;
			for (int i = 0; i < screenshotFinal.height; i++)
			{
				int num3 = (int)((float)i * num2) * screenshotRaw.width;
				int num4 = i * screenshotFinal.width;
				for (int j = 0; j < screenshotFinal.width; j++)
				{
					int num5 = (int)((float)j * num);
					array[num4 + j] = pixels[num3 + num5];
				}
			}
			screenshotFinal.SetPixels(array);
			byte[] data = screenshotFinal.EncodeToJPG(33);
			if (data.Length < 30000)
			{

					SendScreenshotRelay.Invoke(Player.player.GetNetId(), SDG.NetTransport.ENetReliability.Reliable, delegate (SDG.NetPak.NetPakWriter writer)
					{
						ushort num6 = (ushort)data.Length;
						writer.WriteUInt16(num6);
						writer.WriteBytes(data, (int)num6);
					});
				
			}
			yield return new WaitForFixedUpdate();
			yield return new WaitForEndOfFrame();
			Globals.Spied = false;
		}
	}
}
