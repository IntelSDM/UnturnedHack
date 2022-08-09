using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hag.Renderer;
using SharpDX;
using SharpDX.Direct2D1;
using UnityEngine;
using System.IO;
using Hag.Esp_Objects;
using SDG.Unturned;
namespace Hag.Esp
{

    class Drawing
    {
        #region import
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        #endregion
        public static OverlayWindow Overlay;
        private static IntPtr GameWindow;
        Direct2DRenderer Renderer;
        private static int DrawTime;
        public const string WINDOW_NAME = "Unturned";
        Rect windowSize = new Rect();
        public static Direct2DColor CrosshairColor;
        Direct2DBrush whiteSolid;
        Direct2DFont infoFont;
        Direct2DFont Tahoma;
        Direct2DFont Tahoma2;
        Direct2DFont Tahoma3;
        Direct2DFont Tahoma4;

        Direct2DFont ZombieFont;

        public void Start()
        {

            GameWindow = FindWindow(null, WINDOW_NAME);

            GetWindowRect(GameWindow, ref windowSize);
            OverlayCreationOptions overlayOptions = new OverlayCreationOptions()
            {
                BypassTopmost = true,
                Height = windowSize.Bottom - windowSize.Top,
                Width = windowSize.Right - windowSize.Left,
                WindowTitle = HelperMethods.GenerateRandomString(5, 11),
                X = windowSize.Left,
                Y = windowSize.Top
            };

            StickyOverlayWindow overlay = new StickyOverlayWindow(GameWindow, overlayOptions);
            Overlay = overlay;
            var rendererOptions = new Direct2DRendererOptions()
            {
                AntiAliasing = true,
                Hwnd = overlay.WindowHandle,
                MeasureFps = true,
                VSync = false

            };

            Renderer = new Direct2DRenderer(rendererOptions);
            whiteSolid = Renderer.CreateBrush(255, 255, 255, 255);
            infoFont = Renderer.CreateFont("Consolas", 11);
            Tahoma = Renderer.CreateFont("Tahoma", 10);
            Tahoma2 = Renderer.CreateFont("Tahoma", 9);
            Tahoma3 = Renderer.CreateFont("Tahoma", 12);
            Tahoma4 = Renderer.CreateFont("Tahoma", 10);
            ZombieFont = Renderer.CreateFont("Tahoma", 9);
            new Thread(delegate ()
            {

                Render();
            }).Start();


        }
        void DrawZombie()
        {
            try
            {
                if (Globals.LocalPlayer == null || !Provider.isConnected || !Globals.Config.Zombie.Enable)
                    return;
                foreach (BaseZombie basezombie in Globals.ZombieList)
                {
                    if (!Globals.IsScreenPointVisible(basezombie.W2S) || !basezombie.Alive)
                        continue;
                    if (basezombie.Distance > Globals.Config.Zombie.MaxDistance)
                        continue;
                    if ((Globals.Config.Zombie.OnlyDrawVisible && !basezombie.Visible))
                        continue;
                    string tag = Globals.Config.Zombie.Tag ? basezombie.Tag : "";
                    string distance = Globals.Config.Zombie.Distance ? $"({basezombie.Distance}m)" : "";
                    Renderer.DrawTextCentered($"{tag}{distance}", basezombie.W2S.x, basezombie.W2S.y, ZombieFont, new Direct2DColor(basezombie.Colour.r, basezombie.Colour.g, basezombie.Colour.b, basezombie.Colour.a));
                    if (Globals.Config.Zombie.Box3D && !Globals.Config.Zombie.FillBox)
                        Renderer.DrawBox3DOutline(basezombie.BoundPoints, new Direct2DColor(basezombie.BoxColour.r, basezombie.BoxColour.g, basezombie.BoxColour.b, basezombie.BoxColour.a));
                    if (Globals.Config.Zombie.Box3D && Globals.Config.Zombie.FillBox)
                        Renderer.DrawBox3D(basezombie.BoundPoints, new Direct2DColor(basezombie.FilledBoxColour.r, basezombie.FilledBoxColour.g, basezombie.FilledBoxColour.b, basezombie.FilledBoxColour.a), new Direct2DColor(basezombie.BoxColour.r, basezombie.BoxColour.g, basezombie.BoxColour.b, basezombie.BoxColour.a));
                    /*         for (int i = 0; i < basezombie.pts.Length; i++)
                             { 
                             Renderer.DrawTextCentered(i.ToString(),basezombie.pts[i].x, basezombie.pts[i].y, ZombieFont, new Direct2DColor(basezombie.Colour.r, basezombie.Colour.g, basezombie.Colour.b, basezombie.Colour.a));
                             }*/
                    float height = basezombie.BoundPoints[0].y - basezombie.BoundPoints[2].y;
                    float width1 = UnityEngine.Vector3.Distance(basezombie.BoundPoints[2], basezombie.BoundPoints[7]);
                    float width2 = UnityEngine.Vector3.Distance(basezombie.BoundPoints[3], basezombie.BoundPoints[6]);
                    float width = width1 > width2 ? width1 : width2;
                    float halfwidth = width / 2;

                    if (Globals.Config.Zombie.Box && Globals.Config.Zombie.FillBox)
                    {
                        Renderer.FillRectangle((basezombie.HeadW2S.x - halfwidth + 1), basezombie.W2S.y + 1, width + 1, height - 1, new Direct2DColor(basezombie.FilledBoxColour.r, basezombie.FilledBoxColour.g, basezombie.FilledBoxColour.b, basezombie.FilledBoxColour.a));
                        Renderer.DrawRectangle(basezombie.HeadW2S.x - halfwidth, basezombie.W2S.y, width, height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                        Renderer.DrawRectangle(basezombie.HeadW2S.x - halfwidth, basezombie.W2S.y, width, height, 1f, new Direct2DColor(basezombie.BoxColour.r, basezombie.BoxColour.g, basezombie.BoxColour.b, basezombie.BoxColour.a));
                    }
                    if (Globals.Config.Zombie.Box && !Globals.Config.Zombie.FillBox)
                    {
                        Renderer.DrawRectangle(basezombie.HeadW2S.x - halfwidth, basezombie.W2S.y, width, height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                        Renderer.DrawRectangle(basezombie.HeadW2S.x - halfwidth, basezombie.W2S.y, width, height, 1f, new Direct2DColor(basezombie.BoxColour.r, basezombie.BoxColour.g, basezombie.BoxColour.b, basezombie.BoxColour.a));
                    }

                }
            }
            catch { }
        }
        private void Render()
        {
            while (true)
            {
                try
                {

                    #region Start
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    try
                    {
                        Renderer.BeginScene();
                        Renderer.ClearScene();
                    }
                    catch { }

                    #endregion
                    Renderer.DrawCrosshair(CrosshairStyle.Gap, Screen.width / 2, Screen.height / 2, 6, 1, new Direct2DColor(255, 0, 0, 255));
                    DrawZombie();

                    #region End
                    Menu.RenderMenu.Render(Renderer);
                    try
                    {
                        Renderer.EndScene();
                    }
                    catch { }
                    timer.Stop();
                    #endregion
                }
                catch(Exception ex) { File.WriteAllText("DrawException.txt", ex.ToString()); }

                }
        }
        }
}
