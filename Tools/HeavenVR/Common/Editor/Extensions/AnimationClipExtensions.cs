using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.Extensions
{
    internal static class AnimationClipExtensions
    {
        public static bool ContentCompare(this AnimationClip self, AnimationClip other)
        {
            if (self == other || (self.empty && other.empty))
                return true;

            if (!Mathf.Approximately(self.length, other.length) || !Mathf.Approximately(self.frameRate, other.frameRate))
                return false;

            string path = AssetDatabase.GetAssetPath(self);
            if (!path.StartsWith("Assets"))
                return false;

            EditorCurveBinding[] selfBindings = AnimationUtility.GetCurveBindings(self);
            EditorCurveBinding[] otherBindings = AnimationUtility.GetCurveBindings(other);

            if (selfBindings.Length != otherBindings.Length)
                return false;

            for (int i = 0; i < selfBindings.Length; i++)
            {
                EditorCurveBinding selfBinding = selfBindings[i];
                EditorCurveBinding otherBinding = otherBindings[i];

                if (selfBinding.path != otherBinding.path || selfBinding.propertyName != otherBinding.propertyName)
                    return false;

                AnimationCurve selfCurve = AnimationUtility.GetEditorCurve(self, selfBinding);
                AnimationCurve otherCurve = AnimationUtility.GetEditorCurve(other, otherBinding);

                if (!selfCurve.ContentCompare(otherCurve))
                    return false;
            }

            return true;
        }
    }
}
