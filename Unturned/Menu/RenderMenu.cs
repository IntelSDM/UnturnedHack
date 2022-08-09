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

            // Initialize Main Menu
            MenuHistory.Add(Main);
            CurrentMenu = Main;
            Selected = Esp;
        }
        void Start()
        {

            MainMenu();
            Zombies();
            ColourPicker();
            StartCoroutine(KeyControls());
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
            Toggle visibleonly = new Toggle("Only Draw On Visible Zombies", "Draws Esp On Visible Zombies", ref Globals.Config.Zombie.OnlyDrawVisible);
            ZombieEsp.Items.Add(enable);
            ZombieEsp.Items.Add(name);
            ZombieEsp.Items.Add(distance);
            ZombieEsp.Items.Add(maxdistance);
            ZombieEsp.Items.Add(box3d);
            ZombieEsp.Items.Add(box2d);
            ZombieEsp.Items.Add(fillbox);
            ZombieEsp.Items.Add(visibleonly);
            Esp.Items.Add(ZombieEsp);

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
        static SubMenu PlayerList = new SubMenu("Player List", "Modify Specific Player Values");
        static SubMenu Colours = new SubMenu("Colour Menu", "Allows You To Change Colours On The Cheat");
        static SubMenu Config = new SubMenu("Config Menu", "Allows You To Save And Load Settings");
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
                    if (Input.GetKeyDown(KeyCode.DownArrow) && CurrentMenu.index < CurrentMenu.Items.Count -1)
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
