using HeavenVR.DpsConf.Extensions;
using UnityEngine;
using RadialColors = HeavenVR.DpsConf.VRCColors.RadialMenu;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class UIMeshGenerators
    {
        class MeshInfo
        {
            public int idx;
            public readonly SegmentMeshInfo[] segments;

            public MeshInfo(int index, SegmentMeshInfo[] segments)
            {
                idx = index;
                this.segments = segments;
            }
        }
        public static (UIMesh mesh, object context) GenerateRadialMenu(Rect rect, int nSegments, float innerRatio, int resolution, int selectedSegment = -1)
        {
            var mesh = GenerateCircle(rect.SubRect(innerRatio), resolution, RadialColors.Center);

            float segmentAngle = 1f / nSegments;
            float angle = segmentAngle * -0.5f;

            var submeshInfo = new SegmentMeshInfo[nSegments];
            for (int i = 0; i < nSegments; i++)
            {
                var subMesh = GenerateRingBordered(rect, angle, angle + segmentAngle, innerRatio, 3f, resolution, RadialColors.Inner, i == selectedSegment ? RadialColors.OuterSelected : RadialColors.Outer, RadialColors.Border, false);
                angle += segmentAngle;

                submeshInfo[i] = new SegmentMeshInfo(mesh.AddMesh(subMesh), subMesh.VertexCount);
            }

            return (mesh, new MeshInfo(selectedSegment, submeshInfo));
        }
        public static void RecolorRadialMenu(UIMesh mesh, object meshCtx, int selectedSegment)
        {
            if (meshCtx == null)
                return;

            var meshInfo = (MeshInfo)meshCtx;
            var lastSegment = meshInfo.idx;
            if (lastSegment != selectedSegment)
            {
                meshInfo.idx = selectedSegment;

                if (lastSegment >= 0)
                {
                    var oldSegment = meshInfo.segments[lastSegment];
                    RecolorRingBordered(mesh, oldSegment.idx, oldSegment.nVrts, RadialColors.Inner, RadialColors.Outer, RadialColors.Border);
                }

                if (selectedSegment >= 0)
                {
                    var newSegment = meshInfo.segments[selectedSegment];
                    RecolorRingBordered(mesh, newSegment.idx, newSegment.nVrts, RadialColors.Inner, RadialColors.OuterSelected, RadialColors.Border);
                }
            }
        }
    }
}
