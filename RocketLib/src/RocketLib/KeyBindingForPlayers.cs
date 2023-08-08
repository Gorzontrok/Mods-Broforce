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
    [Serializable]
    public class KeyBindingForPlayers
    {
        [XmlIgnore]
        public static Dictionary<string, List<KeyBindingForPlayers>> keyBindingForPlayers = new Dictionary<string, List<KeyBindingForPlayers>>();

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
        [JsonIgnore, XmlIgnore]
        public string modId;

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

        public KeyBindingForPlayers(string modId, string name)
        {
            this.name = name;
            this.modId = modId;
            player0 = new KeyBinding(modId, string.Empty);
            player1 = new KeyBinding(modId, string.Empty);
            player2 = new KeyBinding(modId, string.Empty);
            player3 = new KeyBinding(modId, string.Empty);
        }

        public void Init(string modName)
        {
            try
            {
                if(keyBindingForPlayers == null)
                    keyBindingForPlayers = new Dictionary<string, List<KeyBindingForPlayers>>();

                List<KeyBindingForPlayers> bindings = new List<KeyBindingForPlayers>();
                bool flag = keyBindingForPlayers.TryGetValue(modName, out bindings);
                if (bindings == null)
                    bindings = new List<KeyBindingForPlayers>();
                bindings.Add(this);
                if (flag)
                {
                    keyBindingForPlayers[modName] = bindings;
                }
                else
                {
                    keyBindingForPlayers.Add(modName, bindings);
                }
            }
            catch(Exception e)
            {
                Main.mod.Logger.Log(e.ToString());
            }
        }

        public virtual bool SetKey(int player,  KeyCode key)
        {
            bool keyHasBeenSet = this[player].SetKey(key);
            return keyHasBeenSet;
        }

        public bool IsDown(int player)
        {
            return this[player].IsDown();
        }

        public bool OnGUI(out int player)
        {
            player = 0;
            bool result = false;
            try
            {
                GUILayout.BeginHorizontal(RGUI.Unexpanded);
                GUILayout.Label(name, RGUI.Unexpanded);
                for (int i = 0; i < 4; i++)
                {
                    GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.Width(200));
                    RGUI.LabelCenteredHorizontally(new GUIContent("Player " + i), GUI.skin.label, RGUI.Unexpanded);
                    var temp = this[i].OnGUI();
                    GUILayout.EndVertical();
                    if (temp)
                    {
                        this[i].isSettingKey = true;
                        result = temp;
                        player = i;
                        break;
                    }
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
