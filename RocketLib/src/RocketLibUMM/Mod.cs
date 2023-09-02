using RocketLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityModManagerNet;

namespace RocketLibUMM
{
    public static class Mod
    {
        public static Settings Setting
        {
            get { return Main.settings; }
        }

        public static ModSave save;


        public static void Load()
        {
            save = ModSave.Load();
            save.Initialize();
            ModUI.Initialize();
        }

        public static void Update()
        {
            CheckGesture(save.gesture0, GestureElement.Gestures.Salute);
            CheckGesture(save.gesture1, GestureElement.Gestures.Wave);
            CheckGesture(save.gesture2, GestureElement.Gestures.Point);
            CheckGesture(save.gesture3, GestureElement.Gestures.Thrust);
            CheckGesture(save.gesture4, GestureElement.Gestures.KneeDrop);
            CheckGesture(save.gesture5, GestureElement.Gestures.Shhh);
        }

        // Free lives made an error so shhh and sing are invert
        private static void CheckGesture(KeyBindingForPlayers bindings, GestureElement.Gestures gesture)
        {
            for (int i = 0; i < 3; i++)
            {
                if (bindings.IsDown(i) && HeroController.PlayerIsAlive(i))
                {
                    var character = HeroController.players[i].character;
                    character.SetGestureAnimation(gesture);
                }
            }
        }
    }

}
