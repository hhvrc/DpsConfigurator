using System;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.DpsConf
{
    public static class VRCIcons
    {
        static Texture2D CreateTexture(int height, int width, Color32 color)
        {
            if (height < 1 || width < 1)
                throw new ArgumentException("Height and width must be greater than 0");

            var pixels = new Color32[width * height];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels32(pixels);
            texture.Apply();

            return texture;
        }

        const string ResourcePath = "Assets/Tools/HeavenVR/DpsConfig/Resources/";

        public static class RadialMenu
        {
            const string ResourcePath = VRCIcons.ResourcePath + "RadialMenu/";

            public static class Icons
            {
                const string ResourcePath = RadialMenu.ResourcePath + "Icons/";

                public static readonly Texture2D Hud = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "HUD.png");
                public static readonly Texture2D Back = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "back.png");
                public static readonly Texture2D Playing = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "playing.png");
                public static readonly Texture2D Default = Hud; // TODO: Change me
            }
            public static class SubIcons
            {
                const string ResourcePath = RadialMenu.ResourcePath + "SubIcons/";

                public static readonly Texture2D Axis = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "axis.png");
                public static readonly Texture2D Radial = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "radial.png");
                public static readonly Texture2D Folder = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "folder.png");
                public static readonly Texture2D ToggleOn = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "toggle_on.png");
                public static readonly Texture2D ToggleOff = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "toggle_off.png");
                public static readonly Texture2D PlayOn = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "play_on.png");
                public static readonly Texture2D PlayOff = (Texture2D)EditorGUIUtility.LoadRequired(ResourcePath + "play_off.png");
            }
        }

        public static readonly Texture2D Transparent = CreateTexture(1, 1, new Color32(0, 0, 0, 0));
    }
}