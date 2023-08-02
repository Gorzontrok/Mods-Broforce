using HarmonyLib;
using System;
using UnityEngine;

namespace CustomHeroName
{
    [HarmonyPatch(typeof(HeroController), "GetHeroName", typeof(HeroType))]
    static class CustomName_Patch
    {
        public static bool Prefix(ref HeroType type, ref string __result)
        {
            __result = string.Empty;

            if (!Main.enabled && Main.shouldIgnorePatch)
            {
                Main.shouldIgnorePatch = false;
                return true;
            }

            try
            {
                __result = Mod.GetHeroName(type);
            }
            catch (Exception ex)
            {
                Main.Log(ex);
            }

            return string.IsNullOrEmpty(__result);

        }
    }

    [HarmonyPatch(typeof(Player), "SpawnHero")]
    static class GetCurrentBro_Patch
    {
        static void Prefix(HeroType nextHeroType)
        {
            Main.currentHero = nextHeroType;
        }
    }

    [HarmonyPatch(typeof(CutsceneIntroRoot), "OnLoadComplete")]
    static class Cutscene_Patch
    {
        static bool Prefix(CutsceneIntroRoot __instance, string resourceName, object asset)
        {
            if (!Main.enabled) return true;

            try
            {
                string heroName = Mod.GetHeroName(Main.currentHero);
                if (heroName.IsNullOrEmpty()) return true;

                CutsceneIntroData _curIntroData = (CutsceneIntroData)asset;
                __instance.SetFieldValue("_curIntroData", _curIntroData);

                if (_curIntroData == null)
                {
                    __instance.EndCutscene();
                    return false;
                }
                __instance.headingMesh.TextString = heroName;
                __instance.headingMesh.transform.localScale = new Vector3(_curIntroData.headingScale, _curIntroData.headingScale, _curIntroData.headingScale);
                __instance.headingMesh.UpdateText();
                if (__instance.subtitle1Mesh != null)
                {
                    if (!string.IsNullOrEmpty(_curIntroData.subtitle1))
                    {
                        __instance.subtitle1Mesh.TextString =  _curIntroData.subtitle1;
                        __instance.subtitle1Mesh.transform.localScale = new Vector3(_curIntroData.subtitleScale, _curIntroData.subtitleScale, _curIntroData.subtitleScale);
                    }
                    __instance.subtitle1Mesh.UpdateText();
                }
                if (__instance.subtitle2Mesh != null)
                {
                    if (!string.IsNullOrEmpty(_curIntroData.subtitle2))
                    {
                        __instance.subtitle2Mesh.gameObject.SetActive(true);
                        __instance.subtitle2Mesh.TextString = _curIntroData.subtitle2;
                        __instance.subtitle2Mesh.UpdateText();
                    }
                    else
                    {
                        __instance.subtitle2Mesh.gameObject.SetActive(false);
                    }
                }
                __instance.SetFieldValue("_oldTex", __instance.spriteRenderer.material.mainTexture);
                __instance.spriteRenderer.material.mainTexture = _curIntroData.spriteTexture;
                SpriteSM component = __instance.spriteRenderer.gameObject.GetComponent<SpriteSM>();
                int num = 0;
                int num2 = _curIntroData.spriteTexture.height;
                int x = _curIntroData.spriteTexture.width;
                int y = _curIntroData.spriteTexture.height;
                if (_curIntroData.spriteRect.height > 0f)
                {
                    num = (int)_curIntroData.spriteRect.x;
                    num2 = (int)_curIntroData.spriteRect.y;
                    x = (int)_curIntroData.spriteRect.width;
                    y = (int)_curIntroData.spriteRect.height;
                }
                component.SetLowerLeftPixel((float)num, (float)num2);
                component.SetPixelDimensions(x, y);
                __instance.anim.AddClip(_curIntroData.animClip, _curIntroData.animClip.name);
                __instance.anim.clip = _curIntroData.animClip;
                __instance.barkSource.clip = _curIntroData.bark;
                if (__instance.fanfareSource != null && _curIntroData.introFanfare != null)
                {
                    __instance.fanfareSource.clip = _curIntroData.introFanfare;
                }
                __instance.cutsceneRoot.SetActive(true);
                AnimatedTexture component2 = __instance.spriteRenderer.gameObject.GetComponent<AnimatedTexture>();
                if (component2 != null)
                {
                    if (_curIntroData.spriteAnimRateFramesWidth.x > 0f)
                    {
                        component2.frameRate = _curIntroData.spriteAnimRateFramesWidth.x;
                        component2.frames = (int)_curIntroData.spriteAnimRateFramesWidth.y;
                        component2.frameSpacingWidth = (int)_curIntroData.spriteAnimRateFramesWidth.z;
                        component2.enabled = true;
                        component2.Recalc();
                    }
                    else
                    {
                        component2.enabled = false;
                    }
                }
                if (_curIntroData.spriteSize.x > 0f)
                {
                    component.SetSize(_curIntroData.spriteSize.x, _curIntroData.spriteSize.y);
                }
                component.RecalcTexture();
                component.CalcUVs();
                component.UpdateUVs();

                __instance.SetFieldValue("_curIntroData", _curIntroData);

                return false;
            }
            catch (Exception ex)
            {
                Main.Log(ex);
            }
            return true;
        }

    }

    /*[HarmonyPatch(typeof(CutsceneIntroRoot), "EndCutscene")]
    static class end_Patch
    {
        static void Postfix(CutsceneIntroRoot __instance)
        {
            __instance.cutsceneRoot = null;
           // UnityEngine.Object.Destroy(__instance);
        }
    }*/
}
