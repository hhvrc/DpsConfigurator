using HeavenVR.Tools.GUI;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.DpsConfigurator.Menus
{
    internal class ParametersMenu : IComponent
    {
        public ParametersMenu(string name)
        {
            Name = name;
            Icon = null;
            Color = Color.white;
            Submenus = new List<ParametersMenu>();
            Parameters = new List<ParametersMenu>();
        }

        public string Name { get; set; }
        public Texture2D Icon { get; set; }
        public Color Color { get; set; }
        public List<ParametersMenu> Submenus { get; set; }
        public List<ParametersMenu> Parameters { get; set; }

        bool _submenusExpanded = false;
        bool _parametersExpanded = false;

        public void DrawGUI()
        {
            Name = EditorGUILayout.TextField("Name", Name);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Icon");
            Icon = (Texture2D)EditorGUILayout.ObjectField(Icon, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();
            Color = EditorGUILayout.ColorField("Color", Color);

            if (_submenusExpanded = CustomGUILayout.BeginFoldout("Submenus", _submenusExpanded))
            {
                for (int i = 0; i < Submenus.Count; i++)
                {
                    var submenu = Submenus[i];
                    EditorGUILayout.BeginHorizontal();
                    bool enterSubmenu = GUILayout.Button(submenu.Name);
                    bool removeSubmenu = GUILayout.Button("-", GUILayout.Width(20));
                    EditorGUILayout.EndHorizontal();

                    if (enterSubmenu)
                    {
                        MenuScopeContext.Push(submenu.Name, submenu);
                        return;
                    }
                    else if (removeSubmenu)
                    {
                        Submenus.RemoveAt(i--);
                    }
                }
                if (GUILayout.Button("+")) Submenus.Add(new ParametersMenu("New menu"));
            }
            CustomGUILayout.EndFoldout();
            if (_parametersExpanded = CustomGUILayout.BeginFoldout("Parameters", _parametersExpanded))
            {
                for (int i = 0; i < Parameters.Count; i++)
                {
                    var parameter = Parameters[i];
                    EditorGUILayout.BeginHorizontal();
                    bool enterParameter = GUILayout.Button(parameter.Name);
                    bool removeParameter = GUILayout.Button("-", GUILayout.Width(20));
                    EditorGUILayout.EndHorizontal();

                    if (enterParameter)
                    {
                        MenuScopeContext.Push(parameter.Name, parameter);
                        return;
                    }
                    else if (removeParameter)
                    {
                        Parameters.RemoveAt(i--);
                    }
                }
                if (GUILayout.Button("+")) Parameters.Add(new ParametersMenu("New parameter"));
            }
        }
        void DrawMenu()
        {

        }

        public void Cleanup()
        {
        }

        public JObject ToJson()
        {
            var json = new JObject();



            return json;
        }

        public void FromJson(JToken json)
        {
        }
    }
}
