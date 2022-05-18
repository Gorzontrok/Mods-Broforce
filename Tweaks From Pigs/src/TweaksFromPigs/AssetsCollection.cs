using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RocketLib0;
using UnityEngine;

namespace TweaksFromPigs
{
    public static class AssetsCollection
    {
        public static Shader grenadeShader;
        public static Shader characterShader;
        public static Material Gimp_Pig_anim
        {
            get
            {
                if(_gimp_Pig_anim == null)
                {
                   _gimp_Pig_anim = TFP_Utility.CreateMaterialFromResources("Gimp_Pig_anim", characterShader);
                }
                return _gimp_Pig_anim;
            }
        }
        private static Material _gimp_Pig_anim;

        public static Material Grenade_Tear_Gas
        {
            get
            {
                if (_grenade_Tear_Gas == null)
                {
                    _grenade_Tear_Gas = TFP_Utility.CreateMaterialFromResources("Grenade_Tear_Gas", grenadeShader);
                }
                return _grenade_Tear_Gas;
            }
        }
        private static Material _grenade_Tear_Gas;

    }
}
