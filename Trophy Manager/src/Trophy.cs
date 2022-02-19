using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TrophyManager
{
    public class Trophy
    {
        public static Texture2D errorTexture;
        public static Texture2D hideTexture;
        public static Texture2D achieveFrame;

        public readonly string Name;
        public readonly string Description;
        public readonly int Objective;
        public int Progression { get; private set; }
        public Texture2D TrophyTex { get; private set; }
        public bool Achieved { get; private set; }

        private Texture2D __trophyTextureDone;

        // Constructor
        public Trophy(string name, int objective, int progression)
        {
            new Trophy(name, objective, progression, "NO_DESC", hideTexture);
        }
        public Trophy(string name, int objective, int progression, string description)
        {
            new Trophy(name, objective, progression, description, hideTexture);
        }
        public Trophy(string name, int objective, int progression, Texture2D achieveTexture)
        {
            new Trophy(name, objective, progression, "NO_DESC", achieveTexture);
        }
        public Trophy(string name, int objective, int progression, string description, Texture2D achieveTexture)
        {
            this.Name = name;
            this.Objective = objective;
            this.Description = (!string.IsNullOrEmpty(description)) ? description : "NO_DESC";
            this.TrophyTex = (achieveTexture != null && !Main.settings.LockedTrophyDontHaveImage) ? achieveTexture : CreateTex2d(hideTexture);
            __trophyTextureDone = (achieveTexture != null) ? achieveTexture : CreateTex2d(hideTexture);
            this.Progression = progression;
            TrophyController.AddTrophy(this);
            CheckIsDone();
        }

        public void CheckIsDone()
        {
            if (!Achieved)
            {
                Achieved = Progression >= Objective;
                if (Achieved)
                {
                    AddAchieveFrame();
                    Main.Log("'" + Name + "' trophy is done !");
                    TrophyShower.AddRedeem(this);
                }
            }
        }

        public int IntToShow()
        {
            return this.Progression >= Objective ? Objective : Progression;
        }

        public void UpdateProgression(int Progression)
        {
            if (!Achieved)
            {
                this.Progression = Progression;
                CheckIsDone();
            }
        }

        public void Reset()
        {
            Progression = 0;
            Achieved = false;
        }

        private void AddAchieveFrame()
        {
            if (!Achieved) return;
            try
            {
                for (int x = 0; x < __trophyTextureDone.width; x++)
                {

                    for (int y = 0; y < __trophyTextureDone.height; y++)
                    {
                        Color trophyColor = __trophyTextureDone.GetPixel(x, y);
                        Color achieveFrameColor = achieveFrame.GetPixel(x, y);

                        if (achieveFrameColor != Color.clear)
                        {
                            Color final_color = Color.Lerp(trophyColor, achieveFrameColor, achieveFrameColor.a / 1.0f);
                            __trophyTextureDone.SetPixel(x, y, final_color);
                        }
                    }
                }
                __trophyTextureDone.Apply();
                TrophyTex = __trophyTextureDone;
            }
            catch (Exception ex)
            {
                Main.Log(ex);
                TrophyTex = errorTexture;
            }
        }

        private Texture2D CreateTex2d(Texture2D texture)
        {
            Texture2D tex = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
            tex.anisoLevel = texture.anisoLevel;
            tex.filterMode = texture.filterMode;
            tex.mipMapBias = texture.mipMapBias;
            tex.wrapMode = texture.wrapMode;
            tex.SetPixels32(texture.GetPixels32(tex.mipmapCount));
            tex.SetPixels(texture.GetPixels());
            tex.Apply();
            return tex;
        }
    }
}
