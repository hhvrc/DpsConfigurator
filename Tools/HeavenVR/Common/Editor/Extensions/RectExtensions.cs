using UnityEngine;

namespace HeavenVR.DpsConf.Extensions
{
    public static class RectExtensions
    {
        public static Rect SubRect(this Rect rect, float ratio)
        {
            var newSize = rect.size * ratio;
            return new Rect(rect.position + ((rect.size - newSize) * 0.5f), newSize);
        }
    }
}
