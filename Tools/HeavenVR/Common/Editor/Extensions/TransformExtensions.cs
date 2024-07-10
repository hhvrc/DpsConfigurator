using System;
using System.Linq;
using UnityEngine;

namespace HeavenVR.Tools.Extensions
{
    internal static class TransformExtensions
    {
        public static Transform GetParent(this Transform transform, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (transform == null)
                    return null;

                transform = transform.parent;
            }
            return transform;
        }
        public static Transform AddChild(this Transform transform, string name, params Type[] components)
        {
            var child = new GameObject(name, components).transform;
            child.SetParent(transform, false);

            return child;
        }
        public static Transform GetOrAddChild(this Transform transform, string name, params Type[] components)
        {
            var child = transform.Find(name);

            if (child == null)
            {
                child = transform.AddChild(name, components);
            }
            else
            {
                var childObject = child.gameObject;
                foreach (var component in components)
                    childObject.GetOrAddComponent(component);
            }

            return child;
        }
        public static Transform[] GetAllChildren(this Transform transform)
        {
            return transform.Cast<Transform>().ToArray();
        }
        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            return transform.gameObject.GetOrAddComponent<T>();
        }
        public static Component GetOrAddComponent(this Transform transform, Type type)
        {
            return transform.gameObject.GetOrAddComponent(type);
        }
    }
}
