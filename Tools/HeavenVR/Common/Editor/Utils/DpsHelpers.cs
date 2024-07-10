using HeavenVR.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;

namespace HeavenVR.Tools.DpsConfigurator
{
    internal static class DpsHelpers
    {
        enum DpsLightType
        {
            Tracker,
            NormalTracker,
            PasstroughTracker,
        }
        static void SetupDpsLight(Light light, DpsLightType dpsLightType)
        {
            light.type = LightType.Point;
            light.color = new Color(0f, 0f, 0f, 1f);
            light.intensity = 1f;
            light.bounceIntensity = 1f;
            light.shadows = LightShadows.None;
            light.renderMode = LightRenderMode.ForceVertex;

            switch (dpsLightType)
            {
                case DpsLightType.Tracker:
                    light.range = 0.41f;
                    break;
                case DpsLightType.NormalTracker:
                    light.range = 0.45f;
                    break;
                case DpsLightType.PasstroughTracker:
                    light.range = 0.42f;
                    break;
                default:
                    break;
            }
        }
        public static GameObject CreateDpsOrifice(string name, Transform parent, bool passtroughOrifice)
        {
            // Create orifice
            var orifice = parent.AddChild(name);
            orifice.localPosition = Vector3.zero;
            orifice.localRotation = Quaternion.identity;
            orifice.localScale = Vector3.one;

            // Create tracking lights
            var dpsTracker = orifice.AddChild("tracker", typeof(Light));
            var dpsNormalTracker = orifice.AddChild("normalTracker", typeof(Light));

            // Set normal tracking light position
            dpsNormalTracker.localPosition = new Vector3(0f, -0.01f, 0f);

            // Set up tracking lights
            SetupDpsLight(dpsTracker.GetComponent<Light>(), DpsLightType.Tracker);
            SetupDpsLight(dpsNormalTracker.GetComponent<Light>(), passtroughOrifice ? DpsLightType.PasstroughTracker : DpsLightType.NormalTracker);

            return orifice.gameObject;
        }
    }
}
