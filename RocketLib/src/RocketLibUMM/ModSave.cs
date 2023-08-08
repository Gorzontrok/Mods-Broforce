using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RocketLib;

namespace RocketLibUMM
{
    public class ModSave
    {
        public static string SavePath
        {
            get
            {
                return Path.Combine(Main.mod.Path, "Save.json");
            }
        }
        /// <summary>
        /// Military Salute
        /// </summary>
        public KeyBindingForPlayers gesture0;
        /// <summary>
        /// Waving
        /// </summary>
        public KeyBindingForPlayers gesture1;
        /// <summary>
        /// Point Direction
        /// </summary>
        public KeyBindingForPlayers gesture2;
        /// <summary>
        /// Thrust
        /// </summary>
        public KeyBindingForPlayers gesture3;
        /// <summary>
        /// Knee Drop
        /// </summary>
        public KeyBindingForPlayers gesture4;
        /// <summary>
        /// Shhh
        /// </summary>
        public KeyBindingForPlayers gesture5;
        /// <summary>
        /// Sing (Unused)
        /// </summary>
        public KeyBindingForPlayers gesture6;


        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }

        public static ModSave Load()
        {
            if(File.Exists(SavePath))
                return JsonConvert.DeserializeObject<ModSave>(File.ReadAllText(SavePath));
            return new ModSave();
        }

        public void Initialize()
        {
            try
            {
                if (gesture0 == null)
                    gesture0 = new KeyBindingForPlayers(Main.mod.Info.Id, "Gesture 0");
                if (gesture1 == null)
                    gesture1 = new KeyBindingForPlayers(Main.mod.Info.Id, "Gesture 1");
                if (gesture2 == null)
                    gesture2 = new KeyBindingForPlayers(Main.mod.Info.Id, "Gesture 2");
                if (gesture3 == null)
                    gesture3 = new KeyBindingForPlayers(Main.mod.Info.Id, "Gesture 3");
                if (gesture4 == null)
                    gesture4 = new KeyBindingForPlayers(Main.mod.Info.Id, "Gesture 4");
                if (gesture5 == null)
                    gesture5 = new KeyBindingForPlayers(Main.mod.Info.Id, "Gesture 5");
                if (gesture6 == null)
                    gesture6 = new KeyBindingForPlayers(Main.mod.Info.Id, "Gesture 6");


                gesture0.Init(Main.mod.Info.Id);
                gesture1.Init(Main.mod.Info.Id);
                gesture2.Init(Main.mod.Info.Id);
                gesture3.Init(Main.mod.Info.Id);
                gesture4.Init(Main.mod.Info.Id);
                gesture5.Init(Main.mod.Info.Id);
                gesture6.Init(Main.mod.Info.Id);
            }
            catch(Exception e)
            {
                Main.Log(e);
            }
        }
    }
}
