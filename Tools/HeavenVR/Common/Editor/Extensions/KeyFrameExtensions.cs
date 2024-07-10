using UnityEngine;

namespace HeavenVR.Tools.Extensions
{
    internal static class KeyFrameExtensions
    {
        public static bool ContentCompare(this Keyframe self, Keyframe other)
        {
            return
                Mathf.Approximately(self.time, other.time) &&
                Mathf.Approximately(self.value, other.value) &&
                Mathf.Approximately(self.inTangent, other.inTangent) &&
                Mathf.Approximately(self.inWeight, other.inWeight) &&
                Mathf.Approximately(self.outTangent, other.outTangent) &&
                Mathf.Approximately(self.outWeight, other.outWeight);
        }
    }
}
