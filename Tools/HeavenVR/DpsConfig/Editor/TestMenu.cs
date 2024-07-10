using HeavenVR.DpsConf.CustomElements.RadialMenu;
using HeavenVR.DpsConf.CustomElements.RadialMenu.Items;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf
{
    public class TestMenu : EditorWindow
    {
        [MenuItem("Window/My Window")]
        static void Init()
        {
            var window = GetWindow<TestMenu>();
            window.titleContent = new GUIContent("Custom editor");
            window.Show();
        }

        private void CreateGUI()
        {
            UIElementsEntryPoint.SetAntiAliasing(this, 8);
            // minSize = minSize; // BlackStartX ❤️ (Needed to force AA if it doesnt automatically apply)


            var container = new VisualElement() { style = { alignItems = Align.Center, justifyContent = Justify.FlexStart } };

            container.Add(new Label("Radial menu") { pickingMode = PickingMode.Ignore, style = { unityTextAlign = TextAnchor.MiddleCenter } });

            var circle = new RadialMenuElement() { style = { top = 20, left = 20 } };
            container.Add(circle);

            rootVisualElement.Add(container);


            circle.Items = new List<RadialMenuItemElement>()
            {
                new BackButtonItem(),
                new ButtonItem("<b>2: Button</b>", VRCIcons.RadialMenu.Icons.Default),
                new ToggleItem("<b>3: Toggle</b>", VRCIcons.RadialMenu.Icons.Default),
                new ToggleItem("<b>4: Toggle</b>", VRCIcons.RadialMenu.Icons.Default),
                new ToggleItem("<b>5: Toggle</b>", VRCIcons.RadialMenu.Icons.Default),
                new ToggleItem("<b>6: Toggle</b>", VRCIcons.RadialMenu.Icons.Default),
                new ToggleItem("<b>7: Toggle</b>", VRCIcons.RadialMenu.Icons.Default),
                new ToggleItem("<b>8: Toggle</b>", VRCIcons.RadialMenu.Icons.Default)
            };
        }

        private void OnInspectorUpdate()
        {
            Helpers.OnInspectorUpdate(null, null);
        }
    }
}
