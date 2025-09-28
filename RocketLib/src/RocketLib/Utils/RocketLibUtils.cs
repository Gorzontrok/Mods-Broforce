using UnityEngine;

namespace RocketLib.Utils
{
    public static class RocketLibUtils
    {
        public static void MakeObjectUnpausable(string gameObjectName)
        {
            MakeObjectUnpausable(GameObject.Find(gameObjectName));
        }

        public static void MakeObjectUnpausable(GameObject gameObject)
        {
            if (gameObject != null)
            {
                gameObject.tag = "Unpausable";
            }
        }
    }
}
