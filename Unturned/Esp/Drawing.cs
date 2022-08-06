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
            new Thread(delegate ()
            {

                Render();
            }).Start();


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
                    #region End
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
