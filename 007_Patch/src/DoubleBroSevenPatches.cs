using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;

namespace DoubleBroSevenTrained
{
    [HarmonyPatch(typeof(DoubleBroSeven))]
    public class DoubleBroSevenPatches
    {
        public static bool NotEnabled
        {
            get { return !Mod.Enabled; }
        }
        public static ModConfigs MSettings
        {
            get { return Mod.ModSettings; }
        }
        public static VanillaConfigs VSettings
        {
            get { return Mod.VanillaSettings; }
        }

        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static void Awake(DoubleBroSeven __instance)
        {
            if (NotEnabled)
                return;

            if (VSettings.useMaxAmmo == Its.Yes)
                __instance.originalSpecialAmmo = VSettings.maxAmmo;
            else if (MSettings.useTearGas)
                __instance.originalSpecialAmmo = 5; // 5 is the last if we put 6 or more, an error is drawn

            __instance.useNewPushingFrames = VSettings.usePushingAnimation;
            __instance.useNewLadderClimbingFrames = VSettings.useLadderClimbingAnimation;
            __instance.useLadderClimbingTransition = VSettings.useLadderClimbingTransitionAnimation;

            __instance.gameObject.AddComponent<Trained007>();
        }

        [HarmonyPatch("UseSpecial")]
        [HarmonyPrefix]
        private static bool NewThrowTearGas(DoubleBroSeven __instance)
        {
            if (NotEnabled || !MSettings.useTearGas)
                return true;

            DoubleBroSevenSpecialType currentSpecialType = __instance.GetFieldValue<DoubleBroSevenSpecialType>("currentSpecialType");
            if (currentSpecialType != DoubleBroSevenSpecialType.TearGas)
                return true;

            Networking.Networking.RPC<float>(PID.TargetAll, new RpcSignature<float>(__instance.PlayThrowLightSound), 0.5f, false);
            if (__instance.IsMine)
            {
                if (__instance.IsDucking && __instance.down)
                {
                    ProjectileController.SpawnGrenadeOverNetwork(__instance.tearGasGrenade, __instance,
                        __instance.X + Mathf.Sign(__instance.transform.localScale.x) * VSettings.spawnTearGasPositionAtFeet.x, // x
                        __instance.Y + VSettings.spawnTearGasPositionAtFeet.y, // y
                        0.001f, 0.011f, // radius & force
                        Mathf.Sign(__instance.transform.localScale.x) * VSettings.spawnTearGasPositionAtFeetForce.x, // xI
                        VSettings.spawnTearGasPositionAtFeetForce.y, // yI
                        __instance.playerNum
                        );
                }
                else
                {
                    ProjectileController.SpawnGrenadeOverNetwork(__instance.tearGasGrenade, __instance,
                        __instance.X + Mathf.Sign(__instance.transform.localScale.x) * VSettings.spawnTearGasPosition.x, // x
                        __instance.Y + VSettings.spawnTearGasPosition.y, // y
                        0.001f, 0.011f, // radius & force
                        Mathf.Sign(__instance.transform.localScale.x) * VSettings.spawnTearGasPositionForce.x, // xI
                        VSettings.spawnTearGasPositionForce.y, // yI
                        __instance.playerNum
                        );
                }
            }
            __instance.SpecialAmmo--;
            return false;
        }

        [HarmonyPatch("FireWeapon")]
        [HarmonyPrefix]
        private static bool NewShooting(DoubleBroSeven __instance, float x, float y, float xSpeed, float ySpeed)
        {
            if (NotEnabled || MSettings.hasDrunkShooting == Its.No)
                return true;

            // 007 less accurate if drunk
            if (__instance.GetInt("martinisDrunk") >= VSettings.drunkAt)
            {
                float additionalYSpeed = UnityEngine.Random.Range(MSettings.drunkShootingAdditionalYSpeedRange.x, MSettings.drunkShootingAdditionalYSpeedRange.y);
                __instance.gunSprite.SetLowerLeftPixel((float)(32 * 3), 32f); // TODO: add to VanillaConfigs
                EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * VSettings.muzzleFlashI.x, ySpeed * VSettings.muzzleFlashI.y, __instance.transform);
                ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed, ySpeed + additionalYSpeed, __instance.playerNum);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(PlayerHUD), "Awake")]
        [HarmonyPostfix]
        private static void AddTearGasToHUD(PlayerHUD __instance)
        {
            if (NotEnabled || !MSettings.useTearGas)
                return;

            List<Material> tempList = __instance.doubleBroGrenades.ToList();
            tempList.Add(Mod.TearGasMaterial);
            __instance.doubleBroGrenades = tempList.ToArray();
        }

        [HarmonyPatch("UseSpecial")]
        [HarmonyPostfix]
        private static void PutBalaclavaOnAvatar(DoubleBroSeven __instance)
        {
            if (NotEnabled || !MSettings.showBalaclavaOnAvatar)
                return;

            DoubleBroSevenSpecialType currentSpecialType = __instance.GetFieldValue<DoubleBroSevenSpecialType>("currentSpecialType");
            if (currentSpecialType != DoubleBroSevenSpecialType.Balaclava)
                return;

            var comp = __instance.GetComponent<Trained007>();
            if (comp != null)
            {
                comp.SetBalaclava(__instance.player.hud, Mod.AvatarBalaclavaTexture);
            }
        }

        [HarmonyPatch("StopUsingSpecialRPC")]
        [HarmonyPrefix]
        private static void RemoveBalaclavaFromAvatar(DoubleBroSeven __instance)
        {
            if (NotEnabled || !MSettings.showBalaclavaOnAvatar)
                return;

            DoubleBroSevenSpecialType currentSpecialType = __instance.GetFieldValue<DoubleBroSevenSpecialType>("currentSpecialType");
            if (currentSpecialType != DoubleBroSevenSpecialType.BalaclavaRemoval)
                return;

            var comp = __instance.GetComponent<Trained007>();
            if (comp != null)
            {
                comp.RemoveBalaclava(__instance.player.hud);
            }
        }


        [HarmonyPatch("AnimateActualIdleFrames")]
        [HarmonyPrefix]
        private static bool AnimateActualIdleFrames(DoubleBroSeven __instance)
        {
            if (NotEnabled)
                return true;

            Traverse t = Traverse.Create(__instance);
            try
            {
                if (t.GetInt("martinisDrunk") >= VSettings.drunkAt && t.GetInt("gunFrame") <= 0 && !__instance.fire)
                {
                    t.CallMethod("SetSpriteOffset", 0f, 0f);
                    t.CallMethod("DeactivateGun");
                    t.SetFieldValue("frameRate", 0.0333f);

                    int num = (int)VSettings.drunkAnimationPosition.x + __instance.frame / 4 % VSettings.drunkAnimationMaxFrame;
                    __instance.SetSpriteLowerLeftPixel(num, (int)VSettings.drunkAnimationPosition.y);
                }
                else
                {
                    AnimateActualIdleFramesBase(__instance); // it's to call base method
                }
                return false;
            }
            catch(Exception ex)
            {
                Main.Log(ex);
            }
            return true;
        }

        [HarmonyPatch(typeof(BroBase), "AnimateActualIdleFrames")]
        [HarmonyReversePatch]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void AnimateActualIdleFramesBase(BroBase __instance)
        {
            // it's to call base method
            // the log is not showed
            // i don't know why or how but it works
            // it's from here: https://gist.github.com/pardeike/45196a8b8ef331f38b14e1a7e5ee1782
            Main.Log($"Test: {__instance}");
        }

        [HarmonyPatch(nameof(AnimateInseminationFrames))]
        [HarmonyPrefix]
        private static bool AnimateInseminationFrames(DoubleBroSeven __instance)
        {
            if (NotEnabled)
                return true;

            int collumn = (int)VSettings.inseminationAnimation.x + __instance.CallMethod<int>("CalculateInseminationFrame");
            __instance.SetSpriteLowerLeftPixel(collumn, (int)VSettings.inseminationAnimation.y);
            return false;
        }

        [HarmonyPatch("SetBalaclavaTime")]
        [HarmonyPrefix]
        private static bool SetBalaclavaTime(DoubleBroSeven __instance, float time)
        {
            if (NotEnabled || VSettings.changeBalaclavaTime == Its.No)
                return true;

            __instance.SetFieldValue("balaclavaTime", VSettings.balaclavaTime);
            return false;
        }
    }
}
