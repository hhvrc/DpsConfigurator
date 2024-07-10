using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.Utils
{
    internal static class PrefabUtils
    {
        public static IEnumerable<GameObject> FindAndLoadPrefabs(string searchQuery)
        {
            return AssetDatabase.FindAssets(searchQuery + " t:prefab")
                    .Select(g => AssetDatabase.GUIDToAssetPath(g))
                    .Select(p => AssetDatabase.LoadAssetAtPath<GameObject>(p));
        }
        public static Texture2D RenderPrefabPreview(Object prefab, int width, int height)
        {
            if (prefab == null)
            {
                return TextureUtils.CreateTexture(width, height, (Color32)Color.red);
            }

            var editor = Editor.CreateEditor(prefab);
            var tex = editor.RenderStaticPreview(AssetDatabase.GetAssetPath(prefab), null, width, height);
            Object.DestroyImmediate(editor);

            return tex;
        }
    }
}
