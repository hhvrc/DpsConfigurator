using HeavenVR.DpsConf.Extensions;
using HeavenVR.DpsConf.Generators;
using UnityEngine;
using CursorColors = HeavenVR.DpsConf.VRCColors.RadialCursor;

namespace HeavenVR.DpsConf.CustomElements.RadialMenu
{
    public class RadialCursorElement : CircleElement
    {
        private static readonly float DefaultCursorSize = RadialMenuElement.MenuDiameter * 0.212f;

        public RadialCursorElement()
            : base(4f, CursorColors.Fill, CursorColors.Rings)
        {
            this.SizeSet(DefaultCursorSize);
            
            AddMesh("c1", () => UIMeshGenerators.GenerateRing(contentRect.SubRect(0.6666f), 4f, 32, CursorColors.Rings));
            AddMesh("c2", () => UIMeshGenerators.GenerateRing(contentRect.SubRect(0.3333f), 4f, 32, CursorColors.Rings));
        }
    }
}
