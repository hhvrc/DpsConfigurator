using Assets.Tools.HeavenVR.DPS_Configurator;
using HeavenVR.Tools.DpsConfigurator.Menus;
using HeavenVR.Tools.Extensions;
using HeavenVR.Tools.Utils;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.DpsConfigurator.Components
{
    internal class ConstraintComponent : GUI.CollapsibleMenu
    {
        public static ConstraintComponent CurrentlyAdjustingMenu { get; private set; }
        public static GameObject AdjustmentHelperObject { get; private set; }
        public static GameObject AdjustmentDpsPreviewObject { get; private set; }

        public ConstraintComponent(Object constrainedPrefab, string name, Transform sourceTransform, float sourceWeight, Vector3 translationOffset, Vector3 rotationOffset)
            : base(name)
        {
            _constrainedPrefab = constrainedPrefab;
            _adjustmentScale = 1f;
            SourceTransform = sourceTransform;
            Position = translationOffset;
            Rotation = rotationOffset;
        }
        public ConstraintComponent(Object adjustmentPrefab)
            : this(adjustmentPrefab, "Constraint", null, 1f, Vector3.zero, Vector3.zero)
        {
        }
        public ConstraintComponent()
            : this(null)
        {
            _adjustmentScale = 0.1f;
        }

        public Object PenetratorTestingPrefab;
        public Object OrificeTestingPrefab;

        Object _constrainedPrefab;
        float _adjustmentScale;
        public Transform SourceTransform { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        void EnterAdjustMode()
        {
            ExitAdjustMode();
            CurrentlyAdjustingMenu = this;

            if (_constrainedPrefab == null)
            {
                AdjustmentHelperObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                AdjustmentHelperObject.transform.parent = SourceTransform;
            }
            else
            {
                AdjustmentHelperObject = (GameObject)Object.Instantiate(_constrainedPrefab, SourceTransform);
            }

            AdjustmentHelperObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave;

            AdjustmentHelperObject.transform.localScale = Vector3.one * _adjustmentScale;
            AdjustmentHelperObject.transform.localPosition = Position;
            AdjustmentHelperObject.transform.localRotation = Quaternion.Euler(Rotation);

            Selection.objects = new[] { AdjustmentHelperObject };

            Transform parent = AdjustmentHelperObject.transform;
            if (Application.isPlaying)
            {
                Object prefab = null;
                if (_constrainedPrefab == null || _constrainedPrefab.name.Contains("[Orifice]"))
                {
                    prefab = OrificeTestingPrefab;
                }
                else if (_constrainedPrefab.name.Contains("[Penetrator]"))
                {
                    prefab = PenetratorTestingPrefab;
                }

                if (prefab != null)
                {
                    var gimbal = parent.AddChild("Gimbal");
                    AdjustmentDpsPreviewObject = gimbal.gameObject;
                    var dpsObj = (GameObject)Object.Instantiate(prefab, gimbal);
                    var preview = dpsObj.AddComponent<DpsPreview>();
                }
            }
        }
        public void ExitAdjustMode()
        {
            CurrentlyAdjustingMenu = null;
            if (AdjustmentHelperObject != null)
            {
                GameObject.DestroyImmediate(AdjustmentHelperObject);
            }
            AdjustmentHelperObject = null;
            Selection.objects = new GameObject[0];
        }

        override protected void DrawMenuContents()
        {
            MenuName = EditorGUILayout.TextField("Constraint Name", MenuName);
            SourceTransform = (Transform)EditorGUILayout.ObjectField("Source Transform", SourceTransform, typeof(Transform), true);
            Position = EditorGUILayout.Vector3Field("Position", Position);
            Rotation = EditorGUILayout.Vector3Field("Rotation", Rotation);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 15);
            if (CurrentlyAdjustingMenu == this)
            {
                bool isDone = GUILayout.Button("Done") || AdjustmentHelperObject == null;
                _adjustmentScale = EditorGUILayout.FloatField("Preview Scale", _adjustmentScale);

                if (isDone)
                {
                    ExitAdjustMode();
                }
                else
                {
                    if (AdjustmentHelperObject.transform.parent != SourceTransform)
                    {
                        AdjustmentHelperObject.transform.parent = SourceTransform;
                        AdjustmentHelperObject.transform.localPosition = Position;
                        AdjustmentHelperObject.transform.localRotation = Quaternion.Euler(Rotation);
                    }
                    else
                    {
                        Position = AdjustmentHelperObject.transform.localPosition;
                        Rotation = AdjustmentHelperObject.transform.localRotation.eulerAngles;
                    }

                    AdjustmentHelperObject.transform.localScale = Vector3.one * _adjustmentScale;
                }
            }
            else
            {
                if (GUILayout.Button("Adjust"))
                {
                    EnterAdjustMode();
                }
                if (GUILayout.Button("Test (Enter Playmode)"))
                {
                    MainMenu.Instance?.ConfigSave();
                    // TODO: set state cache to activate test mode in playmode
                    EditorApplication.isPlaying = true;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        protected override void CleanupMenuContents()
        {
            ExitAdjustMode();
        }

        protected override JObject SerializeMenuContents()
        {
            return new JObject
            {
                ["$adjustmentScale"] = _adjustmentScale,
                ["sourceTransform"] = Utils.TransformUtils.GetPath(SourceTransform, MainMenu.SelectedAvatar?.transform),
                ["position"] = JsonUtils.Serialize(Position),
                ["rotation"] = JsonUtils.Serialize(Rotation)
            };
        }
        protected override void DeserializeMenuContents(JToken json)
        {
            _adjustmentScale = json["$adjustmentScale"].Value<float>();
            SourceTransform = Utils.TransformUtils.GetFromPath(json["sourceTransform"]?.Value<string>(), MainMenu.SelectedAvatar?.transform);
            Position = JsonUtils.DeserializeVector3(json["position"]);
            Rotation = JsonUtils.DeserializeVector3(json["rotation"]);
        }
    }
}
