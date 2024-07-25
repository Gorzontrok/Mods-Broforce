using JetBrains.Annotations;
using Newtonsoft.Json;
using RocketLib.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace RocketLib
{
    public class AllModKeyBindings
    {
        // Dictionary< modname, Dictionary< name of key, keybinding > >
        private static Dictionary<string, Dictionary<string, KeyBindingForPlayers>> AllKeyBindings;

        /// <summary>
        /// Finds a keybind for the mod for the specified key if it exists, otherwise it creates a new one.
        /// </summary>
        /// <param name="modName">Name of mod</param>
        /// <param name="keyName">Name of key</param>
        /// <returns>Previously created or newly created keybinding</returns>
        public static KeyBindingForPlayers LoadKeyBinding(string modName, string keyName)
        {
            KeyBindingForPlayers keybinding;
            if (TryGetKeyBinding(modName, keyName, out keybinding))
            {
                return keybinding;
            }
            else
            {
                keybinding = new KeyBindingForPlayers(modName, keyName);
                AddKeyBinding(keybinding, modName);
                return keybinding;
            }
        }

        /// <summary>
        /// Adds a keybind to the dictionary of all keybinds for a specific mod.
        /// </summary>
        /// <param name="keybinding">Keybind to add</param>
        /// <param name="modId">Name of the mod</param>
        public static void AddKeyBinding(KeyBindingForPlayers keybinding, string modId)
        {
            try
            {
                if (AllKeyBindings == null)
                {
                    return;
                }
                Dictionary<string, KeyBindingForPlayers> currentModKeyBindings;
                bool alreadyExists = AllKeyBindings.TryGetValue(modId, out currentModKeyBindings);
                if (!alreadyExists)
                {
                    currentModKeyBindings = new Dictionary<string, KeyBindingForPlayers>();
                    AllKeyBindings.Add(modId, currentModKeyBindings);
                }
                currentModKeyBindings.Add(keybinding.name, keybinding);
            }
            catch (Exception e)
            {
                Main.mod.Logger.Log(e.ToString());
            }
        }

        /// <summary>
        /// Tries to get a specific keybind for a specific mod.
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="keyName"></param>
        /// <param name="keybinding"></param>
        /// <returns>True if it was found, false otherise</returns>
        public static bool TryGetKeyBinding(string modName, string keyName, out KeyBindingForPlayers keybinding)
        {
            Dictionary<string, KeyBindingForPlayers> currentModKeyBindings;
            if (AllKeyBindings.TryGetValue(modName, out currentModKeyBindings))
            {
                return currentModKeyBindings.TryGetValue(keyName, out keybinding);
            }
            keybinding = null;
            return false;
        }

        /// <summary>
        /// Returns a dictionary containing all the keybinds for the specified mod, if there are any.
        /// </summary>
        /// <param name="modName">Mod to find keybinds for</param>
        /// <param name="modKeyBindings">Dictionary containing all the keybinds for this mod</param>
        /// <returns>True if it was found, false otherise</returns>
        public static bool TryGetAllKeyBindingsForMod(string modName, out Dictionary<string, KeyBindingForPlayers> modKeyBindings)
        {
            return AllKeyBindings.TryGetValue(modName, out modKeyBindings);
        }

        /// <summary>
        /// Clears all keybinds for the specified mod
        /// </summary>
        /// <param name="modName">Name of mod</param>
        public static void ClearKeyBindingsForMod(string modName)
        {
            Dictionary<string, KeyBindingForPlayers> modKeyBindings;
            if ( TryGetAllKeyBindingsForMod(modName, out modKeyBindings) )
            {
                foreach (KeyValuePair<string, KeyBindingForPlayers> pair in modKeyBindings)
                {
                    pair.Value.ClearKey();
                }
            }
        }

        /// <summary>
        /// Converts the dictionary containing the keybinds for all mods into JSON
        /// </summary>
        /// <returns>JSON string of all keybinds</returns>
        public static string ConvertToJson()
        {
            return JsonConvert.SerializeObject(AllKeyBindings, Formatting.Indented);
        }

        /// <summary>
        /// Reads from a JSON string and creates a dictionary storing all keybindings for all mods.
        /// </summary>
        /// <param name="json">JSON string to read from</param>
        /// <returns></returns>
        public static bool ReadFromJson(string json)
        {
            try
            {
                AllKeyBindings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, KeyBindingForPlayers>>>(json);
                return true;
            }
            catch ( Exception e )
            {
                Main.mod.Logger.Log("Exception converting from JSON: " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Creates a new Dictionary for keybindings for all mods. This will delete any currently stored keybindings for all mods.
        /// </summary>
        public static void RecreateDictionary()
        {
            AllKeyBindings = new Dictionary<string, Dictionary<string, KeyBindingForPlayers>>();
        }
    }
    
    [Serializable]
    public class KeyBindingForPlayers
    {
        [XmlIgnore]
        public KeyBinding Player0
        {
            get { return player0; }
            set { player0 = value; }
        }
        [XmlIgnore]
        public KeyBinding Player1
        {
            get { return player1; }
            set { player1 = value; }
        }
        [XmlIgnore]
        public KeyBinding Player2
        {
            get { return player2; }
            set { player2 = value; }
        }
        [XmlIgnore]
        public KeyBinding Player3
        {
            get { return player3; }
            set { player3 = value; }
        }

        public string name;

        protected KeyBinding player0;
        protected KeyBinding player1;
        protected KeyBinding player2;
        protected KeyBinding player3;

        public KeyBinding this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0:
                        return player0;
                    case 1:
                        return player1;
                    case 2:
                        return player2;
                    case 3:
                        return player3;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        player0 = value;
                        break;
                    case 1:
                        player1 = value;
                        break;
                    case 2:
                        player2 = value;
                        break;
                    case 3:
                        player3 = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        // Empty constructor used when loading from JSON
        public KeyBindingForPlayers()
        {
        }

        /// <summary>
        /// Create a Keybinding for all 4 players
        /// </summary>
        /// /// <param name="modId">Name of the mod that is adding the keybinding</param>
        /// <param name="name">Name of the key</param>
        public KeyBindingForPlayers(string modId, string name)
        {
            this.name = name;
            player0 = new KeyBinding(name);
            player1 = new KeyBinding(name);
            player2 = new KeyBinding(name);
            player3 = new KeyBinding(name);

            AllModKeyBindings.AddKeyBinding(this, modId);
        }

        public virtual void AssignKey(int player,  KeyCode key)
        {
            this[player].AssignKey(key);
        }

        public virtual void AssignKey(int player, string joystick, int direction)
        {
            this[player].AssignKey(joystick, direction);
        }

        /// <summary>
        /// Gets state of key
        /// </summary>
        /// <param name="player">Player to check keybinding for</param>
        /// <returns>True if key is currently down</returns>
        public bool IsDown(int player)
        {
            return this[player].IsDown();
        }

        /// <summary>
        /// Checks if key was just pressed
        /// </summary>
        /// <param name="player">Player to check keybinding for</param>
        /// <returns>True if key was pressed this frame</returns>
        public bool PressedDown(int player)
        {
            return this[player].PressedDown();
        }

        /// <summary>
        /// Checks if key was just released
        /// </summary>
        /// <param name="player">Player to check keybinding for</param>
        /// <returns>True if key was released this frame</returns>
        public bool Released(int player)
        {
            return this[player].Released();
        }

        public void ClearKey(int player)
        {
            this[player].ClearKey();
        }

        public void ClearKey()
        {
            for (int i = 0; i < 4; ++i )
            {
                this[i].ClearKey();
            }
        }

        public bool OnGUI(out int player, bool displayToolTip = true )
        {
            player = 0;
            bool result = false;
            try
            {
                GUILayout.BeginHorizontal(RGUI.Unexpanded);
                GUILayout.Label(name, RGUI.Unexpanded);
                Rect toolTipPos = Rect.zero;
                for (int i = 0; i < 4; i++)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.Width(200));
                    RGUI.LabelCenteredHorizontally(new GUIContent("Player " + (i + 1)), GUI.skin.label, RGUI.Unexpanded);
                    var temp = this[i].OnGUI(displayToolTip);
                    if ( i == 0 )
                    {
                        toolTipPos = KeyBinding.toolTipRect;
                        toolTipPos.y += 17;
                    }
                    GUILayout.EndVertical();
                    if (temp)
                    {
                        this[i].isSettingKey = true;
                        result = temp;
                        player = i;
                        break;
                    }
                }
                if ( displayToolTip && GUI.tooltip != string.Empty )
                {
                    GUI.Label(toolTipPos, GUI.tooltip);
                    GUI.tooltip = string.Empty;
                }
                GUILayout.EndHorizontal();
            }
            catch(Exception e)
            {
                ScreenLogger.Instance.ExceptionLog(e);
            }
            return result;
        }

        /// <summary>
        /// Displays keybinding options for one or more players
        /// </summary>
        /// <param name="player">Player that had their keybinding clicked</param>
        /// <param name="displayToolTip">Display tooltip below keybinding</param>
        /// <param name="includeToolTip">Include tooltip with the keybinding. Doesn't display the tooltip unless displayToolTip is also set to true</param>
        /// <param name="previousToolTip">Previous tooltip that was displayed, used to ignore previous tooltips</param>
        /// <param name="playerToDisplay">Player to display if onlyOnePlayer is set to true</param>
        /// <param name="onlyOnePlayer">Only display one player's keybinding options</param>
        /// <param name="separateKeyName">Separate the keyname from the button, if set to false it will be included within the button</param>
        /// <param name="fixedWidth">Controls whether the button should be a fixed width or not</param>
        /// <returns></returns>
        public bool OnGUI(out int player, bool displayToolTip, bool includeToolTip, ref string previousToolTip, int playerToDisplay = -1, bool onlyOnePlayer = false, bool separateKeyName = true, bool fixedWidth = true )
        {
            player = 0;
            bool result = false;
            try
            {
                GUILayout.BeginHorizontal(RGUI.Unexpanded);
                if ( separateKeyName )
                {
                    GUILayout.Label(name, RGUI.Unexpanded);
                }
                Rect toolTipPos = Rect.zero;
                if ( !onlyOnePlayer )
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (fixedWidth)
                        {
                            GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.Width(200));
                        }
                        else
                        {
                            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
                        }
                        RGUI.LabelCenteredHorizontally(new GUIContent("Player " + (i + 1)), GUI.skin.label, RGUI.Unexpanded);
                        var temp = this[i].OnGUI(includeToolTip, false, !separateKeyName);
                        if (i == 0)
                        {
                            toolTipPos = KeyBinding.toolTipRect;
                            toolTipPos.y += 17;
                        }
                        GUILayout.EndVertical();
                        if (temp)
                        {
                            this[i].isSettingKey = true;
                            result = temp;
                            player = i;
                            break;
                        }
                    }
                }
                else
                {
                    if ( fixedWidth )
                    {
                        GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.Width(200));
                    }
                    else
                    {
                        GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
                    }
                    var temp = this[playerToDisplay].OnGUI(includeToolTip, false, !separateKeyName);
                    toolTipPos = KeyBinding.toolTipRect;
                    toolTipPos.y += 17;
                    GUILayout.EndVertical();
                    if (temp)
                    {
                        this[playerToDisplay].isSettingKey = true;
                        result = temp;
                        player = playerToDisplay;
                    }
                }
                if (displayToolTip && GUI.tooltip != string.Empty && GUI.tooltip != previousToolTip)
                {
                    GUI.Label(toolTipPos, GUI.tooltip);
                    GUI.tooltip = string.Empty;
                }
                GUILayout.EndHorizontal();
            }
            catch (Exception e)
            {
                ScreenLogger.Instance.ExceptionLog(e);
            }
            return result;
        }
    }
}
