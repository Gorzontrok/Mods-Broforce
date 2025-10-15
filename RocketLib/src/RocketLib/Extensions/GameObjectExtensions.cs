using System;
using System.Text;
using UnityEngine;

namespace RocketLib
{
    /// <summary>
    /// Extensions for the GameObject class.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Remove the component of type T from the current GameObject.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="self"></param>
        public static void RemoveComponent<T>(this GameObject self) where T : Component
        {
            UnityEngine.Object.Destroy(self.GetComponent<T>());
        }

        public static bool HasComponent<T>(this GameObject self) where T : Component
        {
            return self.GetComponent<T>() != null;
        }


        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            T comp = self.GetComponent<T>();
            if (comp == null)
            {
                comp = self.AddComponent<T>();
            }
            return comp;
        }

        public static T GetComponent<T>(this GameObject self, Action<T> action) where T : Component
        {
            T comp = self.GetComponent<T>();
            if (comp != null)
            {
                action(comp);
            }
            return comp;
        }

        public static GameObject FindChildOfName(this GameObject gameObject, string name)
        {
            var transform = gameObject.transform;
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.name == name)
                {
                    return child.gameObject;
                }
            }
            return null;
        }

        public static string GetPath(this GameObject self)
        {
            if (self == null)
            {
                return "NULL";
            }
            Transform transform = self.transform;
            StringBuilder stringBuilder = new StringBuilder(self.name);
            while (transform.parent != null)
            {
                transform = transform.parent;
                stringBuilder.Insert(0, new StringBuilder(transform.name).Append('/'));
            }
            stringBuilder.Insert(0, new StringBuilder(self.scene.name).Append('/'));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Sets the parent and resets localScale to Vector3.one to prevent inheriting parent's scale.
        /// </summary>
        public static void SetParentAndResetScale(this Transform transform, Transform parent, Vector3? localPosition = null)
        {
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            if (localPosition.HasValue)
            {
                transform.localPosition = localPosition.Value;
            }
        }
    }
}
