using HeavenVR.DpsConf.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class VisualElementGenerators
    {
        public static VisualElement CreateIconText(float x, float y, float size, Texture2D icon, string text)
        {
            return new VisualElement
            {
                pickingMode = PickingMode.Ignore,
                style =
                {
                    width = size,
                    height = size,
                    left = x - (size * 0.5f),
                    top = y - (size * 0.5f),
                    backgroundImage = icon,
                    position = Position.Absolute
                }
            }
            .With(new TextElement
            {
                text = text,
                pickingMode = PickingMode.Ignore,
                style =
                {
                    color = Color.white,
                    fontSize = 8,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    top = size
                }
            }
            );
        }
    }
}
