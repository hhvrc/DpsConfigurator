using HeavenVR.Tools.DpsConfigurator;
using HeavenVR.Tools.Utils;
using HeavenVR.Tools.DpsConfigurator.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Tools.HeavenVR.DPS_Configurator.Scripts.Editor.Generators
{
    internal class DpsGenerator
    {
        static Transform InstanciateDpsPrefab(Transform parent, UnityEngine.Object dpsPrefab)
        {
            var dpsComponent = UnityEngine.Object.Instantiate((GameObject)dpsPrefab, parent).transform;
            dpsComponent.name = dpsPrefab.name;

            dpsComponent.gameObject.SetActive(false);
            return dpsComponent;
        }
        static Transform InstanciateDpsGeneric(Transform parent, string genericName)
        {
            Transform dpsComponent;
            switch (genericName)
            {
                case "Orifice Ring":
                    dpsComponent = DpsHelpers.CreateDpsOrifice(genericName, parent, true).transform;
                    break;
                case "Orifice Hole":
                    dpsComponent = DpsHelpers.CreateDpsOrifice(genericName, parent, false).transform;
                    break;
                default:
                    throw new ArgumentException($"Unknown generic name: {genericName}");
            };

            dpsComponent.gameObject.SetActive(false);
            return dpsComponent;
        }
        static void CleanPreviousDpsEntries(Transform dpsTransform)
        {
            // Delete all children that are not the id
            foreach (Transform child in dpsTransform)
            {
                if (!child.name.StartsWith("__id_"))
                {
                    UnityEngine.Object.DestroyImmediate(child.gameObject);
                }
            }
        }
        static IEnumerable<ParentConstraintUtils.Constraint> GetConstaints(IEnumerable<ConstraintComponent> constraintMenus)
        {
            return constraintMenus.Where(i => i.SourceTransform != null).Select(m => new ParentConstraintUtils.Constraint(0f, m.SourceTransform, m.Position, m.Rotation));
        }
    }
}
