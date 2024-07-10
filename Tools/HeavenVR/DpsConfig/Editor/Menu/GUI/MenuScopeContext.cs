using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.GUI
{
    internal static class MenuScopeContext
    {
        static Stack<string> _currentMenuPath = new Stack<string>();
        static Stack<IComponent> _currentMenuScope = new Stack<IComponent>();

        public static void Push(string name, IComponent menu)
        {
            _currentMenuPath.Push(name);
            _currentMenuScope.Push(menu);
        }
        public static void Pop()
        {
            if (_currentMenuScope.Count > 0)
            {
                _currentMenuPath.Pop();
                _currentMenuScope.Pop();
            }
        }
        public static bool Any()
        {
            return _currentMenuScope.Count > 0;
        }
        public static string CurrentMenuPath() => "/" + string.Join("/", _currentMenuPath.Reverse());
        public static void DrawGUI()
        {
            if (_currentMenuScope.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Path: " + CurrentMenuPath());
                bool pop = GUILayout.Button("X", GUILayout.Width(20));
                EditorGUILayout.EndHorizontal();
                
                if (pop)
                {
                    Pop();
                }
                else
                {
                    _currentMenuScope.Peek().DrawGUI();
                }
            }
        }
    }
}
