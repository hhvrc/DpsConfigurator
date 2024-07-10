using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class UIMeshGenerators
    {
        public static UIMesh GenerateCircleBordered(Rect rect, float progressFrom, float progressTo, float borderWidth, int resolution, Color32 innerColor, Color32 outerColor, Color32 borderColor)
        {
            var xRadius = rect.width / 2f;
            var yRadius = rect.height / 2f;
            var xCenter = rect.x + xRadius;
            var yCenter = rect.y + yRadius;

            var progressAmount = progressTo - progressFrom;
            var numSteps = GetRequiredResolution(resolution, progressAmount);
            var polyAngle = progressAmount * MPI2 / numSteps;
            var startAngle = progressFrom * MPI2;

            var numPolys = numSteps * 3;
            var vertices = new Vertex[numPolys + 4];
            var indices = new ushort[numPolys * 3];

            var xAngle = Mathf.Sin(startAngle);
            var yAngle = -Mathf.Cos(startAngle);
            var borderPos = AnglePos(xCenter, yCenter, xRadius - borderWidth, yRadius - borderWidth, xAngle, yAngle);
            var outerPos = AnglePos(xCenter, yCenter, xRadius, yRadius, xAngle, yAngle);

            vertices[0] = new Vertex { position = UIVector(xCenter, yCenter), tint = innerColor };
            vertices[1] = new Vertex { position = borderPos, tint = outerColor };
            vertices[2] = new Vertex { position = borderPos, tint = borderColor };
            vertices[3] = new Vertex { position = outerPos, tint = borderColor };
            for (int i = 0; i < numSteps; i++)
            {
                var angle = startAngle + ((i + 1) * polyAngle);
                xAngle = Mathf.Sin(angle);
                yAngle = -Mathf.Cos(angle);

                borderPos.x = xCenter + (xAngle * (xRadius - borderWidth));
                borderPos.y = yCenter + (yAngle * (yRadius - borderWidth));
                outerPos.x = xCenter + (xAngle * xRadius);
                outerPos.y = yCenter + (yAngle * yRadius);

                var i3 = i * 3;
                vertices[i3 + 4] = new Vertex { position = borderPos, tint = outerColor };
                vertices[i3 + 5] = new Vertex { position = borderPos, tint = borderColor };
                vertices[i3 + 6] = new Vertex { position = outerPos, tint = borderColor };

                var i9 = i * 9;
                indices[i9 + 0] = 0;
                indices[i9 + 1] = (ushort)(i3 + 1);
                indices[i9 + 2] = (ushort)(i3 + 4);
                indices[i9 + 3] = (ushort)(i3 + 2);
                indices[i9 + 4] = (ushort)(i3 + 3);
                indices[i9 + 5] = (ushort)(i3 + 6);
                indices[i9 + 6] = (ushort)(i3 + 6);
                indices[i9 + 7] = (ushort)(i3 + 5);
                indices[i9 + 8] = (ushort)(i3 + 2);
            }

            return new UIMesh(vertices, indices);
        }
        public static UIMesh GenerateCircleBordered(Rect rect, float progress, float borderWidth, int resolution, Color32 innerColor, Color32 outerColor, Color32 borderColor) => GenerateCircleBordered(rect, 0f, progress, borderWidth, resolution, innerColor, outerColor, borderColor);
        public static UIMesh GenerateCircleBordered(Rect rect, float progress, float borderWidth, int resolution, Color32 color, Color32 borderColor) => GenerateCircleBordered(rect, 0f, progress, borderWidth, resolution, color, color, borderColor);
        public static UIMesh GenerateCircleBordered(Rect rect, float progress, float borderWidth, int resolution, Color32 color) => GenerateCircleBordered(rect, 0f, progress, borderWidth, resolution, color, color, color);
        public static UIMesh GenerateCircleBordered(Rect rect, float borderWidth, int resolution, Color32 innerColor, Color32 outerColor, Color32 borderColor) => GenerateCircleBordered(rect, 0f, 1f, borderWidth, resolution, innerColor, outerColor, borderColor);
        public static UIMesh GenerateCircleBordered(Rect rect, float borderWidth, int resolution, Color32 color, Color32 borderColor) => GenerateCircleBordered(rect, 0f, 1f, borderWidth, resolution, color, color, borderColor);
        public static UIMesh GenerateCircleBordered(Rect rect, float borderWidth, int resolution, Color32 color) => GenerateCircleBordered(rect, 0f, 1f, borderWidth, resolution, color, color, color);

    }
}
