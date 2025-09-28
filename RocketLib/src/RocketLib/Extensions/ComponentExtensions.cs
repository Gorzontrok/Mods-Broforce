using System;
using System.Collections.Generic;
using UnityEngine;

namespace RocketLib
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// Remove the component of type T from the current Component.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="self"></param>
        public static void RemoveComponent<T>(this Component self) where T : Component
        {
            self.gameObject.RemoveComponent<T>();
        }

        public static bool HasComponent<T>(this Component self) where T : Component
        {
            return self.gameObject.HasComponent<T>();
        }


        public static T GetOrAddComponent<T>(this Component self) where T : Component
        {
            return self.gameObject.GetOrAddComponent<T>();
        }

        public static T GetComponent<T>(this Component self, Action<T> action) where T : Component
        {
            return self.gameObject.GetComponent(action);
        }

        public static GameObject FindChildOfName(this Component component, string name)
        {
            var transform = component.transform;
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

        public static GameObject[] ToGameObjects<T>(this T[] components) where T : Component
        {
            List<GameObject> list = new List<GameObject>();
            foreach (var component in components)
            {
                list.Add(component.gameObject);
            }
            return list.ToArray();
        }
    }
}