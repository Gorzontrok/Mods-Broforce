using TFBGames.Systems;
using UnityEngine;
using System.IO;

namespace DoubleBroSevenTrained
{
    public class Mod
    {
        public static Mod Instance { get; private set; }

        public static ModConfigs ModSettings
        {
            get { return Main.settings.modConfigs; }
        }

        public static VanillaConfigs VanillaSettings
        {
            get { return Main.settings.vanillaConfigs; }
        }

        public static bool Enabled
        {
            get { return Main.enabled; }
        }

        public static Material TearGasMaterial
        {
            get
            {
                if (_tearGasMaterial == null)
                    _tearGasMaterial = GameSystems.ResourceManager.LoadAssetSync<Material>("sharedtextures:GrenadeTearGas");
                return _tearGasMaterial;
            }
            set
            {
                _tearGasMaterial = value;
            }
        }
        private static Material _tearGasMaterial;

        public static Texture AvatarBalaclavaTexture
        {
            get
            {
                if (_avatarBalaclavaTexture == null)
                    _avatarBalaclavaTexture = LoadTexture("avatar_balaclava.png");
                return _avatarBalaclavaTexture;
            }
            set
            {
                _avatarBalaclavaTexture = value;
            }
        }
        private static Texture _avatarBalaclavaTexture;

        public Mod()
        {
            Instance = this;
        }

        private static Texture2D LoadTexture(string name)
        {
            string path = Path.Combine(Main.mod.Path, name);
            Texture2D result = null;
            if (File.Exists(path))
            {
                result = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                result.LoadImage(File.ReadAllBytes(path));
                result.filterMode = FilterMode.Point;
                result.Apply();
            }
            return result;
        }

    }
}
