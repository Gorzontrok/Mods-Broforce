using System.Collections;
using UnityEngine;

namespace DoubleBroSevenTrained
{
    public class Trained007 : MonoBehaviour
    {
        public Texture originalTex;

        public void SetBalaclava(PlayerHUD hud, Texture avatar)
        {
            if (avatar == null)
                return;

            originalTex = hud.avatar.meshRender.material.mainTexture;
            hud.avatar.meshRender.material.mainTexture = avatar;
        }

        public void RemoveBalaclava(PlayerHUD hud)
        {
            if (originalTex == null)
                return;
            hud.avatar.meshRender.material.mainTexture = originalTex;
        }
    }
}