﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs.HPatch.Projectiles
{
    [HarmonyPatch(typeof(GrenadePrimaryFire), "Launch")]
    class d
    {
        static void Prefix()
        {
            //Main.bmod.InformationLog("ddddddd");
        }
    }
}
