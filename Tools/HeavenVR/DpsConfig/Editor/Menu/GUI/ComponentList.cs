using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.GUI
{
    internal class ComponentList<T> : IComponent where T : IComponent
    {
        public delegate void DrawModButtons(ComponentList<T> menu);
        public static void DefaultModButtons(ComponentList<T> menu)
        {
            if (GUILayout.Button("+")) menu.Items.Add(menu._createComponent());
            if (GUILayout.Button("-") && menu.Items.Count > 0)
            {
                int index = menu.Items.Count - 1;
                menu.Items[index]?.Cleanup();
                menu.Items.RemoveAt(index);
            }
        }
        public static T DefaultCreateComponent()
        {
            return Activator.CreateInstance<T>();
        }

        public ComponentList(string name, DrawModButtons drawModButtons)
        {
            Name = name;
            Items = new List<T>();
            _drawModButtons = drawModButtons;
        }
        public ComponentList(string name, Func<T> createComponent)
        {
            Name = name;
            Items = new List<T>();
            _drawModButtons = DefaultModButtons;
            _createComponent = createComponent;
        }
        public ComponentList(string name) : this(name, DefaultCreateComponent) { }

        public string Name { get; set; }
        public List<T> Items { get; set; }

        readonly DrawModButtons _drawModButtons;
        readonly Func<T> _createComponent;

        public void DrawGUI()
        {
            // Indent the menu
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 15);
            EditorGUILayout.BeginVertical();

            // Store and reset the indent level
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Header
            EditorGUILayout.BeginHorizontal("button");
            EditorGUILayout.LabelField(Name, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            _drawModButtons(this);
            EditorGUILayout.EndHorizontal();

            // List
            EditorGUILayout.BeginVertical("box");
            foreach (var item in Items)
                item.DrawGUI();
            EditorGUILayout.EndVertical();

            // Restore original indentation level
            EditorGUI.indentLevel = indentLevel;

            // Exit menu indentation
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        public void Cleanup()
        {
            foreach (var item in Items)
                item.Cleanup();
        }

        public void Update(IEnumerable<T> newItems, Func<T, T, bool> comparer)
        {
            var toAdd = newItems.Where(item => !Items.Any(oldItem => comparer(oldItem, item)));
            var toKeep = Items.Where(item => newItems.Any(newItem => comparer(item, newItem)));

            Items = toAdd.Concat(toKeep).ToList();
        }

        public JObject ToJson()
        {
            var jitems = new JArray();
            foreach (var item in Items)
            {
                var jitem = item.ToJson();

                if (jitem != null)
                    jitems.Add(jitem);
            }

            if (jitems.Count == 0)
                return null;

            return new JObject
            {
                ["$name"] = Name,
                ["items"] = jitems
            };
        }

        public void FromJson(JToken json)
        {
            if (json == null)
                return;

            Name = json["$name"].ToString();
            Items.Clear();
            foreach (var jitem in json["items"])
            {
                var item = DefaultCreateComponent();
                item.FromJson(jitem);
                Items.Add(item);
            }
        }
    }
}
