using System;
using UnityEngine;

public static class SpriteSMExtensions
{
    public static void SetTexture(this SpriteSM self, Texture2D texture)
    {
        if (texture == null) throw new ArgumentNullException("texture");
        self.meshRender.sharedMaterial.mainTexture = texture;
    }
    public static void SetTexture(this SpriteSM self, Texture texture)
    {
        if (texture == null) throw new ArgumentNullException("texture");
        self.meshRender.sharedMaterial.mainTexture = texture;
    }
    public static Texture GetTexture(this SpriteSM self)
    {
        return self.meshRender.sharedMaterial.mainTexture;
    }

}
