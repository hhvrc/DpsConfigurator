using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.GUI
{
    internal static class CustomGUILayout
    {
        static bool _foldoutActive = false;
        static readonly Stack<bool> _foldoutStateStack = new Stack<bool>();
        public static bool BeginFoldout(string name, bool expanded)
        {
            if (_foldoutActive)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            _foldoutActive = true;

            if (expanded = EditorGUILayout.BeginFoldoutHeaderGroup(expanded, name ?? "null"))
            {
                EditorGUI.indentLevel++;
            }

            _foldoutStateStack.Push(expanded);

            return expanded;
        }
        public static void EndFoldout()
        {
            if (_foldoutStateStack.Pop())
            {
                EditorGUI.indentLevel--;
            }

            if (_foldoutActive)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                _foldoutActive = false;
            }
        }

        public static T ObjectSelectionField<T>(string name, T obj, bool allowSceneObjects) where T : UnityEngine.Object
        {
            GUILayout.BeginHorizontal();
            try
            {
                // Get selected object in inspector
                T selection = (T)EditorGUILayout.ObjectField(name, obj, typeof(T), allowSceneObjects);
                if (selection != obj)
                {
                    return selection;
                }

                // Get selected object in scene
                selection = Selection.activeGameObject?.GetComponent<T>();

                bool wasEnabled = UnityEngine.GUI.enabled;
                UnityEngine.GUI.enabled = selection != null;
                try
                {
                    // Display selection button
                    if (GUILayout.Button("Select from scene", GUILayout.Width(140)))
                    {
                        return selection;
                    }
                }
                finally
                {
                    UnityEngine.GUI.enabled = wasEnabled;
                }
            }
            finally
            {
                GUILayout.EndHorizontal();
            }

            return obj;
        }
    }
}
