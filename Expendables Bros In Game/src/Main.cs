using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using System.Reflection;

namespace ExpendablesBrosInGame_Mod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static Dictionary<int, HeroType> ExpendablesBro_dico = new Dictionary<int, HeroType>() {
            { 42, HeroType.BroneyRoss },
            { 52, HeroType.LeeBroxmas },
            { 62, HeroType.BronnarJensen },
            { 72, HeroType.HaleTheBro },
            { 82, HeroType.TrentBroser },
            { 92, HeroType.Broc },
            { 102, HeroType.TollBroad }
        };

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            settings = Settings.Load<Settings>(modEntry);
            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception e)
            {
                mod.Logger.Log(e.ToString());
            }
            return true;
        }
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var warning = new GUIStyle();
            warning.normal.textColor = Color.yellow;
            warning.fontStyle = FontStyle.Bold;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("If you have 'FiltredBros', i recommend you to disable this mod.", warning, GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            settings.ExpendaEnable = GUILayout.Toggle(settings.ExpendaEnable, "Enable Expendabros", GUILayout.ExpandWidth(false));
            settings.brondflyEnabled = GUILayout.Toggle(settings.brondflyEnabled, "Enable Brondfly", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
        internal static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        internal static void CheckExpendables(Dictionary<int, HeroType> Bro_dico)
        {
            try
            {
                int brondflyStep = 861;
                //Add Expendable Bros
                foreach (KeyValuePair<int, HeroType> bros in ExpendablesBro_dico)
                {
                    if (!settings.ExpendaEnable & Bro_dico.Contains(bros))
                    {
                        Main.Log("Remove " + bros.Value + ".....");
                        Bro_dico.Remove(bros.Key);
                    }
                    else if (settings.ExpendaEnable & !Bro_dico.Contains(bros))
                    {
                        if (bros.Value == HeroType.BrondleFly & settings.brondflyEnabled)
                            Main.Log(bros.Value + " is missing ! Adding....");
                        Bro_dico.Add(bros.Key, bros.Value);
                    }
                }

                //Add or remove Brondfly depend on option
                if (settings.brondflyEnabled & !Bro_dico.ContainsValue(HeroType.BrondleFly))
                {
                    Main.Log("Brondfly is missing ! Adding....");
                    Bro_dico.Add(brondflyStep, HeroType.BrondleFly);
                }
                else if (!settings.brondflyEnabled & Bro_dico.ContainsValue(HeroType.BrondleFly))
                {
                    Main.Log("Remove Brondfly.....");
                    Bro_dico.Remove(brondflyStep);
                }
            }
            catch (Exception ex) { Main.Log(ex); }
        }
    }

    [HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")] //Patch for add the Bros
    static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
        // After my mod FiltredBros, i have no idea how this can working..
        public static bool Prefix(ref HeroType hero)
        {
            Dictionary<int, HeroType> newHeroUnlockIntervals = Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").GetValue() as Dictionary<int, HeroType>;
            if(Main.enabled)
                Main.CheckExpendables(newHeroUnlockIntervals);
            return newHeroUnlockIntervals.ContainsValue(hero);
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool brondflyEnabled;
        public bool ExpendaEnable;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}