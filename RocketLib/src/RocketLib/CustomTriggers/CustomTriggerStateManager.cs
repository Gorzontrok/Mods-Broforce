using System;
using System.Collections.Generic;

namespace RocketLib.CustomTriggers
{
    public static class CustomTriggerStateManager
    {
        private static Dictionary<string, object> currentState = new Dictionary<string, object>();
        private static Dictionary<string, object> stagingState = new Dictionary<string, object>();

        private static List<Action> onLevelStartCallbacks = new List<Action>();
        private static List<Action> onLevelEndCallbacks = new List<Action>();

        public static T Get<T>(string key, T defaultValue = default(T))
        {
            if (currentState.ContainsKey(key))
                return (T)currentState[key];
            return defaultValue;
        }

        public static void SetForLevelStart<T>(string key, T value)
        {
            stagingState[key] = value;
        }

        public static void SetDuringLevel<T>(string key, T value)
        {
            currentState[key] = value;
        }

        public static void RegisterLevelStartAction(Action callback)
        {
            if (callback != null)
                onLevelStartCallbacks.Add(callback);
        }

        public static void RegisterLevelEndAction(Action callback)
        {
            if (callback != null)
                onLevelEndCallbacks.Add(callback);
        }

        internal static void OnLevelStart()
        {
            foreach (var callback in onLevelEndCallbacks)
            {
                try
                {
                    callback?.Invoke();
                }
                catch (Exception ex)
                {
                    RocketLib.Main.logger.Log("Error in level end callback: " + ex.ToString());
                }
            }

            currentState.Clear();
            foreach (var kvp in stagingState)
                currentState[kvp.Key] = kvp.Value;
            stagingState.Clear();

            foreach (var callback in onLevelStartCallbacks)
            {
                try
                {
                    callback?.Invoke();
                }
                catch (Exception ex)
                {
                    RocketLib.Main.logger.Log("Error in level start callback: " + ex.ToString());
                }
            }
        }
    }
}
