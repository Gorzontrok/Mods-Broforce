using RocketLib;
using System;
using UnityEngine;

public static class TestVanDammeAnimExtensions
{
    public static BroBase AsBroBase(this TestVanDammeAnim self)
    {
        return self as BroBase;
    }

    #region Fields
    public static SpriteSM Sprite(this TestVanDammeAnim self)
    {
        return self.GetFieldValue<SpriteSM>("sprite");
    }
    public static Sound Sound(this TestVanDammeAnim self)
    {
        return self.GetFieldValue<Sound>("sound");
    }
    #endregion

    public static TwoInt GetSpriteSize(this TestVanDammeAnim self)
    {
        return new TwoInt(self.GetFieldValue<int>("spritePixelWidth"), self.GetFieldValue<int>("spritePixelHeight"));
    }

    public static void SetSpriteLowerLeftPixel(this TestVanDammeAnim self, int x, int y)
    {
        var size = self.GetSpriteSize();
        self.Sprite().SetLowerLeftPixel(x * size.x, y * size.y);
    }

    public static void SetRendererTexture<T>(this T anim, Texture texture) where T : TestVanDammeAnim
    {
        anim.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", texture);
    }

    public static void SetRendererMaterial<T>(this T anim, Material mat) where T : TestVanDammeAnim
    {
        anim.GetComponent<Renderer>().sharedMaterial = mat;
    }
}
