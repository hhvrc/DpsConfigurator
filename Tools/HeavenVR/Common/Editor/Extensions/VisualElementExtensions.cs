using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.Extensions
{
    public static class VisualElementExtensions
    {
        public static T With<T>(this T element, VisualElement child) where T : VisualElement
        {
            element.Add(child);
            return element;
        }

        public static void SizeSet<T>(this T element, float x, float y) where T : VisualElement
        {
            element.style.maxWidth = element.style.minWidth = element.style.width = x;
            element.style.maxHeight = element.style.minHeight = element.style.height = y;
        }
        public static void SizeSet<T>(this T element, float size) where T : VisualElement => SizeSet(element, size, size);
        public static void SizeSet<T>(this T element, Vector2 size) where T : VisualElement => SizeSet(element, size.x, size.y);
        public static Vector2 SizeGet<T>(this T element) where T : VisualElement =>
            new Vector2(element.style.width.value.value, element.style.height.value.value);

        public static void PositionSet<T>(this T element, float x, float y) where T : VisualElement
        {
            element.style.position = Position.Absolute;
            element.style.left = x;
            element.style.top = y;
        }
        public static void PositionSet<T>(this T element, Vector2 pos) where T : VisualElement => PositionSet(element, pos.x, pos.y);
        public static Vector2 PositionGet<T>(this T element) where T : VisualElement =>
            new Vector2(element.style.left.value.value, element.style.top.value.value);


        public static void PositionSetCenter<T>(this T element, float x, float y) where T : VisualElement => PositionSet(element, x - (element.style.width.value.value * 0.5f), y - (element.style.height.value.value * 0.5f));
        public static void PositionSetCenter<T>(this T element, Vector2 pos) where T : VisualElement => PositionSetCenter(element, pos.x, pos.y);
        public static Vector2 PositionGetCenter<T>(this T element) where T : VisualElement =>
            PositionGet(element) + (SizeGet(element) * 0.5f);

        const double Deg2Rad = Math.PI / 180.0;
        public static void RotationSet<T>(this T element, float rotation) where T : VisualElement
        {
            double angle = rotation * Deg2Rad;
            var center = SizeGet(element) * 0.5f;

            double nCenterY = -center.y;
            double ncenterX = -center.x;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            element.transform.position = new Vector3(
                (float)((cos * ncenterX) - (sin * nCenterY)) + center.x,
                (float)((sin * ncenterX) + (cos * nCenterY)) + center.y
                );
            element.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}
