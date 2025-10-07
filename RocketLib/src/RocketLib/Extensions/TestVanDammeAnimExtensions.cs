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

    public static Vector2 GetSpriteSize(this TestVanDammeAnim self)
    {
        return new Vector2(self.GetFieldValue<int>("spritePixelWidth"), self.GetFieldValue<int>("spritePixelHeight"));
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

    public static bool IsOnAnimal<T>(this T anim) where T : TestVanDammeAnim
    {
        LayerMask platformLayer = anim.GetFieldValue<LayerMask>("platformLayer");
        RaycastHit raycastHit;
        return (Physics.Raycast(new Vector3(anim.X, anim.Y + 5f, 0f), Vector3.down, out raycastHit, 16f, platformLayer) ||
            Physics.Raycast(new Vector3(anim.X + 4f, anim.Y + 5f, 0f), Vector3.down, out raycastHit, 16f, platformLayer) ||
            Physics.Raycast(new Vector3(anim.X - 4f, anim.Y + 5f, 0f), Vector3.down, out raycastHit, 16f, platformLayer)) &&
            raycastHit.collider.GetComponentInParent<Animal>() != null;
    }

    public static void SetReviveSource(this TestVanDammeAnim testVanDammeAnim, TestVanDammeAnim reviveSource)
    {
        testVanDammeAnim.SetFieldValue("reviveSource", reviveSource);
    }
}
