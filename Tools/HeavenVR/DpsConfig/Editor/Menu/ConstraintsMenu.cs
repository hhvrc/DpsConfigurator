using HeavenVR.Tools.GUI;
using HeavenVR.Tools.Utils;
using System;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace HeavenVR.Tools.DpsConfigurator.Menus
{
    public class ConstraintsMenu : IComponent
    {
        static void RefreshDpsMenu(string dpsPrefix, ComponentList<Components.DpsPrefabComponent> menu)
        {
            menu.Update(PrefabUtils.FindAndLoadPrefabs(dpsPrefix).Select(go => new Components.DpsPrefabComponent(go)), (a, b) => a.Prefab == b.Prefab);
        }
        static void RefreshDpsMenuButton(string dpsPrefix, ComponentList<Components.DpsPrefabComponent> menu)
        {
            if (!GUILayout.Button("Refresh")) return;
            RefreshDpsMenu(dpsPrefix, menu);
        }
        static void RefreshGenerics(ComponentList<Components.DpsGenericComponent> menu)
        {
            if (!menu.Items.Exists(g => g.MenuName == "Orifice Ring"))
                menu.Items.Add(new Components.DpsGenericComponent("Orifice Ring"));
            if (!menu.Items.Exists(g => g.MenuName == "Orifice Hole"))
                menu.Items.Add(new Components.DpsGenericComponent("Orifice Hole"));
        }
        static void RefreshPenetrators(ComponentList<Components.DpsPrefabComponent> menu) => RefreshDpsMenu("[TPS][Penetrator]", menu);
        static void RefreshPenetratorsButton(ComponentList<Components.DpsPrefabComponent> menu) => RefreshDpsMenuButton("[TPS][Penetrator]", menu);
        static void RefreshOrifices(ComponentList<Components.DpsPrefabComponent> menu) => RefreshDpsMenu("[TPS][Orifice]", menu);
        static void RefreshOrificesButton(ComponentList<Components.DpsPrefabComponent> menu) => RefreshDpsMenuButton("[TPS][Orifice]", menu);

        static readonly ComponentList<Components.DpsGenericComponent> genericComponents = new ComponentList<Components.DpsGenericComponent>("Orifice Generics", RefreshGenerics);
        static readonly ComponentList<Components.DpsPrefabComponent> penetratorComponents = new ComponentList<Components.DpsPrefabComponent>("Penetrator Prefabs", RefreshPenetratorsButton);
        static readonly ComponentList<Components.DpsPrefabComponent> orificeComponents = new ComponentList<Components.DpsPrefabComponent>("Orifice Prefabs", RefreshOrificesButton);

        public void DrawGUI()
        {
            genericComponents.DrawGUI();
            penetratorComponents.DrawGUI();
            orificeComponents.DrawGUI();
        }

        public void Cleanup()
        {
            throw new NotImplementedException();
        }

        public JObject ToJson()
        {
            var json = new JObject();

            JsonUtils.AssignIfNotNull(json, "generics", genericComponents);
            JsonUtils.AssignIfNotNull(json, "penetrators", penetratorComponents);
            JsonUtils.AssignIfNotNull(json, "orifices", orificeComponents);

            return json;
        }

        public void FromJson(JToken json)
        {
            genericComponents.FromJson(json["generics"]);
            penetratorComponents.FromJson(json["penetrators"]);
            orificeComponents.FromJson(json["orifices"]);

            RefreshGenerics(genericComponents);
            RefreshPenetrators(penetratorComponents);
            RefreshOrifices(orificeComponents);
        }
    }
}