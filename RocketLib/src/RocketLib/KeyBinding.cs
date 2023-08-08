using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace RocketLib
{
    [Serializable]
    public class KeyBinding : IEquatable<KeyBinding>
    {

        public string name;
        public KeyCode key;
        public string modId;
        [JsonIgnore, XmlIgnore]
        public bool isSettingKey;

        public KeyBinding(string modId, string name)
        {
            this.name = name;
            SetKey(KeyCode.None);
            this.modId = modId;
        }

        public virtual bool SetKey(KeyCode key)
        {
            if(key == this.key)
                return true;
            AssignKey(key);
            return true;
        }

        protected virtual void AssignKey(KeyCode key)
        {
            this.key = key;
            isSettingKey = false;
        }

        public virtual bool OnGUI()
        {
            GUILayout.BeginHorizontal(RGUI.Unexpanded);
            GUILayout.Label(name, RGUI.Unexpanded);
            GUILayout.Space(10);
            bool result = GUILayout.Button(isSettingKey ? "Press Any Key/Button" : key.ToString());
            GUILayout.EndHorizontal();
            return result;
        }

        public virtual bool IsDown()
        {
            return Input.GetKey(key);
        }

        public virtual bool IsPressThisFrame()
        {
            return Input.GetKeyDown(key);
        }
        public virtual bool IsReleasedThisFrame()
        {
            return Input.GetKeyUp(key);
        }
        public virtual bool Equals(KeyBinding other)
        {
            if (other == null)
                return false;

            if (key == other.key)
                return true;
            else
                return false;
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
            return this.key.GetHashCode();
        }
    }
}
