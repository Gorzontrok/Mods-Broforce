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
            GUILayout.BeginHorizontal();
            settings.modEnabled = GUILayout.Toggle(settings.modEnabled, "Enable Expandabros Bros", GUILayout.ExpandWidth(true));
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
        public static void Log(String str)
        {
            mod.Logger.Log(str);
        }

        public static void CheckExpendables(Dictionary<int, HeroType> Bro_dico)
        {
            try
            {
                int brondflyStep = 861;
                //Add Expendabable Bros
                foreach (KeyValuePair<int, HeroType> bros in ExpendablesBro_dico)
                {
                    if (!settings.modEnabled & Bro_dico.Contains(bros))
                    {
                        Main.Log("Remove " + bros.Value + ".....");
                        Bro_dico.Remove(bros.Key);
                    }

                    else if (settings.modEnabled & !Bro_dico.Contains(bros))
                    {
                        if (bros.Value == HeroType.BrondleFly & settings.brondflyEnabled)
                            Main.Log(bros.Value + " is missing ! Adding....");
                        Bro_dico.Add(bros.Key, bros.Value);
                    }
                }

                //Add or remove brondfly depend on option
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
            catch (Exception ex) { Main.Log(ex.ToString()); }

        }
        //for the BronnarJensen Patch
        //Tis is the only way that i found, because they share the same variable

    }

    [HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")] //Patch for add the Brosvisua
    static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
            public static bool Prefix(ref HeroType hero)
            {
                Dictionary<int, HeroType> newHeroUnlockIntervals = Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").GetValue() as Dictionary<int, HeroType>;
                Main.CheckExpendables(newHeroUnlockIntervals);
                return newHeroUnlockIntervals.ContainsValue(hero);
            }
    }



    //Fix duck of Bronnar Jensen   A lot of patch because of protected function
    [HarmonyPatch(typeof(TestVanDammeAnim), "SetGunSprite")]
    static class SetGunSprite_Patch
    {
        static Unit unit;
        public static void Prefix(int spriteFrame, int spriteRow, TestVanDammeAnim __instance)
        {
            bool hangingOneArmed = Traverse.Create(typeof(TestVanDammeAnim)).Field("hangingOneArmed").GetValue<bool>(); //Get the "hangingOneArmed" original Value
            int gunSpritePixelWidth = Traverse.Create(typeof(TestVanDammeAnim)).Field("gunSpritePixelWidth").GetValue<int>(); //Get the "gunSpritePixelWidth" original Value
            int gunSpritePixelHeight = Traverse.Create(typeof(TestVanDammeAnim)).Field("gunSpritePixelHeight").GetValue<int>(); //Get the "gunSpritePixelHeight" original Value

            if (unit.actionState == ActionState.Hanging)
            {
                __instance.gunSprite.SetLowerLeftPixel((float)(gunSpritePixelWidth * (6 + spriteFrame)), (float)(gunSpritePixelHeight * (1 + spriteRow)));
            }
            else if (unit.actionState == ActionState.ClimbingLadder && hangingOneArmed)
            {
                __instance.gunSprite.SetLowerLeftPixel((float)(gunSpritePixelWidth * (6 + spriteFrame)), (float)(gunSpritePixelHeight * (1 + spriteRow)));
            }
            else if (__instance.attachedToZipline != null && unit.actionState == ActionState.Jumping)
            {
                __instance.gunSprite.SetLowerLeftPixel((float)(gunSpritePixelWidth * (6 + spriteFrame)), (float)(gunSpritePixelHeight * (1 + spriteRow)));
            }
            else
            {
                __instance.gunSprite.SetLowerLeftPixel((float)(gunSpritePixelWidth * spriteFrame), (float)(gunSpritePixelHeight * (1 + spriteRow)));
            }
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "FireWeapon")]
    static class FireWeapon_Patch
    {
        public static void Prefix(float x, float y, float xSpeed, float ySpeed, TestVanDammeAnim __instance)
        {
            int gunFrame = Traverse.Create(typeof(BronnarJensen)).Field("gunFrame").GetValue<int>(); //Get the "ducking" original Value

            SetGunSprite_Patch.Prefix(gunFrame, 1, __instance);
            EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.15f, ySpeed * 0.15f, __instance.transform);
            ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed, ySpeed, __instance.playerNum);
        }
    }
    

    [HarmonyPatch(typeof(TestVanDammeAnim), "PlayAttackSound")]
    static class PlayAttackSound_Patch
    {
        static Component comp;
        public static void Prefix(float v, TestVanDammeAnim __instance)
        {
            Sound sound = Traverse.Create(typeof(Sound)).Field("sound").GetValue<Sound>(); //Get the "sound" original Value

            if (sound == null)
            {
                sound = Sound.GetInstance();
            }
            if (sound != null)
            {
                sound.PlaySoundEffectAt(__instance.soundHolder.attackSounds, v, comp.transform.position, 1f + __instance.pitchShiftAmount, true, false, false, 0f);
            }
        }
    }


    [HarmonyPatch(typeof(BronnarJensen), "UseFire")]
    static class BronnarJensen_UseFire_Patch
    {
            public static void Prefix(BronnarJensen __instance, ref NetworkObject NO, ref BroforceObject BO, ref Component comp, ref NetworkedUnit NU,  ref TestVanDammeAnim TVDA, ref Unit unit)
            {
                try
                {
                    bool ducking = Traverse.Create(typeof(BronnarJensen)).Field("ducking").GetValue<bool>(); //Get the "ducking" original Value
                    
                    if (ducking && __instance.down)
                    {
                        FireWeapon_Patch.Prefix(BO.X + comp.transform.localScale.x * 6f, BO.Y + 7f, comp.transform.localScale.x * (__instance.shootGrenadeSpeedX * 0.3f) + __instance.xI * 0.45f, 25f + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)), __instance);
                    }
                    else
                    {
                        FireWeapon_Patch.Prefix(BO.X + comp.transform.localScale.x * 6f, BO.Y + 10f, comp.transform.localScale.x * __instance.shootGrenadeSpeedX + __instance.xI * 0.45f, __instance.shootGrenadeSpeedY + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)), __instance);
                    }
                    PlayAttackSound_Patch.Prefix(0.4f, TVDA);
                    Map.DisturbWildLife(BO.X, BO.Y, 60f, NU.playerNum);
                    Traverse.Create(typeof(BronnarJensen)).Field("fireDelay").SetValue(0.6f); //Change the value of fire delay
                }
                catch(Exception ex)
                {
                Main.Log(ex.ToString());
                }
            }
    }
    

    public class Settings : UnityModManager.ModSettings
    {
        public bool modEnabled;
        public bool brondflyEnabled;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }
}


