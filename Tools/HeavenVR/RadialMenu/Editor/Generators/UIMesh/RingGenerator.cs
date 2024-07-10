using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class UIMeshGenerators
    {
        public static UIMesh GenerateRing(Rect rect, float progressFrom, float progressTo, float ringWidth, int resolution, Color32 innerColor, Color32 outerColor)
        {
            var xRadius = rect.width * 0.5f;
            var yRadius = rect.height * 0.5f;
            var xCenter = rect.x + xRadius;
            var yCenter = rect.y + yRadius;

            var xRadiusInner = xRadius - ringWidth;
            var yRadiusInner = yRadius - ringWidth;

            var progressAmount = progressTo - progressFrom;
            var numSteps = GetRequiredResolution(resolution, progressAmount);
            var polyAngle = progressAmount * MPI2 / numSteps;
            var startAngle = progressFrom * MPI2;

            var numPolys = numSteps * 2;
            var vertices = new Vertex[numPolys + 2];
            var indices = new ushort[numPolys * 3];

            var xAngle = Mathf.Sin(startAngle);
            var yAngle = -Mathf.Cos(startAngle);
            var borderPos = AnglePos(xCenter, yCenter, xRadiusInner, yRadiusInner, xAngle, yAngle);
            var outerPos = AnglePos(xCenter, yCenter, xRadius, yRadius, xAngle, yAngle);

            vertices[0] = new Vertex { position = borderPos, tint = innerColor };
            vertices[1] = new Vertex { position = outerPos, tint = outerColor };
            for (int i = 0; i < numSteps; i++)
            {
                var angle = (i + 1) * polyAngle;
                xAngle = Mathf.Sin(angle);
                yAngle = -Mathf.Cos(angle);

                borderPos.x = xCenter + (xAngle * xRadiusInner);
                borderPos.y = yCenter + (yAngle * yRadiusInner);
                outerPos.x = xCenter + (xAngle * xRadius);
                outerPos.y = yCenter + (yAngle * yRadius);

                var i2 = i * 2;
                vertices[i2 + 2] = new Vertex { position = borderPos, tint = innerColor };
                vertices[i2 + 3] = new Vertex { position = outerPos, tint = outerColor };

                var i6 = i * 6;
                indices[i6 + 0] = (ushort)(i2 + 0);
                indices[i6 + 1] = (ushort)(i2 + 1);
                indices[i6 + 2] = (ushort)(i2 + 2);
                indices[i6 + 3] = (ushort)(i2 + 1);
                indices[i6 + 4] = (ushort)(i2 + 3);
                indices[i6 + 5] = (ushort)(i2 + 2);
            }

            return new UIMesh(vertices, indices);
        }
        public static UIMesh GenerateRing(Rect rect, float progress, float ringWidth, int resolution, Color32 innerColor, Color32 outerColor) => GenerateRing(rect, 0f, progress, ringWidth, resolution, innerColor, outerColor);
        public static UIMesh GenerateRing(Rect rect, float progress, float ringWidth, int resolution, Color32 color) => GenerateRing(rect, 0f, progress, ringWidth, resolution, color, color);
        public static UIMesh GenerateRing(Rect rect, float ringWidth, int resolution, Color32 innerColor, Color32 outerColor) => GenerateRing(rect, 0f, 1f, ringWidth, resolution, innerColor, outerColor);
        public static UIMesh GenerateRing(Rect rect, float ringWidth, int resolution, Color32 color) => GenerateRing(rect, 0f, 1f, ringWidth, resolution, color, color);
    }
}
