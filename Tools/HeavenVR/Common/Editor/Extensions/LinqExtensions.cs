using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace HeavenVR.Tools.Extensions
{
    internal static class LinqExtensions
    {
        public delegate string CreateInfoStringDelegate<T>(T item);
        public static IEnumerable<T> UnityDisplayProgressBar<T>(this IEnumerable<T> source, string title, CreateInfoStringDelegate<T> createInfoString = null)
        {
            if (createInfoString == null)
                createInfoString = (T e) => "";

            float length = source.Count();

            int i = 0;
            foreach (T element in source)
            {
                float progress = i++ / length;

                EditorUtility.DisplayProgressBar(title, createInfoString(element), progress);

                yield return element;
            }

            EditorUtility.ClearProgressBar();
        }
        public static IEnumerable<T> UnityDisplayCancellableProgressBar<T>(this IEnumerable<T> source, string title, CreateInfoStringDelegate<T> createInfoString = null)
        {
            if (createInfoString == null)
                createInfoString = (T e) => "";

            float length = source.Count();

            int i = 0;
            foreach (T element in source)
            {
                float progress = i++ / length;

                if (EditorUtility.DisplayCancelableProgressBar(title, createInfoString(element), progress))
                    break;

                yield return element;
            }

            EditorUtility.ClearProgressBar();
        }
    }
}
