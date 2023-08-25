using System;
using UnityEngine;
namespace BroforceOverhaul.Mooks
{
    public static class MookController
    {
        public static bool CanBeTeargased(Mook mook)
        {
            return !(mook as Alien) && mook.mookType != MookType.Vehicle;
        }

        public static bool CanBeStrungUp(TestVanDammeAnim testVanDammeAnim)
        {
            return testVanDammeAnim as Mook && !testVanDammeAnim.IsHeavy();
        }

        public static Material GetSkinlessMaterial(TestVanDammeAnim testVanDammeAnim)
        {
            Mook mook = testVanDammeAnim as Mook;
            if (mook)
            {
                if(mook as MookGeneral)
                {
                    return ResourcesController.GetMaterialResource("Skinless.mookGeneral_skinless.png", ResourcesController.Unlit_DepthCutout);
                }
                else if(mook as MookRiotShield || mook as MookSuicide || mook as MookTrooper || mook as ScoutMook)
                {
                    if(mook is UndeadTrooper)
                    {
                        return ResourcesController.GetMaterialResource("Skinless.mookUndead_skinless.png", ResourcesController.Unlit_DepthCutout);
                    }
                    else if (mook as MookJetpack)
                    {
                        return ResourcesController.GetMaterialResource("Skinless.mookJetpack_skinless.png", ResourcesController.Unlit_DepthCutout);
                    }
                    else
                    {
                        return ResourcesController.GetMaterialResource("Skinless.mook_skinless.png", ResourcesController.Unlit_DepthCutout);
                    }
                }
            }
            return null;
        }
    }
}

