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
            StartCoroutine(KeyControls());
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
                    if (Input.GetKeyDown(KeyCode.DownArrow) && CurrentMenu.index < CurrentMenu.Items.Count)
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
