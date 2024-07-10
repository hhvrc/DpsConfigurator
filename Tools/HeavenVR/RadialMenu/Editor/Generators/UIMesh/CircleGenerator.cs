using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class UIMeshGenerators
    {
        public static UIMesh GenerateCircle(Rect rect, float progressFrom, float progressTo, int resolution, Color32 innerColor, Color32 outerColor)
        {
            var xRadius = rect.width * 0.5f;
            var yRadius = rect.height * 0.5f;
            var xCenter = rect.x + xRadius;
            var yCenter = rect.y + yRadius;

            var progressAmount = progressTo - progressFrom;
            var numSteps = GetRequiredResolution(resolution, progressAmount);
            var polyAngle = progressAmount * MPI2 / numSteps;
            var startAngle = progressFrom * MPI2;

            var vertices = new Vertex[numSteps + 2];
            var indices = new ushort[numSteps * 3];

            vertices[0] = new Vertex { position = UIVector(xCenter, yCenter), tint = innerColor };
            vertices[1] = new Vertex { position = AnglePos(xCenter, yCenter, xRadius, yRadius, startAngle), tint = outerColor };
            for (int i = 0; i < numSteps; i++)
            {
                vertices[i + 2] = new Vertex
                {
                    position = AnglePos(xCenter, yCenter, xRadius, yRadius, startAngle + ((i + 1) * polyAngle)),
                    tint = outerColor
                };

                var i3 = i * 3;
                indices[i3 + 0] = 0;
                indices[i3 + 1] = (ushort)(i + 1);
                indices[i3 + 2] = (ushort)(i + 2);
            }

            return new UIMesh(vertices, indices);
        }
        public static UIMesh GenerateCircle(Rect rect, float progress, int resolution, Color32 innerColor, Color32 outerColor) => GenerateCircle(rect, 0f, progress, resolution, innerColor, outerColor);
        public static UIMesh GenerateCircle(Rect rect, float progress, int resolution, Color32 color) => GenerateCircle(rect, 0f, progress, resolution, color, color);
        public static UIMesh GenerateCircle(Rect rect, int resolution, Color32 innerColor, Color32 outerColor) => GenerateCircle(rect, 0f, 1f, resolution, innerColor, outerColor);
        public static UIMesh GenerateCircle(Rect rect, int resolution, Color32 color) => GenerateCircle(rect, 0f, 1f, resolution, color, color);
    }
}
