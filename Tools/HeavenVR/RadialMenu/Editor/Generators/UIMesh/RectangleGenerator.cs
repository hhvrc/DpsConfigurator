using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.Generators
{
    public static partial class UIMeshGenerators
    {
        public static UIMesh GenerateRectangle(Rect rect, Color32 color)
        {
            return new UIMesh(
                new Vertex[4]
                {
                    new Vertex { position = new Vector3(rect.x, rect.y, Vertex.nearZ), tint = color },
                    new Vertex { position = new Vector3(rect.xMax, rect.y, Vertex.nearZ), tint = color },
                    new Vertex { position = new Vector3(rect.xMax, rect.yMax, Vertex.nearZ), tint = color },
                    new Vertex { position = new Vector3(rect.x, rect.yMax, Vertex.nearZ), tint = color }
                },
                new ushort[6] {
                    0, 1, 2, 0, 2, 3
                }
                );
        }
    }
}
