﻿using System;
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
using System.Reflection;

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
        Direct2DFont PlayerFont;
        Direct2DFont VehicleFont;
        Direct2DFont ItemFont;
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
            PlayerFont = Renderer.CreateFont("Tahoma", 10);
            VehicleFont = Renderer.CreateFont("Tahoma", 9);
            ItemFont = Renderer.CreateFont("Tahoma", 8);
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
                    if (basezombie.Entity == null || basezombie == null)
                        continue;
                    if (!Globals.IsScreenPointVisible(basezombie.W2S) || !basezombie.Alive)
                        continue;
                    if (basezombie.Distance > Globals.Config.Zombie.MaxDistance)
                        continue;
                    if ((Globals.Config.Zombie.OnlyDrawVisible && !basezombie.Visible))
                        continue;
                    string tag = Globals.Config.Zombie.Tag ? basezombie.Tag : "";
                    string distance = Globals.Config.Zombie.Distance ? $"({basezombie.Distance}m)" : "";
                  
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
                    if (Globals.Config.Zombie.Skeleton)
                    {
                     
                        
                        Renderer.DrawLine(basezombie.BonePosition[9].x, basezombie.BonePosition[9].y, basezombie.BonePosition[10].x, basezombie.BonePosition[10].y, 1, new Direct2DColor(basezombie.BoneColour[9].r, basezombie.BoneColour[9].g, basezombie.BoneColour[9].b, basezombie.BoneColour[9].a)); // kinda above the "Spine", basically the real spine
                        Renderer.DrawLine(basezombie.BonePosition[10].x, basezombie.BonePosition[10].y, basezombie.BonePosition[8].x, basezombie.BonePosition[8].y, 1, new Direct2DColor(basezombie.BoneColour[10].r, basezombie.BoneColour[10].g, basezombie.BoneColour[10].b, basezombie.BoneColour[10].a));

                        Renderer.DrawLine(basezombie.BonePosition[10].x, basezombie.BonePosition[10].y, basezombie.BonePosition[4].x, basezombie.BonePosition[4].y, 1, new Direct2DColor(basezombie.BoneColour[4].r, basezombie.BoneColour[4].g, basezombie.BoneColour[4].b, basezombie.BoneColour[4].a));
                        Renderer.DrawLine(basezombie.BonePosition[10].x, basezombie.BonePosition[10].y, basezombie.BonePosition[5].x, basezombie.BonePosition[5].y, 1, new Direct2DColor(basezombie.BoneColour[5].r, basezombie.BoneColour[5].g, basezombie.BoneColour[5].b, basezombie.BoneColour[5].a));
                        Renderer.DrawLine(basezombie.BonePosition[5].x, basezombie.BonePosition[5].y, basezombie.BonePosition[7].x, basezombie.BonePosition[7].y, 1, new Direct2DColor(basezombie.BoneColour[7].r, basezombie.BoneColour[7].g, basezombie.BoneColour[7].b, basezombie.BoneColour[7].a));
                        Renderer.DrawLine(basezombie.BonePosition[4].x, basezombie.BonePosition[4].y, basezombie.BonePosition[6].x, basezombie.BonePosition[6].y, 1, new Direct2DColor(basezombie.BoneColour[6].r, basezombie.BoneColour[6].g, basezombie.BoneColour[6].b, basezombie.BoneColour[6].a));


                        Renderer.DrawLine(basezombie.BonePosition[8].x, basezombie.BonePosition[8].y, basezombie.BonePosition[3].x, basezombie.BonePosition[3].y, 1, new Direct2DColor(basezombie.BoneColour[3].r, basezombie.BoneColour[3].g, basezombie.BoneColour[3].b, basezombie.BoneColour[3].a));
                        Renderer.DrawLine(basezombie.BonePosition[3].x, basezombie.BonePosition[3].y, basezombie.BonePosition[1].x, basezombie.BonePosition[1].y, 1, new Direct2DColor(basezombie.BoneColour[1].r, basezombie.BoneColour[1].g, basezombie.BoneColour[1].b, basezombie.BoneColour[1].a));
                        Renderer.DrawLine(basezombie.BonePosition[8].x, basezombie.BonePosition[8].y, basezombie.BonePosition[2].x, basezombie.BonePosition[2].y, 1, new Direct2DColor(basezombie.BoneColour[2].r, basezombie.BoneColour[2].g, basezombie.BoneColour[2].b, basezombie.BoneColour[2].a));
                        Renderer.DrawLine(basezombie.BonePosition[2].x, basezombie.BonePosition[2].y, basezombie.BonePosition[0].x, basezombie.BonePosition[0].y, 1, new Direct2DColor(basezombie.BoneColour[0].r, basezombie.BoneColour[0].g, basezombie.BoneColour[0].b, basezombie.BoneColour[0].a));

                       Renderer.DrawCircle(basezombie.HeadW2S.x, basezombie.HeadW2S.y, basezombie.Distance > 10 ? 30 / (basezombie.Distance /10) : 10, 1, new Direct2DColor(basezombie.BoneColour[9].r, basezombie.BoneColour[9].g, basezombie.BoneColour[9].b, basezombie.BoneColour[9].a));
                  
                    }
                    Renderer.DrawTextCentered($"{tag}{distance}", basezombie.W2S.x, basezombie.W2S.y, ZombieFont, new Direct2DColor(basezombie.Colour.r, basezombie.Colour.g, basezombie.Colour.b, basezombie.Colour.a));
                }
            }
            catch { }
        }
        void DrawItems()
        {
            try
            {

                if (Globals.LocalPlayer == null || !Provider.isConnected || !Globals.Config.Item.Enable)
                    return;
                foreach (BaseItem baseitem in Globals.ItemList)
                {
                    if (baseitem == null || baseitem.Entity == null)
                        continue;
                    if (!Globals.IsScreenPointVisible(baseitem.W2S))
                        continue;
                 
                    if (Globals.Config.Item.Filters)
                    {
                        if (baseitem.Entity.asset is ItemGunAsset && Globals.Config.GunFilter.Enable && baseitem.Distance < Globals.Config.GunFilter.MaxDistance)
                        {
                            string name = Globals.Config.GunFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.GunFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemMeleeAsset && Globals.Config.MeleeFilter.Enable && baseitem.Distance < Globals.Config.MeleeFilter.MaxDistance)
                        {
                            string name = Globals.Config.MeleeFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.MeleeFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemMedicalAsset && Globals.Config.MedicalFilter.Enable && baseitem.Distance < Globals.Config.MedicalFilter.MaxDistance)
                        {
                            string name = Globals.Config.MedicalFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.MedicalFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if ((baseitem.Entity.asset is ItemFoodAsset || baseitem.Entity.asset is ItemWaterAsset) && Globals.Config.FoodAndWaterFilter.Enable && baseitem.Distance < Globals.Config.FoodAndWaterFilter.MaxDistance)
                        {
                            string name = Globals.Config.FoodAndWaterFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.FoodAndWaterFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemClothingAsset && Globals.Config.ClothingFilter.Enable && baseitem.Distance < Globals.Config.ClothingFilter.MaxDistance)
                        {
                            string name = Globals.Config.ClothingFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.ClothingFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemBackpackAsset && Globals.Config.BackpackFilter.Enable && baseitem.Distance < Globals.Config.BackpackFilter.MaxDistance)
                        {
                            string name = Globals.Config.BackpackFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.BackpackFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemCaliberAsset && Globals.Config.AmmoFilter.Enable && baseitem.Distance < Globals.Config.AmmoFilter.MaxDistance)
                        {
                            string name = Globals.Config.AmmoFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.AmmoFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemMagazineAsset && Globals.Config.MagazineFilter.Enable && baseitem.Distance < Globals.Config.MagazineFilter.MaxDistance)
                        {
                            string name = Globals.Config.MagazineFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.MagazineFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemFuelAsset && Globals.Config.FuelFilter.Enable && baseitem.Distance < Globals.Config.FuelFilter.MaxDistance)
                        {
                            string name = Globals.Config.FuelFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.FuelFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemThrowableAsset && Globals.Config.ThrowableFilter.Enable && baseitem.Distance < Globals.Config.ThrowableFilter.MaxDistance)
                        {
                            string name = Globals.Config.ThrowableFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.ThrowableFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemOpticAsset && Globals.Config.OpticFilter.Enable && baseitem.Distance < Globals.Config.OpticFilter.MaxDistance)
                        {
                            string name = Globals.Config.OpticFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.OpticFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemBarrelAsset && Globals.Config.BarrelFilter.Enable && baseitem.Distance < Globals.Config.BarrelFilter.MaxDistance)
                        {
                            string name = Globals.Config.BarrelFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.BarrelFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if ((baseitem.Entity.asset is ItemFarmAsset || baseitem.Entity.asset is ItemFisherAsset) && Globals.Config.FarmFilter.Enable && baseitem.Distance < Globals.Config.FarmFilter.MaxDistance)
                        {
                            string name = Globals.Config.FarmFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.FarmFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if ((baseitem.Entity.asset is ItemDetonatorAsset || baseitem.Entity.asset is ItemChargeAsset)  && Globals.Config.ExplosivesFilter.Enable && baseitem.Distance < Globals.Config.ExplosivesFilter.MaxDistance)
                        {
                            string name = Globals.Config.ExplosivesFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.ExplosivesFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                        if (baseitem.Entity.asset is ItemSupplyAsset && Globals.Config.SupplyFilter.Enable && baseitem.Distance < Globals.Config.SupplyFilter.MaxDistance)
                        {
                            string name = Globals.Config.SupplyFilter.Name ? baseitem.Name : "";
                            string distance = Globals.Config.SupplyFilter.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                            Renderer.DrawTextCentered($"{name}{distance}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                        }
                    }
                    else
                    {
                        if (baseitem.Distance > Globals.Config.Item.MaxDistance)
                            continue;
                        string name = Globals.Config.Item.Name ? baseitem.Name : "";
                        string distance = Globals.Config.Item.Distance ? $"({baseitem.Distance.ToString()}m)" : "";
                        Renderer.DrawTextCentered($"{name}{distance}{baseitem.Entity.asset.ToString()}", baseitem.W2S.x, baseitem.W2S.y, ItemFont, new Direct2DColor(baseitem.Colour.r, baseitem.Colour.g, baseitem.Colour.b, baseitem.Colour.a));
                    }
                    }
                }
            catch { }

        }
        void DrawVehicles()
        {
            try
            {

                if (Globals.LocalPlayer == null || !Provider.isConnected || !Globals.Config.Vehicle.Enabled)
                    return;
                foreach (BaseVehicle basevehicle in Globals.VehicleList)
                { 
                  if (!Globals.IsScreenPointVisible(basevehicle.W2S) || basevehicle.Entity.isDead)
                        continue;

                    string name = Globals.Config.Vehicle.Name ? basevehicle.Name : "";
                    string distance = Globals.Config.Vehicle.Distance ? $"({basevehicle.Distance.ToString()}m)" : "";
                    string locked = basevehicle.Locked ? "[Locked]" : "[Unlocked]";
                    string lockedstr = Globals.Config.Vehicle.LockedStatus ? locked : "";
                    if (basevehicle.Distance > Globals.Config.Vehicle.MaxDistance)
                        continue;
                    if (basevehicle.OwnedByYou && Globals.Config.Vehicle.DrawOwnVehicles)
                        Renderer.DrawTextCentered($"{name}{lockedstr}{distance}", basevehicle.W2S.x, basevehicle.W2S.y, VehicleFont, new Direct2DColor(basevehicle.Colour.r, basevehicle.Colour.g, basevehicle.Colour.b, basevehicle.Colour.a));
                    
                    if (basevehicle.OwnedByYou)
                        continue;
                   // if ((!basevehicle.OwnedByYou && Globals.Config.Vehicle.DrawOwnVehicles && !basevehicle.Locked))
                     //   continue;
                    //   if ((Globals.Config.Vehicle.DrawUnlocked && basevehicle.Locked && (!basevehicle.OwnedByYou && Globals.Config.Vehicle.DrawOwnVehicles))) // this line is meant to check if it is locked and only draw unlocked but ignore owned vehicles if drawownvehicles is on
                    //     continue;
                    if((Globals.Config.Vehicle.DrawUnlocked && !basevehicle.Locked) || (Globals.Config.Vehicle.DrawLocked && basevehicle.Locked))
                    Renderer.DrawTextCentered($"{name}{lockedstr}{distance}", basevehicle.W2S.x, basevehicle.W2S.y, VehicleFont, new Direct2DColor(basevehicle.Colour.r, basevehicle.Colour.g, basevehicle.Colour.b, basevehicle.Colour.a));
                }
            }
            catch { }
            }
        void DrawPlayer()
        {
            try
            {
                if (Globals.LocalPlayer == null || !Provider.isConnected || !Globals.Config.Player.Enable)
                    return;
                //    Player.player.stance.stance = EPlayerStance.DRIVING;
                //     Player.player.movement.pluginSpeedMultiplier = 100;
               // Player.player.movement.pendingLaunchVelocity = new Vector3(100,100,100);

                foreach (BasePlayer baseplayer in Globals.PlayerList)
                {
                   // baseplayer.Entity.movement.controller.height = 100;
               //     baseplayer.Entity.movement.controller.center = new Vector3(0, 100, 0);
                   
                    if (baseplayer.Entity == null || baseplayer == null)
                        continue;
                    if (!Globals.IsScreenPointVisible(baseplayer.W2S) || !baseplayer.Alive)
                        continue;
                    #region Normal Player
                    if (!baseplayer.Friendly)
                    {


                        if (baseplayer.Distance > Globals.Config.Player.MaxDistance)
                            continue;
                        if ((Globals.Config.Player.OnlyDrawVisible && !baseplayer.Visible))
                            continue;
                        string tag = Globals.Config.Player.Name ? baseplayer.Name : "";
                        string distance = Globals.Config.Player.Distance ? $"({baseplayer.Distance}m)" : "";
                        string weapon = Globals.Config.Player.Weapon ? $"\n{baseplayer.Weapon}" : "";
                        if (Globals.Config.Player.Box3D && !Globals.Config.Player.FillBox)
                            Renderer.DrawBox3DOutline(baseplayer.BoundPoints, new Direct2DColor(baseplayer.BoxColour.r, baseplayer.BoxColour.g, baseplayer.BoxColour.b, baseplayer.BoxColour.a));
                        if (Globals.Config.Player.Box3D && Globals.Config.Player.FillBox)
                            Renderer.DrawBox3D(baseplayer.BoundPoints, new Direct2DColor(baseplayer.FilledBoxColour.r, baseplayer.FilledBoxColour.g, baseplayer.FilledBoxColour.b, baseplayer.FilledBoxColour.a), new Direct2DColor(baseplayer.BoxColour.r, baseplayer.BoxColour.g, baseplayer.BoxColour.b, baseplayer.BoxColour.a));

                        float height = baseplayer.BoundPoints[0].y - baseplayer.BoundPoints[2].y;
                        float width1 = UnityEngine.Vector3.Distance(baseplayer.BoundPoints[2], baseplayer.BoundPoints[7]);
                        float width2 = UnityEngine.Vector3.Distance(baseplayer.BoundPoints[3], baseplayer.BoundPoints[6]);
                        float width = width1 > width2 ? width1 : width2;
                        float halfwidth = width / 2;

                        if (Globals.Config.Player.Box && Globals.Config.Player.FillBox)
                        {
                            Renderer.FillRectangle((baseplayer.HeadW2S.x - halfwidth + 1), baseplayer.W2S.y + 1, width + 1, height - 1, new Direct2DColor(baseplayer.FilledBoxColour.r, baseplayer.FilledBoxColour.g, baseplayer.FilledBoxColour.b, baseplayer.FilledBoxColour.a));
                            Renderer.DrawRectangle(baseplayer.HeadW2S.x - halfwidth, baseplayer.W2S.y, width, height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                            Renderer.DrawRectangle(baseplayer.HeadW2S.x - halfwidth, baseplayer.W2S.y, width, height, 1f, new Direct2DColor(baseplayer.BoxColour.r, baseplayer.BoxColour.g, baseplayer.BoxColour.b, baseplayer.BoxColour.a));
                        }
                        if (Globals.Config.Player.Box && !Globals.Config.Player.FillBox)
                        {
                            Renderer.DrawRectangle(baseplayer.HeadW2S.x - halfwidth, baseplayer.W2S.y, width, height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                            Renderer.DrawRectangle(baseplayer.HeadW2S.x - halfwidth, baseplayer.W2S.y, width, height, 1f, new Direct2DColor(baseplayer.BoxColour.r, baseplayer.BoxColour.g, baseplayer.BoxColour.b, baseplayer.BoxColour.a));
                        }
                        if (Globals.Config.Player.Skeleton)
                        {
                            /*  for (int i = 0; i < baseplayer.BonePosition.Length; i++)
                              {
                                  //        Renderer.DrawTextCentered($"{ i.ToString()}", baseplayer.BonePosition[i].x, baseplayer.BonePosition[i].y, ZombieFont, new Direct2DColor(baseplayer.Colour.r, baseplayer.Colour.g, baseplayer.Colour.b, baseplayer.Colour.a));
                              }
                            */
                            Renderer.DrawLine(baseplayer.BonePosition[9].x, baseplayer.BonePosition[9].y, baseplayer.BonePosition[10].x, baseplayer.BonePosition[10].y, 1, new Direct2DColor(baseplayer.BoneColour[9].r, baseplayer.BoneColour[9].g, baseplayer.BoneColour[9].b, baseplayer.BoneColour[9].a)); // kinda above the "Spine", basically the real spine
                            Renderer.DrawLine(baseplayer.BonePosition[10].x, baseplayer.BonePosition[10].y, baseplayer.BonePosition[8].x, baseplayer.BonePosition[8].y, 1, new Direct2DColor(baseplayer.BoneColour[10].r, baseplayer.BoneColour[10].g, baseplayer.BoneColour[10].b, baseplayer.BoneColour[10].a));

                            Renderer.DrawLine(baseplayer.BonePosition[10].x, baseplayer.BonePosition[10].y, baseplayer.BonePosition[4].x, baseplayer.BonePosition[4].y, 1, new Direct2DColor(baseplayer.BoneColour[4].r, baseplayer.BoneColour[4].g, baseplayer.BoneColour[4].b, baseplayer.BoneColour[4].a));
                            Renderer.DrawLine(baseplayer.BonePosition[10].x, baseplayer.BonePosition[10].y, baseplayer.BonePosition[5].x, baseplayer.BonePosition[5].y, 1, new Direct2DColor(baseplayer.BoneColour[5].r, baseplayer.BoneColour[5].g, baseplayer.BoneColour[5].b, baseplayer.BoneColour[5].a));
                            Renderer.DrawLine(baseplayer.BonePosition[5].x, baseplayer.BonePosition[5].y, baseplayer.BonePosition[7].x, baseplayer.BonePosition[7].y, 1, new Direct2DColor(baseplayer.BoneColour[7].r, baseplayer.BoneColour[7].g, baseplayer.BoneColour[7].b, baseplayer.BoneColour[7].a));
                            Renderer.DrawLine(baseplayer.BonePosition[4].x, baseplayer.BonePosition[4].y, baseplayer.BonePosition[6].x, baseplayer.BonePosition[6].y, 1, new Direct2DColor(baseplayer.BoneColour[6].r, baseplayer.BoneColour[6].g, baseplayer.BoneColour[6].b, baseplayer.BoneColour[6].a));


                            Renderer.DrawLine(baseplayer.BonePosition[8].x, baseplayer.BonePosition[8].y, baseplayer.BonePosition[3].x, baseplayer.BonePosition[3].y, 1, new Direct2DColor(baseplayer.BoneColour[3].r, baseplayer.BoneColour[3].g, baseplayer.BoneColour[3].b, baseplayer.BoneColour[3].a));
                            Renderer.DrawLine(baseplayer.BonePosition[3].x, baseplayer.BonePosition[3].y, baseplayer.BonePosition[1].x, baseplayer.BonePosition[1].y, 1, new Direct2DColor(baseplayer.BoneColour[1].r, baseplayer.BoneColour[1].g, baseplayer.BoneColour[1].b, baseplayer.BoneColour[1].a));
                            Renderer.DrawLine(baseplayer.BonePosition[8].x, baseplayer.BonePosition[8].y, baseplayer.BonePosition[2].x, baseplayer.BonePosition[2].y, 1, new Direct2DColor(baseplayer.BoneColour[2].r, baseplayer.BoneColour[2].g, baseplayer.BoneColour[2].b, baseplayer.BoneColour[2].a));
                            Renderer.DrawLine(baseplayer.BonePosition[2].x, baseplayer.BonePosition[2].y, baseplayer.BonePosition[0].x, baseplayer.BonePosition[0].y, 1, new Direct2DColor(baseplayer.BoneColour[0].r, baseplayer.BoneColour[0].g, baseplayer.BoneColour[0].b, baseplayer.BoneColour[0].a));

                            Renderer.DrawCircle(baseplayer.HeadW2S.x, baseplayer.HeadW2S.y, baseplayer.Distance > 10 ? 30 / (baseplayer.Distance / 10) : 10, 1, new Direct2DColor(baseplayer.BoneColour[9].r, baseplayer.BoneColour[9].g, baseplayer.BoneColour[9].b, baseplayer.BoneColour[9].a));

                        }
                        Renderer.DrawTextCentered($"{tag}{distance}{weapon}{baseplayer.Velocity}", baseplayer.W2S.x, baseplayer.W2S.y, PlayerFont, new Direct2DColor(baseplayer.Colour.r, baseplayer.Colour.g, baseplayer.Colour.b, baseplayer.Colour.a));
                    }
                    #endregion
                    #region Friendly Player
                    else
                    {
                        if (baseplayer.Distance > Globals.Config.FriendlyPlayer.MaxDistance)
                            continue;
                        if ((Globals.Config.FriendlyPlayer.OnlyDrawVisible && !baseplayer.Visible))
                            continue;
                        string tag = Globals.Config.FriendlyPlayer.Name ? baseplayer.Name : "";
                        string distance = Globals.Config.FriendlyPlayer.Distance ? $"({baseplayer.Distance}m)" : "";
                        string weapon = Globals.Config.FriendlyPlayer.Weapon ? $"\n{baseplayer.Weapon}" : "";
                        if (Globals.Config.FriendlyPlayer.Box3D && !Globals.Config.FriendlyPlayer.FillBox)
                            Renderer.DrawBox3DOutline(baseplayer.BoundPoints, new Direct2DColor(baseplayer.BoxColour.r, baseplayer.BoxColour.g, baseplayer.BoxColour.b, baseplayer.BoxColour.a));
                        if (Globals.Config.FriendlyPlayer.Box3D && Globals.Config.FriendlyPlayer.FillBox)
                            Renderer.DrawBox3D(baseplayer.BoundPoints, new Direct2DColor(baseplayer.FilledBoxColour.r, baseplayer.FilledBoxColour.g, baseplayer.FilledBoxColour.b, baseplayer.FilledBoxColour.a), new Direct2DColor(baseplayer.BoxColour.r, baseplayer.BoxColour.g, baseplayer.BoxColour.b, baseplayer.BoxColour.a));

                        float height = baseplayer.BoundPoints[0].y - baseplayer.BoundPoints[2].y;
                        float width1 = UnityEngine.Vector3.Distance(baseplayer.BoundPoints[2], baseplayer.BoundPoints[7]);
                        float width2 = UnityEngine.Vector3.Distance(baseplayer.BoundPoints[3], baseplayer.BoundPoints[6]);
                        float width = width1 > width2 ? width1 : width2;
                        float halfwidth = width / 2;

                        if (Globals.Config.FriendlyPlayer.Box && Globals.Config.FriendlyPlayer.FillBox)
                        {
                            Renderer.FillRectangle((baseplayer.HeadW2S.x - halfwidth + 1), baseplayer.W2S.y + 1, width + 1, height - 1, new Direct2DColor(baseplayer.FilledBoxColour.r, baseplayer.FilledBoxColour.g, baseplayer.FilledBoxColour.b, baseplayer.FilledBoxColour.a));
                            Renderer.DrawRectangle(baseplayer.HeadW2S.x - halfwidth, baseplayer.W2S.y, width, height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                            Renderer.DrawRectangle(baseplayer.HeadW2S.x - halfwidth, baseplayer.W2S.y, width, height, 1f, new Direct2DColor(baseplayer.BoxColour.r, baseplayer.BoxColour.g, baseplayer.BoxColour.b, baseplayer.BoxColour.a));
                        }
                        if (Globals.Config.FriendlyPlayer.Box && !Globals.Config.FriendlyPlayer.FillBox)
                        {
                            Renderer.DrawRectangle(baseplayer.HeadW2S.x - halfwidth, baseplayer.W2S.y, width, height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                            Renderer.DrawRectangle(baseplayer.HeadW2S.x - halfwidth, baseplayer.W2S.y, width, height, 1f, new Direct2DColor(baseplayer.BoxColour.r, baseplayer.BoxColour.g, baseplayer.BoxColour.b, baseplayer.BoxColour.a));
                        }
                        if (Globals.Config.FriendlyPlayer.Skeleton)
                        {
                            /*  for (int i = 0; i < baseplayer.BonePosition.Length; i++)
                              {
                                  //        Renderer.DrawTextCentered($"{ i.ToString()}", baseplayer.BonePosition[i].x, baseplayer.BonePosition[i].y, ZombieFont, new Direct2DColor(baseplayer.Colour.r, baseplayer.Colour.g, baseplayer.Colour.b, baseplayer.Colour.a));
                              }
                            */
                            Renderer.DrawLine(baseplayer.BonePosition[9].x, baseplayer.BonePosition[9].y, baseplayer.BonePosition[10].x, baseplayer.BonePosition[10].y, 1, new Direct2DColor(baseplayer.BoneColour[9].r, baseplayer.BoneColour[9].g, baseplayer.BoneColour[9].b, baseplayer.BoneColour[9].a)); // kinda above the "Spine", basically the real spine
                            Renderer.DrawLine(baseplayer.BonePosition[10].x, baseplayer.BonePosition[10].y, baseplayer.BonePosition[8].x, baseplayer.BonePosition[8].y, 1, new Direct2DColor(baseplayer.BoneColour[10].r, baseplayer.BoneColour[10].g, baseplayer.BoneColour[10].b, baseplayer.BoneColour[10].a));

                            Renderer.DrawLine(baseplayer.BonePosition[10].x, baseplayer.BonePosition[10].y, baseplayer.BonePosition[4].x, baseplayer.BonePosition[4].y, 1, new Direct2DColor(baseplayer.BoneColour[4].r, baseplayer.BoneColour[4].g, baseplayer.BoneColour[4].b, baseplayer.BoneColour[4].a));
                            Renderer.DrawLine(baseplayer.BonePosition[10].x, baseplayer.BonePosition[10].y, baseplayer.BonePosition[5].x, baseplayer.BonePosition[5].y, 1, new Direct2DColor(baseplayer.BoneColour[5].r, baseplayer.BoneColour[5].g, baseplayer.BoneColour[5].b, baseplayer.BoneColour[5].a));
                            Renderer.DrawLine(baseplayer.BonePosition[5].x, baseplayer.BonePosition[5].y, baseplayer.BonePosition[7].x, baseplayer.BonePosition[7].y, 1, new Direct2DColor(baseplayer.BoneColour[7].r, baseplayer.BoneColour[7].g, baseplayer.BoneColour[7].b, baseplayer.BoneColour[7].a));
                            Renderer.DrawLine(baseplayer.BonePosition[4].x, baseplayer.BonePosition[4].y, baseplayer.BonePosition[6].x, baseplayer.BonePosition[6].y, 1, new Direct2DColor(baseplayer.BoneColour[6].r, baseplayer.BoneColour[6].g, baseplayer.BoneColour[6].b, baseplayer.BoneColour[6].a));


                            Renderer.DrawLine(baseplayer.BonePosition[8].x, baseplayer.BonePosition[8].y, baseplayer.BonePosition[3].x, baseplayer.BonePosition[3].y, 1, new Direct2DColor(baseplayer.BoneColour[3].r, baseplayer.BoneColour[3].g, baseplayer.BoneColour[3].b, baseplayer.BoneColour[3].a));
                            Renderer.DrawLine(baseplayer.BonePosition[3].x, baseplayer.BonePosition[3].y, baseplayer.BonePosition[1].x, baseplayer.BonePosition[1].y, 1, new Direct2DColor(baseplayer.BoneColour[1].r, baseplayer.BoneColour[1].g, baseplayer.BoneColour[1].b, baseplayer.BoneColour[1].a));
                            Renderer.DrawLine(baseplayer.BonePosition[8].x, baseplayer.BonePosition[8].y, baseplayer.BonePosition[2].x, baseplayer.BonePosition[2].y, 1, new Direct2DColor(baseplayer.BoneColour[2].r, baseplayer.BoneColour[2].g, baseplayer.BoneColour[2].b, baseplayer.BoneColour[2].a));
                            Renderer.DrawLine(baseplayer.BonePosition[2].x, baseplayer.BonePosition[2].y, baseplayer.BonePosition[0].x, baseplayer.BonePosition[0].y, 1, new Direct2DColor(baseplayer.BoneColour[0].r, baseplayer.BoneColour[0].g, baseplayer.BoneColour[0].b, baseplayer.BoneColour[0].a));

                            Renderer.DrawCircle(baseplayer.HeadW2S.x, baseplayer.HeadW2S.y, baseplayer.Distance > 10 ? 30 / (baseplayer.Distance / 10) : 10, 1, new Direct2DColor(baseplayer.BoneColour[9].r, baseplayer.BoneColour[9].g, baseplayer.BoneColour[9].b, baseplayer.BoneColour[9].a));

                        }
                        Renderer.DrawTextCentered($"{tag}{distance}{weapon}", baseplayer.W2S.x, baseplayer.W2S.y, PlayerFont, new Direct2DColor(baseplayer.Colour.r, baseplayer.Colour.g, baseplayer.Colour.b, baseplayer.Colour.a));
                    }
                    #endregion
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
                    DrawVehicles();
                    DrawItems();
                    DrawZombie();
                    DrawPlayer();
               
                    Renderer.DrawCrosshair(CrosshairStyle.Gap, Screen.width / 2, Screen.height / 2, 6, 1, new Direct2DColor(255, 0, 0, 255));
                    if(Globals.Config.Aimbot.DrawFov)
                    Renderer.DrawCircle(Screen.width / 2, Screen.height / 2, Globals.Config.Aimbot.Fov, 1, new Direct2DColor(255, 255, 255, 255));
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
