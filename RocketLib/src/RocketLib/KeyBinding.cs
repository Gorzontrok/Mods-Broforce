using System;
using System.Collections;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using UnityModManagerNet;

namespace RocketLib
{
    [Flags]
    public enum ModifierKeys
    {
        None = 0,
        Ctrl = 1 << 0,
        Shift = 1 << 1,
        Alt = 1 << 2
    }

    [Serializable]
    public class KeyBinding : IEquatable<KeyBinding>
    {
        public string name;
        public KeyCode key;
        public bool axis = false;
        public string joystickAxis;
        public int joystickDirection = 1;
        public float axisThreshold = 0.8f;
        public string joystickDisplayName;
        public bool playStation = false;
        public int axisNum = -1;
        public ModifierKeys modifiers = ModifierKeys.None;
        [JsonIgnore, XmlIgnore]
        public bool isSettingKey;
        [JsonIgnore, XmlIgnore]
        public static Rect toolTipRect;
        [JsonIgnore, XmlIgnore]
        public bool wasDown = false;
        [JsonIgnore, XmlIgnore]
        public static bool[] playstationControllers = { false, false, false, false };

        public KeyBinding()
        {
            this.name = string.Empty;
            AssignKey(KeyCode.None);
        }

        public KeyBinding(string name)
        {
            this.name = name;
            AssignKey(KeyCode.None);
        }

        public virtual bool Equals(KeyBinding other)
        {
            if (other == null)
                return false;

            return (this.axis == false && this.axis == other.axis && this.key == other.key && this.modifiers == other.modifiers) || (this.axis == true && this.axis == other.axis && this.joystickAxis == other.joystickAxis);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            KeyBinding keyBindingObj = obj as KeyBinding;
            if (keyBindingObj == null)
                return false;
            else
                return Equals(keyBindingObj);
        }

        public static bool operator ==(KeyBinding keyBinding1, KeyBinding keyBinding2)
        {
            if (((object)keyBinding1) == null || ((object)keyBinding2) == null)
                return object.Equals(keyBinding1, keyBinding2);

            return keyBinding1.Equals(keyBinding2);
        }

        public static bool operator !=(KeyBinding keyBinding1, KeyBinding keyBinding2)
        {
            if (((object)keyBinding1) == null || ((object)keyBinding2) == null)
                return !object.Equals(keyBinding1, keyBinding2);

            return !(keyBinding1.Equals(keyBinding2));
        }

        public override int GetHashCode()
        {
            return this.key.GetHashCode() ^ this.modifiers.GetHashCode();
        }

        /// <summary>
        /// Checks whether a keybinding has been assigned.
        /// </summary>
        /// <returns>True if a keybinding is assigned, otherwise false.</returns>
        public bool HasKeyAssigned()
        {
            return this.key != KeyCode.None || this.axis;
        }

        /// <summary>
        /// Gets state of key
        /// </summary>
        /// <returns>True if key is pressed down</returns>
        public virtual bool IsDown()
        {
            // Check if required modifiers are pressed
            if (modifiers != ModifierKeys.None)
            {
                ModifierKeys currentModifiers = ModifierKeys.None;
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    currentModifiers |= ModifierKeys.Ctrl;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    currentModifiers |= ModifierKeys.Shift;
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                    currentModifiers |= ModifierKeys.Alt;

                if ((currentModifiers & modifiers) != modifiers)
                    return false;
            }

            if (this.axis)
            {
                // Check if keybinding is set for a playstation controller's left trigger or right trigger
                if (playStation && axisNum == 4 || axisNum == 5)
                {
                    return (Input.GetAxis(this.joystickAxis) + 1) / 2.0f >= this.axisThreshold;
                }
                else
                {
                    return Mathf.Abs(Input.GetAxis(this.joystickAxis)) >= this.axisThreshold && Mathf.Sign(Input.GetAxis(this.joystickAxis)) == joystickDirection;
                }

            }
            else
            {
                return Input.GetKey(key);
            }
        }

        /// <summary>
        /// Checks if key was just pressed
        /// </summary>
        /// <returns>True if key was pressed this frame</returns>
        public virtual bool PressedDown()
        {
            bool down = IsDown();
            if (!wasDown && down)
            {
                wasDown = down;
                return true;
            }
            else
            {
                wasDown = down;
                return false;
            }
        }

        /// <summary>
        /// Checks if key was just released
        /// </summary>
        /// <returns>True if key was released this frame</returns>
        public virtual bool Released()
        {
            bool down = IsDown();
            if (wasDown && !down)
            {
                wasDown = down;
                return false;
            }
            else
            {
                wasDown = down;
                return true;
            }
        }

        public virtual float GetAxis()
        {
            if (this.axis)
            {
                return Input.GetAxis(this.joystickAxis);
            }
            else
            {
                return -2f;
            }
        }

        public virtual void AssignKey(KeyCode key)
        {
            // Unassign key
            if (key == KeyCode.Delete)
            {
                key = KeyCode.None;
            }
            this.key = key;
            this.axis = false;
            this.joystickAxis = string.Empty;
            this.isSettingKey = false;
            this.playStation = false;
            // Clear modifiers when assigning new key (they'll be set in BindKey if needed)
            this.modifiers = ModifierKeys.None;
        }

        public virtual void AssignKey(string joystick, int joystickDirection)
        {
            this.key = KeyCode.None;
            this.axis = true;
            this.joystickAxis = joystick;
            axisNum = joystickAxis[joystickAxis.Length - 1] - '0';
            float axisValue = Input.GetAxis(joystick);
            this.joystickDirection = joystickDirection;
            bool direction = (this.joystickDirection == 1);
            // Set axis display name
            if (!playStation)
            {
                switch (axisNum)
                {
                    case 1:
                        this.joystickDisplayName = "Left Stick " + (direction ? "Right" : "Left");
                        break;
                    case 2:
                        this.joystickDisplayName = "Left Stick " + (direction ? "Down" : "Up");
                        break;
                    case 3:
                        this.joystickDisplayName = (direction ? "Left" : "Right") + " Trigger";
                        break;
                    case 4:
                        this.joystickDisplayName = "Right Stick " + (direction ? "Right" : "Left");
                        break;
                    case 5:
                        this.joystickDisplayName = "Right Stick " + (direction ? "Down" : "Up");
                        break;
                    case 6:
                        this.joystickDisplayName = "D-Pad " + (direction ? "Right" : "Left");
                        break;
                    case 7:
                        this.joystickDisplayName = "D-Pad " + (direction ? "Up" : "Down");
                        break;
                    default:
                        this.joystickDisplayName = joystickAxis;
                        break;
                }
            }
            else
            {
                switch (axisNum)
                {
                    case 1:
                        this.joystickDisplayName = "Left Stick " + (direction ? "Right" : "Left");
                        break;
                    case 2:
                        this.joystickDisplayName = "Left Stick " + (direction ? "Down" : "Up");
                        break;
                    case 3:
                        this.joystickDisplayName = "Right Stick " + (direction ? "Right" : "Left");
                        break;
                    case 4:
                        this.joystickDisplayName = "Left Trigger";
                        break;
                    case 5:
                        this.joystickDisplayName = "Right Trigger";
                        break;
                    case 6:
                        this.joystickDisplayName = "Right Stick " + (direction ? "Down" : "Up");
                        break;
                    case 7:
                        this.joystickDisplayName = "D-Pad " + (direction ? "Right" : "Left");
                        break;
                    case 8:
                        this.joystickDisplayName = "D-Pad " + (direction ? "Up" : "Down");
                        break;
                    default:
                        this.joystickDisplayName = joystickAxis;
                        break;
                }
            }

            this.isSettingKey = false;
        }

        public virtual void ClearKey()
        {
            this.key = KeyCode.None;
            this.axis = false;
            this.joystickAxis = String.Empty;
            this.isSettingKey = false;
            this.modifiers = ModifierKeys.None;
        }

        private string GetKeyDisplayString()
        {
            if (this.axis)
                return this.joystickDisplayName;

            if (key == KeyCode.None)
                return "None";

            string displayString = "";
            if ((modifiers & ModifierKeys.Ctrl) != 0)
                displayString += "Ctrl+";
            if ((modifiers & ModifierKeys.Shift) != 0)
                displayString += "Shift+";
            if ((modifiers & ModifierKeys.Alt) != 0)
                displayString += "Alt+";
            displayString += key.ToString();

            return displayString;
        }

        public static IEnumerator BindKey(KeyBinding keyBinding)
        {
            InputReader.IsBlocked = true;
            yield return new WaitForSeconds(0.1f);
            KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToArray();
            bool exit = false;
            playstationControllers = new bool[] { false, false, false, false };
            while (!exit)
            {
                foreach (KeyCode keyCode in keyCodes)
                {
                    if (Input.GetKeyUp(keyCode))
                    {
                        // Don't bind modifier keys alone
                        if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl ||
                            keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift ||
                            keyCode == KeyCode.LeftAlt || keyCode == KeyCode.RightAlt)
                        {
                            continue;
                        }

                        keyBinding.AssignKey(keyCode);

                        // Check and set modifier keys
                        keyBinding.modifiers = ModifierKeys.None;
                        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                            keyBinding.modifiers |= ModifierKeys.Ctrl;
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                            keyBinding.modifiers |= ModifierKeys.Shift;
                        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                            keyBinding.modifiers |= ModifierKeys.Alt;

                        exit = true;
                        break;
                    }
                }
                // Check controller axes
                for (int i = 1; i < 5; i++)
                {
                    if (playstationControllers[i - 1])
                    {
                        for (int j = 1; j < 9; ++j)
                        {
                            string currentAxis = "Joy" + i + " Axis " + j;

                            // Handle triggers differently since they default to -1
                            if (j == 4 || j == 5)
                            {
                                if ((Input.GetAxis(currentAxis) + 1) / 2.0f >= keyBinding.axisThreshold)
                                {
                                    keyBinding.playStation = true;
                                    keyBinding.AssignKey(currentAxis, (int)Mathf.Sign(Input.GetAxis(currentAxis)));
                                    exit = true;
                                    break;
                                }
                            }
                            else if (Mathf.Abs(Input.GetAxis(currentAxis)) >= keyBinding.axisThreshold)
                            {
                                keyBinding.playStation = true;
                                keyBinding.AssignKey(currentAxis, (int)Mathf.Sign(Input.GetAxis(currentAxis)));
                                exit = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 1; j < 9; ++j)
                        {
                            string currentAxis = "Joy" + i + " Axis " + j;
                            if (Mathf.Abs(Input.GetAxis(currentAxis)) >= keyBinding.axisThreshold)
                            {
                                // Check first if this is a playstation controller
                                string[] joysticknames = Input.GetJoystickNames();
                                if (joysticknames.Length > i - 1 && joysticknames[i - 1].ToLower().Contains("wireless controller"))
                                {
                                    // Recheck this controller now knowing it's playstation
                                    playstationControllers[i - 1] = true;
                                    --i;
                                    continue;
                                }
                                else
                                {
                                    keyBinding.playStation = false;
                                    keyBinding.AssignKey(currentAxis, (int)Mathf.Sign(Input.GetAxis(currentAxis)));
                                    exit = true;
                                    break;
                                }
                            }
                        }
                    }

                }
                yield return null;
            }
            InputReader.IsBlocked = false;
        }

        public virtual bool OnGUI(bool displayToolTip, bool displayName = false)
        {
            GUILayout.BeginHorizontal(RGUI.Unexpanded);
            if (displayName)
            {
                GUILayout.Label(name, RGUI.Unexpanded);
            }
            GUILayout.Space(10);
            bool result;
            string toolTip = displayToolTip ? "Press Delete to clear" : "";
            if (this.isSettingKey)
            {
                result = GUILayout.Button(new GUIContent("Press Any Key/Button", toolTip));
            }
            else
            {
                result = GUILayout.Button(new GUIContent(GetKeyDisplayString(), toolTip));
            }
            toolTipRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndHorizontal();
            if (result && !this.isSettingKey && !InputReader.IsBlocked)
            {
                UnityModManager.UI.Instance.StartCoroutine(BindKey(this));
            }
            else
            {
                result = false;
            }
            return result;
        }

        public virtual bool OnGUI(bool displayToolTip, bool displayName, bool includeNameInside)
        {
            GUILayout.BeginHorizontal(RGUI.Unexpanded);
            if (displayName)
            {
                GUILayout.Label(name, RGUI.Unexpanded);
                GUILayout.Space(10);
            }
            bool result;
            string toolTip = displayToolTip ? "Press Delete to clear" : "";
            string prefix = includeNameInside ? (name + ": ") : "";
            if (this.isSettingKey)
            {
                result = GUILayout.Button(new GUIContent(prefix + "Press Any Key/Button", toolTip));
            }
            else
            {
                result = GUILayout.Button(new GUIContent(prefix + GetKeyDisplayString(), toolTip));
            }
            toolTipRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndHorizontal();
            if (result && !this.isSettingKey && !InputReader.IsBlocked)
            {
                UnityModManager.UI.Instance.StartCoroutine(BindKey(this));
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
