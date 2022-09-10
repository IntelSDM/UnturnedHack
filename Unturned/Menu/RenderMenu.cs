using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using Hag.Renderer;
using UnityEngine;
using System.Collections;
using Hag.Helpers;
using UnityEngine;

namespace Hag.Menu
{
    class RenderMenu : MonoBehaviour
    {
        void MainMenu()
        {
            Main.Items.Add(Esp);
            Main.Items.Add(Aimbot);
            Main.Items.Add(Misc);
            Main.Items.Add(PlayerList);
            Main.Items.Add(Colours);
            Main.Items.Add(Config);

            Aimbot.Items.Add(ZombieAimbot);
            Aimbot.Items.Add(PlayerAimbot);
            // Initialize Main Menu
            MenuHistory.Add(Main);
            CurrentMenu = Main;
            Selected = Esp;
        }
        void RemovePlayerFromFriendsList(ulong steamid)
        {
            if (!Globals.Config.Friends.FriendsList.ContainsKey(steamid))
                return;
            Globals.Config.Friends.FriendsList.Remove(steamid);
        }
        void AddPlayerToFriendsList(ulong steamid,string name)
        {
            if (Globals.Config.Friends.FriendsList.ContainsKey(steamid))
                return;
            Globals.Config.Friends.FriendsList.Add(steamid,name);
        }
        IEnumerator RefreshLists()
        {
            for (; ; )
            {
                if (ShowGUI)
                {

                    try
                    {
                        FriendsList.Items.Clear();
                        foreach (KeyValuePair<ulong, string> pair in Globals.Config.Friends.FriendsList)
                        {
                            SubMenu menu = new SubMenu(pair.Value, "");
                            menu.Items.Add(new Button("Remove Friend", "Remove Player From Friends List", () => RemovePlayerFromFriendsList(pair.Key)));
                            FriendsList.Items.Add(menu);

                        }
                    }
                    catch { }
                    try
                    {
                        PlayersList.Items.Clear();
                        foreach (Esp_Objects.BasePlayer bp in Globals.PlayerList)
                        {
                            SubMenu menu = new SubMenu($"{bp.Name} | {bp.SteamPlayer.playerID.characterName} | {bp.SteamPlayer.playerID.nickName}", "");
                            menu.Items.Add(new Button("Add Friend", "Add Player To Friends List", () => AddPlayerToFriendsList(bp.SteamPlayer.playerID.steamID.m_SteamID, bp.Name)));
                            PlayersList.Items.Add(menu);
                        }
                    }
                    catch { }
                    try
                    {
                        VehiclesList.Items.Clear();
                        Toggle ignoreowned = new Toggle("Ignore Locked Vehicles", "", ref Globals.Config.Vehicle.IgnoreOwnedVehiclesInList);
                        VehiclesList.Items.Add(ignoreowned);
                        foreach (Esp_Objects.BaseVehicle bv in Globals.VehicleList)
                        {
                            if (bv.Locked && Globals.Config.Vehicle.IgnoreOwnedVehiclesInList)
                                continue;
                            SubMenu menu = new SubMenu($"{bv.Name} | Cords: {bv.Entity.transform.position} | Distance: {bv.Distance} | Locked {bv.Locked} | Driven: {bv.IsDriven}", "");
                            menu.Items.Add(new Button("Teleport To Vehicle", "Buggy But Teleports You To Vehicle", () => bv.TeleportToCar()));
                            menu.Items.Add(new Button("UnTeleport To Vehicle", "Removes You From Teleported Vehicle", () => bv.LeaveCar()));
                            VehiclesList.Items.Add(menu);
                        }
                    }
                    catch { }
                    }
                yield return new WaitForSeconds(2f);
            }
        }
        void Start()
        {

            MainMenu();
           
            Players();
            FriendlyPlayers();
            Friends();
            Zombies();
            ItemEsps();
            VehicleEsps();

            Weapons();
            ColourPicker();

            AimbotGenerals();
            LegitZombieAimbots();
            LegitPlayerAimbots();

            StartCoroutine(KeyControls());
            StartCoroutine(RefreshLists());
        }
        void Friends()
        {
            SubMenu friendoptions = new SubMenu("Friend Options", "Manage How Friends Are Collected");
            Toggle steamfriends = new Toggle("Use Steam Friends", "Uses Friends Added From Steam", ref Globals.Config.Friends.SteamFriends);
            Toggle groupmembers = new Toggle("Use Group Members", "Uses Your Active Group's Members", ref Globals.Config.Friends.GroupAsFriends);
          

            friendoptions.Items.Add(steamfriends);
            friendoptions.Items.Add(groupmembers);
            friendoptions.Items.Add(FriendsList);
            PlayerList.Items.Add(friendoptions);
            PlayerList.Items.Add(PlayersList);
            PlayerList.Items.Add(VehiclesList);
        }
        void Weapons()
        {
            SubMenu weapon = new SubMenu("Weapons", "Weapon Options");
            Toggle norecoil = new Toggle("No Recoil", "Changes Recoil To Set Value", ref Globals.Config.Weapon.NoRecoil);
            FloatSlider recoilx = new FloatSlider("Recoil x Amount", "Your Amount Of Recoil On X Axis", ref Globals.Config.Weapon.RecoilxAmount, 0, 1, 0.05f);
            FloatSlider recoily = new FloatSlider("Recoil y Amount", "Your Amount Of Recoil On Y Axis", ref Globals.Config.Weapon.RecoilyAmount, 0, 1, 0.05f);
            Toggle nospread = new Toggle("No Spread", "Changes Spread To Set Value", ref Globals.Config.Weapon.NoSpread);
            FloatSlider spread1 = new FloatSlider("Spread Aim Amount", "Your Amount Of Recoil On X Axis", ref Globals.Config.Weapon.NoSpreadAim, 0, 1, 0.05f);
            FloatSlider spread2 = new FloatSlider("Spread Crouch Amount", "Your Amount Of Recoil On Y Axis", ref Globals.Config.Weapon.NoSpreadCrouch, 0, 1, 0.05f);
            FloatSlider spread3 = new FloatSlider("Spread Hip Amount", "Your Amount Of Recoil On X Axis", ref Globals.Config.Weapon.NoSpreadHip, 0, 1, 0.05f);
            FloatSlider spread4 = new FloatSlider("Spread Prone Amount", "Your Amount Of Recoil On Y Axis", ref Globals.Config.Weapon.NoSpreadProne, 0, 1, 0.05f);
            FloatSlider spread5 = new FloatSlider("Spread Sprint Amount", "Your Amount Of Recoil On Y Axis", ref Globals.Config.Weapon.NoSpreadSprint, 0, 1, 0.05f);
            weapon.Items.Add(norecoil);
            weapon.Items.Add(recoilx);
            weapon.Items.Add(recoily);
            weapon.Items.Add(nospread);
            weapon.Items.Add(spread1);
            weapon.Items.Add(spread2);
            weapon.Items.Add(spread3);
            weapon.Items.Add(spread4);
            weapon.Items.Add(spread5);
            Misc.Items.Add(weapon);
        }
        void AimbotGenerals()
        {
            SubMenu aimbot = new SubMenu("General", "General Aimbot Options");
            Toggle enable = new Toggle("Draw Fov", "Draws A Circle To Show Aimbot Area", ref Globals.Config.Aimbot.DrawFov);
            IntSlider aimfov = new IntSlider("Fov", "Your Aimbot Activation Area", ref Globals.Config.Aimbot.Fov, 0, 2000, 5);
            aimbot.Items.Add(enable);
            aimbot.Items.Add(aimfov);
            Aimbot.Items.Add(aimbot);
        }
        void LegitZombieAimbots()
        {
            SubMenu aimbot = new SubMenu("Legit Aimbot", "Static Aimbot");
            Toggle enable = new Toggle("Enable", "Turns On Static Aimbot", ref Globals.Config.ZombieAimbot.LegitAimbotEnabled);
            IntSlider distance = new IntSlider("Max Distance", "Aimbot Activation Range", ref Globals.Config.ZombieAimbot.LegitMaxDistance, 0, 2000, 15);
            Toggle enablesmooth = new Toggle("Enable Smoothing", "Turns On Smoothing", ref Globals.Config.ZombieAimbot.Smooth);
            IntSlider smoothing = new IntSlider("Smoothing", "Amount Of Smoothing", ref Globals.Config.ZombieAimbot.Smoothing, 0, 100, 2);
            IntSlider bone = new IntSlider("Target Bone", "0:Head 1:Spine 2:Pelvis 3:Arms 4:Hands 5:Legs 6:Feet 7:Nearest", ref Globals.Config.ZombieAimbot.LegitAimbotBone, 0, 7, 1);
            Toggle vischecks = new Toggle("Visibility Checks", "Only Target Visible Players Based On Target Bone", ref Globals.Config.ZombieAimbot.LegitVisiblityChecks);
            Keybind bind = new Keybind("Aimbot Key", "Key To Lock Onto Players", ref Globals.Config.ZombieAimbot.LegitAimbotKey);
            aimbot.Items.Add(enable);
            aimbot.Items.Add(distance);
            aimbot.Items.Add(bone);
            aimbot.Items.Add(enablesmooth);
            aimbot.Items.Add(smoothing);
            aimbot.Items.Add(vischecks);
            aimbot.Items.Add(bind);
            ZombieAimbot.Items.Add(aimbot);
        }
        void LegitPlayerAimbots()
        {
            SubMenu aimbot = new SubMenu("Legit Aimbot", "Static Aimbot");
            Toggle enable = new Toggle("Enable", "Turns On Static Aimbot", ref Globals.Config.PlayerAimbot.LegitAimbotEnabled);
            IntSlider distance = new IntSlider("Max Distance", "Aimbot Activation Range", ref Globals.Config.PlayerAimbot.LegitMaxDistance, 0, 2000, 15);
            Toggle enablesmooth = new Toggle("Enable Smoothing", "Turns On Smoothing", ref Globals.Config.PlayerAimbot.Smooth);
            IntSlider smoothing = new IntSlider("Smoothing", "Amount Of Smoothing", ref Globals.Config.PlayerAimbot.Smoothing, 0, 100, 2);
            IntSlider bone = new IntSlider("Target Bone", "0:Head 1:Spine 2:Pelvis", ref Globals.Config.PlayerAimbot.LegitAimbotBone, 0, 7, 1);
            Toggle vischecks = new Toggle("Visibility Checks", "Only Target Visible Players Based On Target Bone", ref Globals.Config.PlayerAimbot.LegitVisiblityChecks);
            Keybind bind = new Keybind("Aimbot Key", "Key To Lock Onto Players", ref Globals.Config.PlayerAimbot.LegitAimbotKey);
            aimbot.Items.Add(enable);
            aimbot.Items.Add(distance);
            aimbot.Items.Add(bone);
            aimbot.Items.Add(enablesmooth);
            aimbot.Items.Add(smoothing);
            aimbot.Items.Add(vischecks);
            aimbot.Items.Add(bind);
            PlayerAimbot.Items.Add(aimbot);
        }
        void Zombies()
        {
            SubMenu ZombieEsp = new SubMenu("Zombie ESP", "Draw Zombie Visuals");
            Toggle enable = new Toggle("Enable", "Enable Zombie Esp", ref Globals.Config.Zombie.Enable);
            Toggle name = new Toggle("Name", "Draw Name Of Zombie", ref Globals.Config.Zombie.Tag);
            Toggle distance = new Toggle("Distance", "Draw Distance From Zombie", ref Globals.Config.Zombie.Distance);
            IntSlider maxdistance = new IntSlider("Max Distance", "Maximum Draw Distance Of Zombies", ref Globals.Config.Zombie.MaxDistance, 0, 2000, 25);
            Toggle box3d = new Toggle("3D Box", "Draws Box Around Dimensions Of Zombie", ref Globals.Config.Zombie.Box3D);
            Toggle box2d = new Toggle("2D Box", "Draws Box From Head To Toe Of Zombie", ref Globals.Config.Zombie.Box);
            Toggle fillbox = new Toggle("Fill Box", "Fills The Box", ref Globals.Config.Zombie.FillBox);
            Toggle skeleton = new Toggle("Skeleton", "Draws Bones With Visibility Checks", ref Globals.Config.Zombie.Skeleton);
            Toggle visibleonly = new Toggle("Only Draw On Visible Zombies", "Draws Esp On Visible Zombies", ref Globals.Config.Zombie.OnlyDrawVisible);
            ZombieEsp.Items.Add(enable);
            ZombieEsp.Items.Add(name);
            ZombieEsp.Items.Add(distance);
            ZombieEsp.Items.Add(maxdistance);
            ZombieEsp.Items.Add(box3d);
            ZombieEsp.Items.Add(box2d);
            ZombieEsp.Items.Add(fillbox);
            ZombieEsp.Items.Add(skeleton);
            ZombieEsp.Items.Add(visibleonly);
            Esp.Items.Add(ZombieEsp);

        }
        void Players()
        {
            SubMenu playeresp = new SubMenu("Player ESP", "Draw Player Visuals");
            Toggle enable = new Toggle("Enable", "Enable Player Esp", ref Globals.Config.Player.Enable);
            Toggle name = new Toggle("Name", "Draw Name Of Player", ref Globals.Config.Player.Name);
            Toggle distance = new Toggle("Distance", "Draw Distance From Player", ref Globals.Config.Player.Distance);
            Toggle weapon = new Toggle("Weapon", "Draw Player's Weapon", ref Globals.Config.Player.Weapon);
            IntSlider maxdistance = new IntSlider("Max Distance", "Maximum Draw Distance Of Players", ref Globals.Config.Player.MaxDistance, 0, 3000, 25);
            Toggle box3d = new Toggle("3D Box", "Draws Box Around Dimensions Of Player", ref Globals.Config.Player.Box3D);
            Toggle box2d = new Toggle("2D Box", "Draws Box From Head To Toe Of Player", ref Globals.Config.Player.Box);
            Toggle fillbox = new Toggle("Fill Box", "Fills The Box", ref Globals.Config.Player.FillBox);
            Toggle skeleton = new Toggle("Skeleton", "Draws Bones With Visibility Checks", ref Globals.Config.Player.Skeleton);
            Toggle visibleonly = new Toggle("Only Draw On Visible Players", "Draws Esp On Visible Players", ref Globals.Config.Player.OnlyDrawVisible);
            playeresp.Items.Add(enable);
            playeresp.Items.Add(name);
            playeresp.Items.Add(distance);
            playeresp.Items.Add(weapon);
            playeresp.Items.Add(maxdistance);
            playeresp.Items.Add(box3d);
            playeresp.Items.Add(box2d);
            playeresp.Items.Add(fillbox);
            playeresp.Items.Add(skeleton);
            playeresp.Items.Add(visibleonly);
            Esp.Items.Add(playeresp);

        }
        Dictionary<string, Configs.ItemFilter> ItemFilters = new Dictionary<string, Configs.ItemFilter>();
        void ItemEsps()
        {
            SubMenu itemesp = new SubMenu("Item ESP", "Draw Item Visuals");
            Toggle enable = new Toggle("Enable", "Enable Item Esp", ref Globals.Config.Item.Enable);
            Toggle name = new Toggle("Name", "Draw Name Of Items", ref Globals.Config.Item.Name);
            Toggle distance = new Toggle("Distance", "Draw Distance From Items", ref Globals.Config.Item.Distance);
            IntSlider maxdistance = new IntSlider("Max Distance", "Maximum Draw Distance Of Items", ref Globals.Config.Item.MaxDistance, 0, 3000, 25);
            Toggle filters = new Toggle("Enable Item Filters", "Enables Custom Item Filters", ref Globals.Config.Item.Filters);
            SubMenu submenu = new SubMenu("Item Filters Menu", "Menu Of Filters");
            ItemFilters.Add("Weapon Filter", Globals.Config.GunFilter);
            ItemFilters.Add("Ammo Filter", Globals.Config.AmmoFilter);
            ItemFilters.Add("Magazine Filter", Globals.Config.MagazineFilter);
            ItemFilters.Add("Misc Supplies Filter", Globals.Config.SupplyFilter);
            ItemFilters.Add("Backpack Filter", Globals.Config.BackpackFilter);
            ItemFilters.Add("Clothing Filter", Globals.Config.ClothingFilter);
            ItemFilters.Add("Explosives Filter", Globals.Config.ExplosivesFilter);
            ItemFilters.Add("Farming Filter", Globals.Config.FarmFilter);
            ItemFilters.Add("Food/Water Filter", Globals.Config.FoodAndWaterFilter);
            ItemFilters.Add("Fuel Filter", Globals.Config.FuelFilter);
            ItemFilters.Add("Medical Filter", Globals.Config.MedicalFilter);
            ItemFilters.Add("Melee Filter", Globals.Config.MeleeFilter);
            ItemFilters.Add("Optic Filter", Globals.Config.OpticFilter);
            ItemFilters.Add("Barrel Filter", Globals.Config.BarrelFilter);
            ItemFilters.Add("Throwable Filter", Globals.Config.ThrowableFilter);
            foreach (KeyValuePair<string,Configs.ItemFilter> pair in ItemFilters)
            {
                SubMenu filtermenu = new SubMenu(pair.Key, "");
                Toggle filterenable = new Toggle("Enable", "Enables Filter", ref pair.Value.Enable);
                Toggle filtername = new Toggle("Name", "Draw Distance For Filter Names", ref pair.Value.Name);
                Toggle filterdistance = new Toggle("Distance", "Draw Distance For Filter Items", ref pair.Value.Distance);
                IntSlider filtermaxdistance = new IntSlider("Max Distance", "Maximum Draw Distance Of Filter Items", ref pair.Value.MaxDistance, 0, 3000, 25);
                filtermenu.Items.Add(filterenable);
                filtermenu.Items.Add(filtername);
                filtermenu.Items.Add(filterdistance);
                filtermenu.Items.Add(filtermaxdistance);
                submenu.Items.Add(filtermenu);
            }
            itemesp.Items.Add(enable);
            itemesp.Items.Add(name);
            itemesp.Items.Add(distance);
            itemesp.Items.Add(maxdistance);
            itemesp.Items.Add(filters);
            itemesp.Items.Add(submenu);
            Esp.Items.Add(itemesp);

        }
        void VehicleEsps()
        {
            SubMenu vehicleesp = new SubMenu("Vehicle ESP", "Draw Vehicle Visuals");
            Toggle enable = new Toggle("Enable", "Enable Vehicle Esp", ref Globals.Config.Vehicle.Enabled);
            Toggle name = new Toggle("Name", "Draw Name Of Vehicle", ref Globals.Config.Vehicle.Name);
            Toggle status = new Toggle("Status", "Draws If Vehicle Is Locked Or Unlocked", ref Globals.Config.Vehicle.LockedStatus);
            Toggle distance = new Toggle("Distance", "Draw Distance From Vehicles", ref Globals.Config.Vehicle.Distance);
            IntSlider maxdistance = new IntSlider("Max Distance", "Maximum Draw Distance Of Vehicles", ref Globals.Config.Vehicle.MaxDistance, 0, 3000, 25);
            Toggle ownvehicles = new Toggle("Draw Own Vehicles", "Draws Vehicles You Own", ref Globals.Config.Vehicle.DrawOwnVehicles);
            Toggle unlocked = new Toggle("Draw Unlocked Vehicles", "Draws Unlocked Vehicles", ref Globals.Config.Vehicle.DrawUnlocked);
            Toggle locked = new Toggle("Draw Locked Vehicles", "Draws Locked Vehicles", ref Globals.Config.Vehicle.DrawLocked);

            vehicleesp.Items.Add(enable);
            vehicleesp.Items.Add(name);
            vehicleesp.Items.Add(distance);
            vehicleesp.Items.Add(status);
            vehicleesp.Items.Add(maxdistance);
            vehicleesp.Items.Add(ownvehicles);
            vehicleesp.Items.Add(unlocked);
            vehicleesp.Items.Add(locked);
            Esp.Items.Add(vehicleesp);

        }
        void FriendlyPlayers()
        {
            SubMenu playeresp = new SubMenu("Friendly Player ESP", "Draw Zombie Visuals");
            Toggle enable = new Toggle("Enable", "Enable Player Esp", ref Globals.Config.FriendlyPlayer.Enable);
            Toggle name = new Toggle("Name", "Draw Name Of Player", ref Globals.Config.FriendlyPlayer.Name);
            Toggle distance = new Toggle("Distance", "Draw Distance From Player", ref Globals.Config.FriendlyPlayer.Distance);
            Toggle weapon = new Toggle("Weapon", "Draw Player's Weapon", ref Globals.Config.FriendlyPlayer.Weapon);
            IntSlider maxdistance = new IntSlider("Max Distance", "Maximum Draw Distance Of Players", ref Globals.Config.FriendlyPlayer.MaxDistance, 0, 3000, 25);
            Toggle box3d = new Toggle("3D Box", "Draws Box Around Dimensions Of Player", ref Globals.Config.FriendlyPlayer.Box3D);
            Toggle box2d = new Toggle("2D Box", "Draws Box From Head To Toe Of Player", ref Globals.Config.FriendlyPlayer.Box);
            Toggle fillbox = new Toggle("Fill Box", "Fills The Box", ref Globals.Config.FriendlyPlayer.FillBox);
            Toggle skeleton = new Toggle("Skeleton", "Draws Bones With Visibility Checks", ref Globals.Config.FriendlyPlayer.Skeleton);
            Toggle visibleonly = new Toggle("Only Draw On Visible Players", "Draws Esp On Visible Players", ref Globals.Config.FriendlyPlayer.OnlyDrawVisible);
            playeresp.Items.Add(enable);
            playeresp.Items.Add(name);
            playeresp.Items.Add(distance);
            playeresp.Items.Add(weapon);
            playeresp.Items.Add(maxdistance);
            playeresp.Items.Add(box3d);
            playeresp.Items.Add(box2d);
            playeresp.Items.Add(fillbox);
            playeresp.Items.Add(skeleton);
            playeresp.Items.Add(visibleonly);
            Esp.Items.Add(playeresp);

        }
        void ColourPicker()
        {
            Dictionary<string,SubMenu> CreatedMenuList = new Dictionary<string, SubMenu>();
            foreach (KeyValuePair<string, Color32> value in Globals.Config.Colours.GlobalColors)
            {
              
                    string HostColourMenuString = value.Key.Substring(0, value.Key.IndexOf(" ")); // find the first space and end there
                    SubMenu HostMenu = new SubMenu(HostColourMenuString, "Fully Automated Colour System And Automated Categorization");
                    if (CreatedMenuList.ContainsKey(HostColourMenuString))
                        HostMenu = CreatedMenuList[HostColourMenuString];
                    else
                    {
                        CreatedMenuList[HostColourMenuString] = HostMenu;
                    }
                         SubMenu colourmenu = new SubMenu(value.Key.Substring(value.Key.IndexOf(" ") +1,value.Key.Length - value.Key.IndexOf(" ") -1), "Fully Automated Colour System And Automated Categorization");
                         int alpha = Helpers.ColourHelper.GetColour(value.Key).a;
                         IntSlider slidera = new IntSlider("Alpha", "Change The Colour Opacity", ref alpha, 0, 255, 10);
                         int red = Helpers.ColourHelper.GetColour(value.Key).r;
                         IntSlider sliderr = new IntSlider("Red", "Change Amount Of Red In Colour", ref red, 0, 255, 10);
                         int green = Helpers.ColourHelper.GetColour(value.Key).g;
                         IntSlider sliderg = new IntSlider("Green", "Change Amount Of Green In Colour", ref green, 0, 255, 10);
                         int blue = Helpers.ColourHelper.GetColour(value.Key).b;
                         IntSlider sliderb = new IntSlider("Blue", "Change Amount Of Blue In Colour", ref blue, 0, 255, 10);
                         colourmenu.Items.Add(slidera);
                         colourmenu.Items.Add(sliderr);
                         colourmenu.Items.Add(sliderg);
                         colourmenu.Items.Add(sliderb);
                         colourmenu.Items.Add(new Button("Save Colour", "Right Arrow To Save The Colour", () => Helpers.ColourHelper.SetColour(value.Key, new Color32((byte)red, (byte)green, (byte)blue, (byte)alpha))));
                         HostMenu.Items.Add(colourmenu);
                    
              
                // got to add the menus after we have initialized all the values
          
            }
            foreach (SubMenu menu in CreatedMenuList.Values)
                Colours.Items.Add(menu);
          
        }

        #region Vars

        static SubMenu Main = new SubMenu("Main", "Menu");
        static SubMenu Esp = new SubMenu("ESP", "Draw Visuals");
        static SubMenu Aimbot = new SubMenu("Aimbot", "Lock Onto Enemies");
        static SubMenu Misc = new SubMenu("Misc", "Modify World And Local Player Values");
        static SubMenu PlayerList = new SubMenu("Lists", "Modify Specific Player Values And Manage Friends Or Certain Items");
        static SubMenu Colours = new SubMenu("Colour Menu", "Allows You To Change Colours On The Cheat");
        static SubMenu Config = new SubMenu("Config Menu", "Allows You To Save And Load Settings");

        static SubMenu ZombieAimbot = new SubMenu("Zombie Aimbot", "Aimbot Options For Zombies");
        static SubMenu PlayerAimbot = new SubMenu("Player Aimbot", "Aimbot Options For Players");

        SubMenu FriendsList = new SubMenu("Friend List", "Managed Friends In Your Friends List");
        SubMenu PlayersList = new SubMenu("Player List", "Managed Players In Your Game");
        SubMenu VehiclesList = new SubMenu("Vehicle List", "Managed Vehicles In Your Game");
        #endregion
        #region Actual Code

        static List<SubMenu> MenuHistory = new List<SubMenu>();
        static SubMenu CurrentMenu;
        static Entity Selected;
        static bool ShowGUI = true;
        static Direct2DBrush PrimaryColour;
        static Direct2DBrush SecondaryColour;
        static Direct2DFont Verdana;
        static KeyCode SetKey()
        {
            KeyCode Key = new KeyCode();
            Event e = Event.current;
            if (e.keyCode != KeyCode.RightArrow)
            {
                Key = e.keyCode;


            }
            else
            {
                Key = KeyCode.None;

            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                Key = KeyCode.Mouse0;

            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Key = KeyCode.Mouse1;

            }
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Key = KeyCode.Mouse2;

            }
            if (Input.GetKeyDown(KeyCode.Mouse3))
            {
                Key = KeyCode.Mouse3;

            }
            if (Input.GetKeyDown(KeyCode.Mouse4))
            {
                Key = KeyCode.Mouse4;

            }
            if (Input.GetKeyDown(KeyCode.Mouse5))
            {
                Key = KeyCode.Mouse5;

            }
            if (Input.GetKeyDown(KeyCode.Mouse6))
            {
                Key = KeyCode.Mouse6;

            }
            return Key;
        }
        IEnumerator KeyControls()
        {

            for (; ; )
            {

                try
                {

                   
                        if (Input.GetKeyDown(KeyCode.Insert))
                            ShowGUI = !ShowGUI;
                    if (ShowGUI)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow) && CurrentMenu.index < CurrentMenu.Items.Count - 1)
                            CurrentMenu.index++;
                        if (Input.GetKeyDown(KeyCode.UpArrow) && CurrentMenu.index > 0)
                            CurrentMenu.index--;
                        if (Input.GetKeyDown(KeyCode.Backspace) && MenuHistory.Count > 1)
                        {
                            CurrentMenu = MenuHistory[MenuHistory.Count - 2];
                            MenuHistory.Remove(MenuHistory.Last<SubMenu>());
                            goto End;
                        }
                        if (((Input.GetKeyDown(KeyCode.LeftArrow) && Selected is SubMenu)) && CurrentMenu.index < CurrentMenu.Items.Count)
                        {

                            CurrentMenu = MenuHistory[MenuHistory.Count - 2];
                            MenuHistory.Remove(MenuHistory.Last<SubMenu>());
                            goto End;
                        }
                        foreach (Entity entity in CurrentMenu.Items)
                        {

                            if (CurrentMenu.index == CurrentMenu.Items.IndexOf(entity))
                                Selected = entity;
                            if (entity != Selected)
                                continue;

                        }
                        if (Selected is Keybind)
                        {
                            Keybind bind = Selected as Keybind;
                            if (bind.Value == KeyCode.None)
                                bind.Value = SetKey();

                        }
                        if (Selected is SubMenu && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
                        {
                            CurrentMenu = Selected as SubMenu;
                            MenuHistory.Add(Selected as SubMenu);
                            goto End;// opens a new menu so we need to exit the loop to then render our new currentmenu
                        }
                        if (Selected is Toggle && Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            Toggle toggle = Selected as Toggle;
                            toggle.Value = true;
                        }
                        if (Selected is Toggle && Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            Toggle toggle = Selected as Toggle;
                            toggle.Value = false;
                        }
                        if (Selected is Button && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
                        {
                            Button button = Selected as Button;
                            button.Method();
                        }
                        if (Selected is IntSlider && Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            IntSlider slider = Selected as IntSlider;
                            int result = slider.Value + slider.IncrementValue;

                            if (result > slider.MaxValue)
                                slider.Value = slider.MaxValue;
                            else
                                slider.Value = result;
                        }
                        if (Selected is IntSlider && Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            IntSlider slider = Selected as IntSlider;
                            int result = slider.Value - slider.IncrementValue;

                            if (result < slider.MinValue)
                                slider.Value = slider.MinValue;
                            else
                                slider.Value = result;
                        }
                        if (Selected is FloatSlider && Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            FloatSlider slider = Selected as FloatSlider;
                            float result = slider.Value + slider.IncrementValue;

                            if (result > slider.MaxValue)
                                slider.Value = slider.MaxValue;
                            else
                                slider.Value = result;
                        }
                        if (Selected is FloatSlider && Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            FloatSlider slider = Selected as FloatSlider;
                            float result = slider.Value - slider.IncrementValue;

                            if (result < slider.MinValue)
                                slider.Value = slider.MinValue;
                            else
                                slider.Value = result;
                        }
                        if (Selected is Keybind && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
                        {
                            Keybind bind = Selected as Keybind;
                            bind.Value = KeyCode.None;
                        }

                    }
                }
                catch (Exception ex) { }
                End:
                yield return new WaitForEndOfFrame();
            }
        }
        public static void Render(Direct2DRenderer Renderer)
        {

            #region Brushes
            Color32 OriginalPrimaryColour = ColourHelper.GetColour("Menu Primary Colour");
            Color32 OriginalSecondaryColour = ColourHelper.GetColour("Menu Secondary Colour");
            PrimaryColour = Renderer.CreateBrush(OriginalPrimaryColour.r, OriginalPrimaryColour.g, OriginalPrimaryColour.b, OriginalPrimaryColour.a);
            SecondaryColour = Renderer.CreateBrush(OriginalSecondaryColour.r, OriginalSecondaryColour.g, OriginalSecondaryColour.b, OriginalSecondaryColour.a);
            #endregion
            #region Font
            Verdana = Renderer.CreateFont("Verdana", 18);
            #endregion
            #region MenuHistory
            try
            {
                if (!ShowGUI)
                    return;
                string text = string.Empty;
                if (MenuHistory.Count > 0)
                {
                    foreach (SubMenu subMenu in MenuHistory)
                    {
                        if (subMenu != null)
                        {
                            if (subMenu == MenuHistory.Last<SubMenu>())
                            {
                                text += subMenu.Name + " v ";
                            }
                            else
                            {
                                text = text + subMenu.Name + " > ";
                            }
                        }
                    }
                }
                Renderer.DrawText(text, Globals.Config.Menu.Menux - 10, Globals.Config.Menu.Menuy - 20, 12, Verdana, PrimaryColour);
                #endregion

                foreach (Entity entity in CurrentMenu.Items)
                {

                    if (Selected == entity)
                    {
                        if (entity.Description != null)
                            Renderer.DrawText(entity.Description, Globals.Config.Menu.Menux - 10, Globals.Config.Menu.Menuy + (20f * (float)CurrentMenu.Items.Count), 12, Verdana, PrimaryColour);
                        if (entity is Toggle)
                        {
                            Toggle toggle = entity as Toggle;
                            string ToggleStr = toggle.Value ? "Enabled" : "Disabled";
                            Renderer.DrawText($"- {entity.Name}: {ToggleStr}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                        }
                        if (entity is IntSlider)
                        {
                            IntSlider slider = entity as IntSlider;
                            Renderer.DrawText($"- {entity.Name}: {slider.Value}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                        }
                        if (entity is FloatSlider)
                        {
                            FloatSlider slider = entity as FloatSlider;
                            Renderer.DrawText($"- {entity.Name}: {Math.Round(slider.Value, 2)}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                        }
                        if (entity is Keybind)
                        {
                            Keybind bind = entity as Keybind;
                            Renderer.DrawText($"- {entity.Name}: {bind.Value.ToString()}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                        }
                        if (entity is SubMenu)

                            Renderer.DrawText($"> {entity.Name}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                        if (entity is Button)
                            Renderer.DrawText($"- {entity.Name}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                    }
                    else
                    {
                        if (entity is Toggle)
                        {
                            Toggle toggle = entity as Toggle;
                            string ToggleStr = toggle.Value ? "Enabled" : "Disabled";
                            Renderer.DrawText($"- {entity.Name}: {ToggleStr}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                        }
                        if (entity is IntSlider)
                        {
                            IntSlider slider = entity as IntSlider;
                            Renderer.DrawText($"- {entity.Name}: {slider.Value}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                        }
                        if (entity is FloatSlider)
                        {
                            FloatSlider slider = entity as FloatSlider;
                            Renderer.DrawText($"- {entity.Name}: {Math.Round(slider.Value, 2)}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                        }
                        if (entity is Keybind)
                        {
                            Keybind bind = entity as Keybind;
                            Renderer.DrawText($"- {entity.Name}: {bind.Value.ToString()}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                        }
                        if (entity is SubMenu)
                            Renderer.DrawText($"> {entity.Name}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                        if (entity is Button)
                            Renderer.DrawText($"- {entity.Name}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                    }
                }

            }
            catch (Exception ex) { }
          

        }
        #endregion
    }
}
