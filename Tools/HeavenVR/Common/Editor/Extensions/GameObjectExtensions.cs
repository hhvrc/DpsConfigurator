using System;
using UnityEngine;

namespace HeavenVR.Tools.Extensions
{
    internal static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent(out T component))
            {
                component = go.AddComponent<T>();
            }

            return component;
        }
        public static Component GetOrAddComponent(this GameObject go, Type type)
        {
            if (!go.TryGetComponent(type, out Component component))
            {
                component = go.AddComponent(type);
            }

            return component;
        }
    }
}
