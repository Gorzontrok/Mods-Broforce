using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static RocketLib.CustomTriggers.CustomTriggerPatches;

namespace RocketLib.CustomTriggers
{
    public class CustomTriggerManager
    {
        public static List<CustomTrigger> CustomTriggers = new List<CustomTrigger>();
        static TriggerActionInfo currentAction;
        static string currentActionName;

        public static void RegisterCustomTrigger(Type customTriggerActionType, Type customTriggerActionInfoType, string actionName, string tag, int priority = 0)
        {
            if (CustomTriggers.Any(t => t.ActionName == actionName))
            {
                throw new Exception($"Custom trigger with name '{actionName}' is already registered");
            }
            CustomTrigger customTrigger = new CustomTrigger(customTriggerActionType, customTriggerActionInfoType, actionName, tag, priority);
            CustomTriggers.Add(customTrigger);

            CustomTriggers = CustomTriggers
                .OrderBy(t => t.Tag)
                .ThenByDescending(t => t.Priority)
                .ThenBy(t => t.ActionName)
                .ToList();
        }

        public static void DisplayAddCustomTriggers(LevelEditorGUI __instance, ref TriggerInfo ___selectedTrigger, ref TriggerActionInfo ___selectedAction)
        {
            GUILayout.Space(10);
            GUILayout.Label("Modded Actions:");
            GUILayout.Space(7);

            string currentTag = null;
            foreach (var customTrigger in CustomTriggers)
            {
                if (currentTag != customTrigger.Tag)
                {
                    currentTag = customTrigger.Tag;
                    GUILayout.Space(5);
                    GUILayout.Label($"[{currentTag}]");
                    GUILayout.Space(2);
                }

                if (GUILayout.Button("Add New " + customTrigger.ActionName + " Action", new GUILayoutOption[0]))
                {
                    LevelEditorGUI_ShowTriggerMenu_Patch.PlayClickSound(__instance);
                    TriggerActionInfo customActionInfo = Activator.CreateInstance(customTrigger.CustomTriggerActionInfoType) as TriggerActionInfo;
                    customActionInfo.type = TriggerActionType.Weather;
                    ___selectedTrigger.actions.Add(customActionInfo);
                    ___selectedAction = customActionInfo;
                }
            }
        }

        public static string GetCustomActionType(TriggerActionInfo actionInfo)
        {
            if (currentAction == actionInfo)
            {
                return currentActionName;
            }

            foreach (var customTrigger in CustomTriggers)
            {
                if (actionInfo.GetType() == customTrigger.CustomTriggerActionInfoType)
                {
                    currentActionName = customTrigger.ActionName;
                    currentAction = actionInfo;
                    return currentActionName;
                }
            }

            return string.Empty;
        }

        public static CustomTriggerActionInfo ConvertToCustomInfo(WeatherActionInfo weatherInfo)
        {
            try
            {
                string[] parts = weatherInfo.name.Split('|');
                if (parts.Length < 3)
                {
                    throw new Exception("Invalid custom trigger encoding format");
                }

                string encodedJson = parts[1];
                string actualName = parts[2];

                string json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedJson));

                JObject wrapper = JObject.Parse(json);
                string triggerType = wrapper["TriggerType"]?.ToString();

                if (string.IsNullOrEmpty(triggerType))
                {
                    throw new Exception("TriggerType not found in JSON");
                }

                var customTrigger = CustomTriggers.FirstOrDefault(t => t.ActionName == triggerType);
                if (customTrigger == null)
                {
                    throw new Exception($"Unknown custom trigger type: {triggerType}");
                }

                CustomTriggerActionInfo customInfo = Activator.CreateInstance(customTrigger.CustomTriggerActionInfoType) as CustomTriggerActionInfo;

                customInfo.type = weatherInfo.type;
                customInfo.timeOffset = weatherInfo.timeOffset;
                customInfo.onlyOnHardMode = weatherInfo.onlyOnHardMode;
                customInfo.name = actualName;

                JToken dataToken = wrapper["Data"];
                if (dataToken != null)
                {
                    JsonConvert.PopulateObject(dataToken.ToString(), customInfo);
                }

                return customInfo;
            }
            catch (Exception ex)
            {
                RocketLib.Main.logger.Log($"Failed to convert WeatherActionInfo to CustomTriggerActionInfo: {ex}");
                throw;
            }
        }

        public static WeatherActionInfo ConvertToWeatherInfo(CustomTriggerActionInfo customInfo)
        {
            try
            {
                WeatherActionInfo weatherInfo = new WeatherActionInfo();

                string triggerType = GetCustomActionType(customInfo);

                if (string.IsNullOrEmpty(triggerType))
                {
                    throw new Exception("Custom trigger type not registered");
                }

                var wrapper = new
                {
                    TriggerType = triggerType,
                    Data = customInfo
                };

                string json = JsonConvert.SerializeObject(wrapper, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                string encodedJson = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));

                weatherInfo.type = TriggerActionType.Weather;
                weatherInfo.timeOffset = customInfo.timeOffset;
                weatherInfo.onlyOnHardMode = customInfo.onlyOnHardMode;
                weatherInfo.name = $"CUSTOMTRIGGER|{encodedJson}|{customInfo.name ?? string.Empty}";

                return weatherInfo;
            }
            catch (Exception ex)
            {
                RocketLib.Main.logger.Log($"Failed to convert CustomTriggerActionInfo to WeatherActionInfo: {ex}");
                throw;
            }
        }

        public static TriggerAction CreateCustomAction(TriggerActionInfo info)
        {
            try
            {
                string[] parts = info.name.Split('|');
                if (parts.Length < 3)
                {
                    throw new Exception("Invalid custom trigger encoding format");
                }

                string encodedJson = parts[1];
                string actualName = parts[2];

                string json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedJson));

                JObject wrapper = JObject.Parse(json);
                string triggerType = wrapper["TriggerType"]?.ToString();

                if (string.IsNullOrEmpty(triggerType))
                {
                    throw new Exception("TriggerType not found in JSON");
                }

                var customTrigger = CustomTriggers.FirstOrDefault(t => t.ActionName == triggerType);
                if (customTrigger == null)
                {
                    throw new Exception($"Unknown custom trigger type: {triggerType}");
                }

                CustomTriggerActionInfo customInfo = ConvertToCustomInfo(info as WeatherActionInfo);

                TriggerAction action = Activator.CreateInstance(customTrigger.CustomTriggerActionType) as TriggerAction;
                action.Info = customInfo;
                action.timeOffsetLeft = customInfo.timeOffset;
                action.AssignDeterministicIDs();

                return action;
            }
            catch (Exception ex)
            {
                RocketLib.Main.logger.Log($"Failed to create custom trigger action: {ex}");
                throw;
            }
        }

        public static void ConvertAllCustomTriggersInMapData(MapData mapData)
        {
            if (mapData == null || mapData.TriggerList == null || CustomTriggers.Count == 0)
            {
                return;
            }

            foreach (var trigger in mapData.TriggerList)
            {
                if (trigger.actions == null)
                    continue;

                for (int i = 0; i < trigger.actions.Count; i++)
                {
                    var action = trigger.actions[i];
                    if (action is WeatherActionInfo weatherAction && weatherAction.name != null && weatherAction.name.StartsWith("CUSTOMTRIGGER|"))
                    {
                        var customInfo = ConvertToCustomInfo(weatherAction);
                        trigger.actions[i] = customInfo;
                    }
                }
            }
        }
    }
}
