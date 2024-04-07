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
        public static Dictionary<string, Dictionary<string, KeyBindingForPlayers>> AllKeyBindings;

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
        /// <param name="name">Name of the key</param>
        /// <param name="modId">Name of the mod that is adding the keybinding</param>
        public KeyBindingForPlayers(string name, string modId)
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

        public bool IsDown(int player)
        {
            return this[player].IsDown();
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
    }
}
