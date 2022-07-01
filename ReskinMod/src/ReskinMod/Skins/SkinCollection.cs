using System;
using System.Collections.Generic;

namespace ReskinMod.Skins
{
    public class SkinCollection
    {
        public static List<SkinCollection> villagerSkinCollections = new List<SkinCollection>();
        public static List<SkinCollection> citizenSkinCollections = new List<SkinCollection>();

        public readonly string name;
        public readonly int skinCollectionNumber;
        public List<Skin> skins = new List<Skin>();

        public SkinCollection(string n, int i)
        {
            name = n;
            skinCollectionNumber = i;
        }

        public bool AddNewSkin(string path)
        {
            Skin skin = null;
            try
            {
                skin = new Skin(path);
            }
            catch(Exception ex)
            {
                Main.ErrorLog(ex);
            }

            if(skin == null || skin.skinType == SkinType.None || skin.texture == null)
            {
                string msg = $"Failed Create skin for the file '{path}'";
                SkinCollectionController.conflictsAndErrors.Add("<color=\"yellow\">" + msg + "</color>");
                Main.ErrorLog(msg);
                return false;
            }
            else
            {
                Skin skin1 = GetSkin(skin.skinType, skin.skinNumber);
                if (skin1 != null)
                {
                    string msg = $"File conflict :\t{skin.path}\n\t{skin1.path}\nSecond file has been choses";
                    SkinCollectionController.conflictsAndErrors.Add("<color=\"red\">" + msg + "</color>");
                    Main.WarningLog(msg);
                    return false;
                }
                else
                {
                    skins.Add(skin);
                    return true;
                }
            }
        }

        public Skin GetSkin(SkinType skinType, int skinNumber)
        {
            foreach(Skin skin in skins)
            {
                if(skin.skinType == skinType && skin.skinNumber == skinNumber)
                {
                    return skin;
                }
            }
            return null;
        }
    }
}
