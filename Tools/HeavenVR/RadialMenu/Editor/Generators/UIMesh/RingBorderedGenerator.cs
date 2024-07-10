using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class UIMeshGenerators
    {
        struct SegmentMeshInfo
        {
            public readonly ushort idx;
            public readonly ushort nVrts;

            public SegmentMeshInfo(ushort index, ushort nVertices)
            {
                idx = index;
                nVrts = nVertices;
            }
        }
        class RingBorderedBuilder
        {
            readonly float axRadius;
            readonly float ayRadius;
            readonly float bxRadius;
            readonly float byRadius;
            readonly float cxRadius;
            readonly float cyRadius;
            readonly float dxRadius;
            readonly float dyRadius;

            readonly float xCenter;
            readonly float yCenter;

            readonly Color32 colorBorder;
            readonly Color32 colorOuter;
            readonly Color32 colorInner;

            readonly Vertex[] vrts;
            readonly ushort[] tris;

            readonly float borderWidth;

            readonly float anglePoly;
            readonly float angleBeg;
            readonly float angleEnd;

            readonly bool fullCircle;
            readonly bool endBorder;

            public RingBorderedBuilder(Rect rect, float progressFrom, float progressTo, float ringRatio, float borderWidth, int resolution, Color32 innerColor, Color32 outerColor, Color32 borderColor, bool createEndBorder)
            {
                if (ringRatio <= 0f || ringRatio >= 1f)
                    throw new System.ArgumentOutOfRangeException("Ring ratio must be between 0 and 1");

                axRadius = rect.width * 0.5f;
                ayRadius = rect.height * 0.5f;
                bxRadius = axRadius - borderWidth;
                byRadius = ayRadius - borderWidth;
                cxRadius = axRadius * ringRatio;
                cyRadius = ayRadius * ringRatio;
                dxRadius = cxRadius - borderWidth;
                dyRadius = cyRadius - borderWidth;

                xCenter = rect.x + axRadius;
                yCenter = rect.y + ayRadius;

                colorBorder = borderColor;
                colorOuter = outerColor;
                colorInner = innerColor;

                var totalProgressAmount = progressTo - progressFrom;
                var totalProgressAngle = totalProgressAmount * MPI2;
                var nSteps = GetRequiredResolution(resolution, totalProgressAmount);

                this.borderWidth = borderWidth;

                anglePoly = totalProgressAngle / nSteps;
                angleBeg = progressFrom * MPI2;
                angleEnd = angleBeg + totalProgressAngle;

                fullCircle = Mathf.Approximately(totalProgressAmount, 1f);
                endBorder = createEndBorder;

                int nVrt = nSteps * 6;
                int nTri = nVrt * 3;
                if (!fullCircle)
                {
                    nVrt += 6; // We will need a row of vertices for the last step, whereas if it was a full circle, we would have a full row of vertices at the beginning.
                    nTri += endBorder ? 12 : 6; // +12 for the border on the start and finish of the ring (2 quads) (4 * 3)
                }

                vrts = new Vertex[nVrt];
                tris = new ushort[nTri];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void BuildVerts(int vrtIdx, float angle)
            {
                var ratioX = Mathf.Sin(angle);
                var ratioY = -Mathf.Cos(angle);

                var radiusB = new Vector3(xCenter + (ratioX * bxRadius), yCenter + (ratioY * byRadius), Vertex.nearZ);
                var radiusC = new Vector3(xCenter + (ratioX * cxRadius), yCenter + (ratioY * cyRadius), Vertex.nearZ);

                vrts[vrtIdx + 0] = new Vertex { position = new Vector3(xCenter + (ratioX * axRadius), yCenter + (ratioY * ayRadius), Vertex.nearZ), tint = colorBorder };
                vrts[vrtIdx + 1] = new Vertex { position = radiusB, tint = colorBorder };
                vrts[vrtIdx + 2] = new Vertex { position = radiusB, tint = colorOuter };
                vrts[vrtIdx + 3] = new Vertex { position = radiusC, tint = colorInner };
                vrts[vrtIdx + 4] = new Vertex { position = radiusC, tint = colorBorder };
                vrts[vrtIdx + 5] = new Vertex { position = new Vector3(xCenter + (ratioX * dxRadius), yCenter + (ratioY * dyRadius), Vertex.nearZ), tint = colorBorder };
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void StitchVerts(ushort a, ushort b)
            {
                var triIdx = a * 3;

                tris[triIdx + 0] = (ushort)(a + 0);
                tris[triIdx + 1] = (ushort)(b + 0);
                tris[triIdx + 2] = (ushort)(a + 1);

                tris[triIdx + 3] = (ushort)(a + 1);
                tris[triIdx + 4] = (ushort)(b + 0);
                tris[triIdx + 5] = (ushort)(b + 1);

                tris[triIdx + 6] = (ushort)(a + 2);
                tris[triIdx + 7] = (ushort)(b + 2);
                tris[triIdx + 8] = (ushort)(a + 3);

                tris[triIdx + 9] = (ushort)(a + 3);
                tris[triIdx + 10] = (ushort)(b + 2);
                tris[triIdx + 11] = (ushort)(b + 3);

                tris[triIdx + 12] = (ushort)(a + 4);
                tris[triIdx + 13] = (ushort)(b + 4);
                tris[triIdx + 14] = (ushort)(a + 5);

                tris[triIdx + 15] = (ushort)(a + 5);
                tris[triIdx + 16] = (ushort)(b + 4);
                tris[triIdx + 17] = (ushort)(b + 5);
            }

            void BuildEdgeBorders()
            {
                float width = borderWidth;

                var vrtIdx = vrts.Length - 3;
                var triIdx = tris.Length - 6;

                if (endBorder)
                {
                    width *= 0.5f;
                    vrtIdx -= 3;
                    triIdx -= 6;

                    float endIndentAngle = angleEnd - (0.25f * MPI2);
                    var endIndentOffset = new Vector3(width * Mathf.Sin(endIndentAngle), width * -Mathf.Cos(endIndentAngle));

                    vrts[vrtIdx + 1].position += endIndentOffset;
                    vrts[vrtIdx + 2].position += endIndentOffset;
                    vrts[vrtIdx + 3].position += endIndentOffset;
                    vrts[vrtIdx + 4].position += endIndentOffset;

                    tris[triIdx + 6] = (ushort)(vrtIdx + 0);
                    tris[triIdx + 7] = (ushort)(vrtIdx + 5);
                    tris[triIdx + 8] = (ushort)(vrtIdx + 1);
                    tris[triIdx + 9] = (ushort)(vrtIdx + 5);
                    tris[triIdx + 10] = (ushort)(vrtIdx + 4);
                    tris[triIdx + 11] = (ushort)(vrtIdx + 1);
                }

                float begIndentAngle = angleBeg + (0.25f * MPI2);
                var begIndentOffset = new Vector3(width * Mathf.Sin(begIndentAngle), width * -Mathf.Cos(begIndentAngle));

                vrts[1].position += begIndentOffset;
                vrts[2].position += begIndentOffset;
                vrts[3].position += begIndentOffset;
                vrts[4].position += begIndentOffset;

                tris[triIdx + 0] = 5;
                tris[triIdx + 1] = 0;
                tris[triIdx + 2] = 1;
                tris[triIdx + 3] = 1;
                tris[triIdx + 4] = 4;
                tris[triIdx + 5] = 5;
            }

            public UIMesh Build()
            {
                int endIdx = vrts.Length - 6;

                float angle = angleBeg;
                for (int vrtIdx = 0; vrtIdx < endIdx; vrtIdx += 6)
                {
                    BuildVerts(vrtIdx, angle);
                    StitchVerts((ushort)vrtIdx, (ushort)(vrtIdx + 6));
                    angle += anglePoly;
                }
                BuildVerts(endIdx, angle);

                if (fullCircle)
                {
                    StitchVerts((ushort)endIdx, 0);
                }
                else
                {
                    BuildEdgeBorders();
                }

                return new UIMesh(vrts, tris);
            }
        }

        public static UIMesh GenerateRingBordered(Rect rect, float progressFrom, float progressTo, float ringRatio, float borderWidth, int resolution, Color32 innerColor, Color32 outerColor, Color32 borderColor, bool createEndBorder = true)
        {
            float ringWidth = Mathf.Min(rect.width, rect.height) * ringRatio;

            if (borderWidth <= 0f)
                return GenerateRing(rect, progressFrom, progressTo, ringWidth, resolution, innerColor, outerColor);
            else if (borderWidth * 2 >= ringWidth)
                return GenerateRing(rect, progressFrom, progressTo, ringWidth, resolution, borderColor, borderColor);
            else
                return new RingBorderedBuilder(rect, progressFrom, progressTo, ringRatio, borderWidth, resolution, innerColor, outerColor, borderColor, createEndBorder).Build();
        }
        public static void RecolorRingBordered(UIMesh mesh, int idx, int nVrts, Color32 innerColor, Color32 outerColor, Color32 borderColor)
        {
            int endIdx = idx + nVrts;

            for (int vrtIdx = idx; vrtIdx < endIdx; vrtIdx += 6)
            {
                mesh.Vertices[vrtIdx + 0].tint = borderColor;
                mesh.Vertices[vrtIdx + 1].tint = borderColor;
                mesh.Vertices[vrtIdx + 2].tint = outerColor;
                mesh.Vertices[vrtIdx + 3].tint = innerColor;
                mesh.Vertices[vrtIdx + 4].tint = borderColor;
                mesh.Vertices[vrtIdx + 5].tint = borderColor;
            }
        }
    }
}
