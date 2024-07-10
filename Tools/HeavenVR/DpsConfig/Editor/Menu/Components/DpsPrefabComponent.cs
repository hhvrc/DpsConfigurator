using HeavenVR.Tools.Utils;
using System;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.DpsConfigurator.Components
{
    internal class DpsPrefabComponent : GUI.CollapsibleMenu
    {        
        public DpsPrefabComponent()
            : this(null)
        {
        }
        public DpsPrefabComponent(UnityEngine.Object prefab)
            : base(prefab == null ? "Uninitialized" : prefab.name)
        {
            Prefab = prefab;
            Constraints = new GUI.ComponentList<ConstraintComponent>("Constraints", () => new ConstraintComponent(Prefab));
        }

        UnityEngine.Object _prefab;
        public UnityEngine.Object Prefab { get => _prefab; private set { _prefab = value; Preview = PrefabUtils.RenderPrefabPreview(Prefab, 256, 256); MenuName = value.name; } }
        public Texture2D Preview { get; private set; }
        public GUI.ComponentList<ConstraintComponent> Constraints { get; }

        override protected void DrawMenuContents()
        {
            EditorGUILayout.BeginHorizontal();

            if (Preview == null)
                Preview = PrefabUtils.RenderPrefabPreview(Prefab, 256, 256);

            Rect textureRect = EditorGUILayout.GetControlRect(GUILayout.Height(128));
            textureRect.x += 15 * EditorGUI.indentLevel;
            textureRect.width = 128;
            UnityEngine.GUI.DrawTexture(textureRect, Preview);

            EditorGUILayout.BeginVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            Constraints.DrawGUI();
        }

        protected override void CleanupMenuContents()
        {
            Constraints.Cleanup();
        }

        protected override JObject SerializeMenuContents()
        {
            if (Constraints.Items.Count == 0)
                return null;

            return new JObject
            {
                ["prefabGuid"] = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Prefab)),
                ["constraints"] = Constraints.ToJson()
            };
        }
        protected override void DeserializeMenuContents(JToken json)
        {
            if (json == null)
                return;

            Prefab = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(json["prefabGuid"].Value<string>()));
            Constraints.FromJson(json["constraints"]);
        }
    }
}
