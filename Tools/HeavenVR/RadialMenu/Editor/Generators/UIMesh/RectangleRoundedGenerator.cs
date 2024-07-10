using System;
using UnityEngine;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class UIMeshGenerators
    {
        public static UIMesh GenerateRectangleRounded(Rect rect, float cornerRadius, int cornerResolution, Color32 color)
        {
            cornerRadius = Mathf.Max(Mathf.Min(cornerRadius, Mathf.Min(rect.height, rect.width) * 0.5f), 0f);
            float cornerDiameter = cornerRadius * 2f;

            if (Mathf.Approximately(cornerRadius, 0f))
            {
                return GenerateRectangle(rect, color);
            }

            bool noVerticalFace = Mathf.Approximately(cornerDiameter, rect.height);
            bool noHorizontalFace = Mathf.Approximately(cornerDiameter, rect.width);

            if (noVerticalFace && noHorizontalFace)
            {
                return GenerateCircle(rect, cornerResolution, color, color);
            }

            cornerResolution = Math.Max((cornerResolution / 4) + 1, 1) * 4;

            float circleRight = rect.xMax - cornerDiameter;
            float circleBottom = rect.yMax - cornerDiameter;

            UIMesh baseMesh = new UIMesh();

            if (!noVerticalFace)
            {
                baseMesh.AddMesh(GenerateRectangle(new Rect(rect.x, rect.y + cornerRadius, rect.width, rect.height - cornerDiameter), color));
            }
            if (!noHorizontalFace)
            {
                baseMesh.AddMesh(GenerateRectangle(new Rect(rect.x + cornerRadius, rect.y, rect.width - cornerDiameter, rect.height), color));
            }

            baseMesh.AddMesh(GenerateCircle(new Rect(rect.x, rect.y, cornerDiameter, cornerDiameter), 0.75f, 1f, cornerResolution, color, color));
            baseMesh.AddMesh(GenerateCircle(new Rect(rect.x, circleBottom, cornerDiameter, cornerDiameter), 0.5f, 0.75f, cornerResolution, color, color));
            baseMesh.AddMesh(GenerateCircle(new Rect(circleRight, rect.y, cornerDiameter, cornerDiameter), 0f, 0.25f, cornerResolution, color, color));
            baseMesh.AddMesh(GenerateCircle(new Rect(circleRight, circleBottom, cornerDiameter, cornerDiameter), 0.25f, 0.5f, cornerResolution, color, color));

            // TODO: optimize the optimizer
            // baseMesh.Optimize();

            return baseMesh;
        }
    }
}
