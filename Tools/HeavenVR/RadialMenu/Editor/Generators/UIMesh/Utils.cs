using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class UIMeshGenerators
    {
        const float MPI2 = Mathf.PI * 2;
        static Vector3 UIVector(float x, float y) => new Vector3(x, y, Vertex.nearZ);
        static Vector3 AnglePos(float xCenter, float yCenter, float xRadius, float yRadius, float xAngle, float yAngle) => UIVector(xCenter + (xRadius * xAngle), yCenter + (yRadius * yAngle));
        static Vector3 AnglePos(float xCenter, float yCenter, float xRadius, float yRadius, float angle) => AnglePos(xCenter, yCenter, xRadius, yRadius, Mathf.Sin(angle), -Mathf.Cos(angle));

        // Have at most the requested resolution, but at least one step per 20% progress.
        static int GetRequiredResolution(int prefered, float progress)
        {
            return Mathf.Max(1, prefered, Mathf.CeilToInt(progress * 5f));
        }
    }
}
