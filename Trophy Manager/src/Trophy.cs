using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TrophyManager
{
    public class Trophy
    {
        // Attribut
        private string __Name = string.Empty;
        private string __Description = string.Empty;
        private string __ImagePath = string.Empty;
        private string __ImagePathDone = string.Empty;
        private bool __IsDone = false;
        private int __Objective = 0;
        private int __Progression = 0;

        private string ImagePath_Missing = Main.trophyFolderPath + "imgMissing.png";
        private string ImagePath_MissingDone = Main.trophyFolderPath + "m_imgMissing.png";

        private string ImagePath_Error = Main.trophyFolderPath + "error.png";

        private bool SayImageIsMissing = false;

        public int Progression
        {
            get
            {
                return this.__Progression;
            }
        }
        public string Name
        {
            get
            {
                return this.__Name;
            }
        }
        public string Description
        {
            get
            {
                return this.__Description;
            }
        }
        public string ImagePath
        {
            get
            {
                this.CheckIsDone();
                if (this.__IsDone)
                {
                    return this.__ImagePathDone;
                }
                return this.__ImagePath;
            }
        }
        public Texture ImageTex
        {
            get
            {
                return GetImageTexture();
            }
        }
        public bool IsDone
        {
            get
            {
                this.CheckIsDone();
                return this.__IsDone;
            }
        }
        public int Objective
        {
            get
            {
                return this.__Objective;
            }
        }

        // Constructor
        public Trophy(string Name, int Objective)
        {
            new Trophy(Name, Objective, "NO_DESC", ImagePath_Missing, ImagePath_MissingDone);
        }
        public Trophy(string Name, int Objective, string Description)
        {
            new Trophy(Name, Objective, Description, ImagePath_Missing, ImagePath_MissingDone);
        }
        public Trophy(int Objective, string Name, string ImagePath)
        {
            new Trophy(Name, Objective, "NO_DESC", ImagePath, ImagePath_MissingDone);
        }
        public Trophy(string Name, string ImagePathDone, int Objective)
        {
            new Trophy(Name, Objective, "NO_DESC", ImagePath_Missing, ImagePathDone);
        }
        public Trophy(string Name, int Objective, string Description, string ImagePath)
        {
            new Trophy(Name, Objective, Description, ImagePath, ImagePath_MissingDone);
        }
        public Trophy(string Name, string Description, string ImagePathDone, int Objective)
        {
            new Trophy(Name, Objective, Description, ImagePath, ImagePathDone);
        }
        public Trophy(string Name, string ImagePath, int Objective,  string ImagePathDone)
        {
            new Trophy(Name, Objective, "NO_DESC", ImagePath, ImagePathDone);
        }
        public Trophy(string Name, int Objective, string Description, string ImagePath, string ImagePathDone)
        {
            this.__Name = Name;
            this.__Objective = Objective;
            this.__Description = (!string.IsNullOrEmpty(Description))? Description : "NO_DESC";
            this.__ImagePath = (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath) && !Main.settings.LockTrophyDontHaveImage)? ImagePath : ImagePath_Missing;
            this.__ImagePathDone = (!string.IsNullOrEmpty(ImagePathDone) && File.Exists(ImagePathDone)) ? ImagePathDone : ImagePath_MissingDone;
            this.__Progression = 0;
            TrophyController.AddTrophy(this);
        }

        // Method
        public void CheckIsDone()
        {
            if (this.__IsDone) return;

            this.__IsDone = this.Progression >= Objective;
            if(this.__IsDone)
            {
                Main.Log("'" + this.__Name + "' trophy is done !");
                TrophyShower.AddRedeem(this.ImageTex, this.__Name);
            }
        }

        public int IntToShow()
        {
            return this.Progression >= Objective ? Objective : Progression;
        }

        public void UpdateProgression(int Progression)
        {
            this.__Progression = Progression;
            TrophyController.UpdateTrophyValue(this);
        }

        private Texture GetImageTexture()
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            try
            {
                tex.LoadImage(File.ReadAllBytes(this.__ImagePath));
                if(this.__IsDone)
                    tex.LoadImage(File.ReadAllBytes(this.__ImagePathDone));
            }
            catch (Exception ex)
            {
                tex.LoadImage(File.ReadAllBytes(this.ImagePath_Error));
                if(!this.SayImageIsMissing)
                {
                    Main.Log(this.__Name + "\n" + ex);
                    this.SayImageIsMissing = true;
                }
            }
            return tex;
        }

        public void Reset()
        {
            this.__Progression = 0;
            this.__IsDone = false;
            TrophyController.UpdateTrophyValue(this);
        }
    }
}
