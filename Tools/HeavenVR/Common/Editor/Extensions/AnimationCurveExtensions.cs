using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.Extensions
{
    internal static class AnimationCurveExtensions
    {
        public static bool ContentCompare(this AnimationCurve self, AnimationCurve other)
        {
            if (self != other)
            {
                if (self.length != other.length)
                    return false;

                for (int j = 0; j < self.length; j++)
                {
                    if (
                        !self[j].ContentCompare(other[j]) ||
                        AnimationUtility.GetKeyLeftTangentMode(self, j) != AnimationUtility.GetKeyLeftTangentMode(other, j) ||
                        AnimationUtility.GetKeyRightTangentMode(self, j) != AnimationUtility.GetKeyRightTangentMode(other, j)
                        )
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
