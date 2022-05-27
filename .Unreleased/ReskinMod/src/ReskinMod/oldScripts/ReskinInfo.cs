using System;
using UnityEngine;
using RocketLib0;
using ReskinMod;

namespace ReskinMod
{
    /*// Never instantiate SpriteSM, the bro will not disappear when it should.
    internal class ReskinInfo
    {
        public ReskinInfo()
        {
        }
        public ReskinInfo(string _Name, string _Folder, string _SecondMatName)
        {
            this.Name = _Name;
            this.Folder = _Folder;
            this.SecondMatName = _SecondMatName;
        }

        public TestVanDammeAnim instance = null;

        public Texture CharacterTex = null;
        protected Texture CharacterTex2 = null;

        public Texture GunTex = null;
        protected Texture GunTex2 = null;


        protected string Folder = string.Empty;
        protected string Name = string.Empty;
        protected string SecondMatName = string.Empty;

        protected bool IsSecondMaterial = false;

        protected string Character_ImagePath
        {
            get
            {
                return this.Folder + this.Name + (this.IsSecondMaterial ? this.SecondMatName : "") + "_anim.png";
            }
        }
        protected string Gun_ImagePath
        {
            get
            {
                return this.Folder + this.Name + (this.IsSecondMaterial ? this.SecondMatName : "") + "_gun_anim.png";
            }
        }

        public virtual void CheckTex()
        {
            try
            {
                SpriteSM sprite = this.instance.gameObject.GetComponent<SpriteSM>();

                if (this.CharacterTex == null && sprite != null)
                {
                    this.CharacterTex = Utility.GetTextureFromPath(this.Character_ImagePath, Material.Instantiate(sprite.meshRender.sharedMaterial));
                }
                if (this.GunTex == null && this.instance.gunSprite != null)
                {
                    this.GunTex = Utility.GetTextureFromPath(this.Gun_ImagePath, Material.Instantiate(this.instance.gunSprite.meshRender.sharedMaterial));
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed to check Texture of {this.instance.GetType()}.", ex);
            }
        }

        public virtual void CheckSecondTex(Material cMat)
        {
            CheckSecondTex(cMat, null);
        }
        public virtual void CheckSecondTex(Material cMat, Material gMat)
        {
            try
            {
                this.IsSecondMaterial = true;
                if (cMat != null)
                {
                    this.CharacterTex2 = Utility.GetTextureFromPath(this.Character_ImagePath, Material.Instantiate(cMat));
                }
                if (gMat != null)
                {
                    this.GunTex2 = Utility.GetTextureFromPath(this.Gun_ImagePath, Material.Instantiate(gMat));
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed to Check Second Texture of {this.instance.GetType()}", ex);
            }
            this.IsSecondMaterial = false;
        }

        public virtual TestVanDammeAnim ApplyReskin(TestVanDammeAnim instance)
        {
            try
            {
                if (this.CharacterTex != null)
                {
                    SpriteSM sprite = instance.gameObject.GetComponent<SpriteSM>();
                    sprite.meshRender.sharedMaterial.SetTexture("_MainTex", this.CharacterTex);
                }
                if (this.GunTex != null)
                {
                    instance.gunSprite.GetComponent<Renderer>().material.mainTexture = this.GunTex;
                    instance.gunSprite.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", this.GunTex);
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed to apply the Re-skin of {this.instance.GetType()}.", ex);
            }
            return instance;
        }

        public virtual Texture GetCharacterTex2(Material Mat)
        {
            try
            {
                if (this.CharacterTex2 != null)
                {
                    return this.CharacterTex2;
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed To Get Second Character Texture of {this.instance.GetType()}.", ex);
            }
            return Mat.mainTexture;
        }
        public virtual Texture GetGunTex2(Material Mat)
        {
            try
            {
                if (this.GunTex2 != null)
                {
                    return this.GunTex2;
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed To Get Second Gun Texture of {this.instance.GetType()}.", ex);
            }
            return Mat.mainTexture;
        }
    }

    internal class RI_Mook : ReskinInfo
    {
        public RI_Mook()
        {
            this.Folder = Utility.Mook_Folder;
        }

        protected Texture DecapitatedTex = null;

        public virtual void CheckTex(Mook i)
        {
            try
            {
                this.instance = i;

                if (string.IsNullOrEmpty(this.Name))
                {
                    this.Name = this.GetMookName(i);
                }
                if (string.IsNullOrEmpty(this.SecondMatName))
                {
                    this.SecondMatName = this.GetMookSecondMaterial(this.Name);
                }
                SpriteSM sprite = i.gameObject.GetComponent<SpriteSM>();

                if (this.CharacterTex == null && sprite != null)
                {
                    this.CharacterTex = Utility.GetTextureFromPath(this.Character_ImagePath, Material.Instantiate(sprite.meshRender.sharedMaterial));
                }
                if (!this.HasNoGun(i) && this.GunTex == null && i.gunSprite.meshRender.sharedMaterial != null)
                {
                    this.GunTex = Utility.GetTextureFromPath(this.Gun_ImagePath, Material.Instantiate(i.gunSprite.meshRender.sharedMaterial));
                }

                if (this.HasDecapitatedSkin(i) && this.DecapitatedTex == null)
                {
                    this.DecapitatedTex = GetDecapitatedTexture(i);
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed to check Texture with: {i.GetType()}.", ex);
            }
        }


        public virtual Texture GetDecapitated()
        {
            try
            {
                if (this.DecapitatedTex != null)
                {
                    return this.DecapitatedTex;
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed To Get Decapitated Texture of {this.instance.GetType()}.", ex);
            }
            return this.CharacterTex;
        }

        private bool HasNoGun(Mook i)
        {
            return (i as Satan) || (i as Warlock) || (i as MookGrenadier);
        }

        private string GetMookName(Mook mook)
        {
            MookType mookType = mook.GetMookType();

            switch (mookType)
            {
                case MookType.Trooper:
                    if (mook as SkinnedMook) return "Skinless";
                    else if (mook as MookJetpack) return "Mook_Jetpack";
                    else return "Mook";
                case MookType.Devil:
                    return "Satan";
                case MookType.Vehicle:
                    return "";
                case MookType.RiotShield:
                    return "Shield";
                case MookType.Boomer:
                    if (mook as MookHellSoulCatcher) return "Hell_Soul_Catcher";
                    else return "Hell_Boomer";
                case MookType.BigGuy:
                    if (mook as MookHellBigGuy) return "Hell_Big_Guy";
                    else if (mook as MookHellArmouredBigGuy) return "Hell_Armoured_Boomer";
                    else return "Big_Guy";
                case MookType.None:
                    if (mook as HellLostSoul) return "Hell_LostSoul";
                    else return "";
                case MookType.Alien:
                    AlienXenomorph alien = mook as AlienXenomorph;
                    if ((mook as AlienXenomorph) && (alien.hasBrainBox)) return "Alien_Xenomorph_Brainbox";
                    else if (mook as AlienBrute) return "Alien_Brute";
                    else return "Alien_Xenomorph";
                case MookType.Melter:
                    if (mook as AlienMosquito) return "Alien_Mosquitto";
                    else return "Alien_Melter";
                default:
                    return mook.GetMookType().ToString();
            }
        }

        private string GetMookSecondMaterial(string MookName)
        {
            switch (MookName)
            {
                case "Hell_Boomer":
                case "Hell_Soul_Catcher":
                case "UndeadSuicide":
                case "Suicide":
                    return "_warning";
                case "Shield":
                case "Scout":
                    return "_disarmed";
                case "Dog":
                    return "_upgraded";
            }
            return "";
        }

        private bool HasDecapitatedSkin(Mook mook)
        {
            return (mook as MookTrooper) || (mook as MookSuicide) || (mook as MookBigGuy) || (mook as SkinnedMook);
        }

        private Texture GetDecapitatedTexture(Mook CurrentMook)
        {
            string ImagePath = this.Folder + this.Name + "_anim_decapitated.png";
            if (CurrentMook as MookTrooper)
            {
                MookTrooper mook = CurrentMook as MookTrooper;
                if (CurrentMook as MookBazooka)
                {
                    mook = CurrentMook as MookBazooka;
                }
                else if (CurrentMook as MookJetpack)
                {
                    mook = CurrentMook as MookJetpack;
                }
                else if (CurrentMook as UndeadTrooper)
                {
                    mook = CurrentMook as UndeadTrooper;
                }
                return Utility.GetTextureFromPath(ImagePath, Material.Instantiate(mook.decapitatedMaterial));
            }
            else if (CurrentMook as MookSuicide)
            {
                MookSuicide mook = CurrentMook as MookSuicide;
                return Utility.GetTextureFromPath(ImagePath, Material.Instantiate(mook.decapitatedMaterial));
            }
            else if (CurrentMook as MookBigGuy)
            {
                MookBigGuy mook = CurrentMook as MookBigGuy;
                if (!(CurrentMook as MookArmouredGuy))
                {
                    if (CurrentMook as MookHellBigGuy)
                    {
                        mook = CurrentMook as MookHellBigGuy;
                    }
                    else if (CurrentMook as MookHellArmouredBigGuy)
                    {
                        mook = CurrentMook as MookHellArmouredBigGuy;
                    }
                    return Utility.GetTextureFromPath(ImagePath, Material.Instantiate(mook.decapitatedMaterial));
                }
            }
            else if (CurrentMook as SkinnedMook)
            {
                SkinnedMook mook = CurrentMook as SkinnedMook;
                return Utility.GetTextureFromPath(ImagePath, Material.Instantiate(mook.decapitatedMaterial));
            }
            SpriteSM sprite = CurrentMook.gameObject.GetComponent<SpriteSM>();
            return sprite.meshRender.sharedMaterial.mainTexture;
        }
    }


    internal class RI_Bro : ReskinInfo
    {
        public RI_Bro()
        {
            this.Folder = Utility.Bro_Folder;
        }

        public Texture AvatarTex = null;
        protected Texture AvatarTex2 = null;

        protected HeroType heroType = HeroType.None;

        protected string Avatar_ImagePath
        {
            get
            {
                return Utility.HUD_Folder + "avatar_" + this.Name + (this.IsSecondMaterial ? this.SecondMatName : "") + ".png";
            }
        }

        public override TestVanDammeAnim ApplyReskin(TestVanDammeAnim instance)
        {
            try
            {
                instance = base.ApplyReskin(instance);
                if (this.AvatarTex != null)
                {
                    instance.player.hud.avatar.meshRender.sharedMaterial.SetTexture("_MainTex", this.AvatarTex);
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed to Check Second Texture of {instance.GetType()}", ex);
            }
            return instance;
        }
        public virtual void CheckTex(HeroType h, TestVanDammeAnim i)
        {
            try
            {
                this.IsSecondMaterial = false;
                this.instance = i;
                if (this.heroType == HeroType.None)
                {
                    this.heroType = h;
                }

                if (string.IsNullOrEmpty(this.Name))
                {
                    this.Name = this.GetBroName(this.heroType);
                }
                if (string.IsNullOrEmpty(this.SecondMatName))
                {
                    this.SecondMatName = this.GetBroSecondMaterial(this.heroType);
                }

                base.CheckTex();

                if (this.AvatarTex == null && this.instance.player.hud.avatar.meshRender.sharedMaterial != null)
                {
                    this.AvatarTex = Utility.GetTextureFromPath(this.Avatar_ImagePath, Material.Instantiate(this.instance.player.hud.avatar.meshRender.sharedMaterial));
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed to check Texture of {this.instance.GetType()}.", ex);
            }
        }

        public virtual void CheckAvatarTex2(Material aMat)
        {
            try
            {
                this.IsSecondMaterial = true;
                this.AvatarTex2 = Utility.GetTextureFromPath(this.Avatar_ImagePath, Material.Instantiate(aMat));
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed to check the Second Avatar Texture of {this.instance.GetType()}.", ex);
            }
            this.IsSecondMaterial = false;
        }

        public virtual Texture GetAvatarTex2(Material Mat)
        {
            try
            {
                if (this.AvatarTex2 != null)
                {
                    return this.AvatarTex2;
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog($"Failed To Get Second Avatar Texture of {this.instance.GetType()}.", ex);
            }
            return Mat.mainTexture;
        }

        private string GetBroName(HeroType hero)
        {
            switch (hero)
            {
                case HeroType.Blade:
                    return "Brade";
                case HeroType.Brononymous:
                    return "BroInBlack";
                case HeroType.BroveHeart:
                    return "BroHeart";
                case HeroType.DirtyHarry:
                    return "DirtyBrorry";
                case HeroType.Nebro:
                    return "NeoBro";
                case HeroType.Predabro:
                    return "TheBrodator";
                case HeroType.TimeBroVanDamme:
                    return "TimeBro";
                case HeroType.HaleTheBro:
                    return "BroCaesar";
                case HeroType.DoubleBroSeven:
                    return "007";
                default:
                    return hero.ToString();
            }
        }
        private string GetBroSecondMaterial(HeroType hero)
        {
            switch (hero)
            {
                case HeroType.Predabro:
                    return "_stealth";
                case HeroType.Brominator:
                    return "_metal";
                case HeroType.DoubleBroSeven:
                    return "_balaclava";
                case HeroType.IndianaBrones:
                case HeroType.Nebro:
                    return "_armless";
                case HeroType.AshBrolliams:
                    return "_bloody";
            }
            return "";
        }
    }*/
}
