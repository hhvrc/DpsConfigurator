using UnityEngine;

namespace HeavenVR.DpsConf
{
    public static class VRCColors
    {
        public static class RadialMenu
        {
            public static readonly Color32 Center = new Color(0.2196f, 0.2392f, 0.2706f, 1f);
            public static readonly Color32 Inner = new Color(0.062f, 0.368f, 0.376f, 1f);
            public static readonly Color32 Outer = new Color(0.094f, 0.203f, 0.203f, 1f);
            public static readonly Color32 OuterSelected = new Color(0.050f, 0.639f, 0.666f, 1f);
            public static readonly Color32 Border = new Color(0.0627f, 0.4314f, 0.4314f, 1f);
        }
        public static class RadialCursor
        {
            public static readonly Color32 Fill = new Color(0f, 0.1823f, 0.1863f, 0.5f);
            public static readonly Color32 Rings = new Color(0f, 0.3921f, 0.3921f, 1f);
        }
        public static class RadialMenuSegment
        {
            public static readonly Color SubIcon = new Color(0.22f, 0.24f, 0.27f);
        }
    }
}