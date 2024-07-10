using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HeavenVR.Tools.Utils
{
    internal class TransformUtils
    {
        static void RemoveBeforeAndWith(List<Transform> list, Transform element)
        {
            int index = list.IndexOf(element);
            if (index >= 0)
            {
                list.RemoveRange(0, index + 1);
            }
        }
        static void ReduceToCommonRoot(List<Transform> pathA, List<Transform> pathB)
        {
            var commonRoot = pathA.Intersect(pathB).LastOrDefault();
            if (commonRoot != null)
            {
                RemoveBeforeAndWith(pathA, commonRoot);
                RemoveBeforeAndWith(pathB, commonRoot);
            }
        }
        public static List<Transform> GetAbsolutePath(Transform transform)
        {
            var path = new List<Transform>();
            while (transform != null)
            {
                path.Add(transform);
                transform = transform.parent;
            }
            path.Reverse();
            return path;
        }

        public static string GetPath(Transform transform, Transform relativeTo = null)
        {
            if (transform == null)
            {
                return string.Empty;
            }
            if (transform == relativeTo)
            {
                return ".";
            }

            try
            {
                // Walk up the hierarchy from the transform and save the hierarchy to a list
                var transformPath = GetAbsolutePath(transform);
                var relativeToPath = GetAbsolutePath(relativeTo);
                ReduceToCommonRoot(transformPath, relativeToPath);

                var path = new List<string>();

                path.AddRange(relativeToPath.Select(t => ".."));
                path.AddRange(transformPath.Select(t => Convert.ToBase64String(Encoding.UTF8.GetBytes(t.name)) + "," + t.GetSiblingIndex()));

                return string.Join("\\", path.ToArray());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }
        public static Transform GetFromPath(string path, Transform relativeTo = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            if (path == ".")
            {
                return relativeTo;
            }

            try
            {
                var pathParts = path.Split('\\');

                var current = relativeTo;
                for (int i = 0; i < pathParts.Length; i++)
                {
                    var dir = pathParts[i];

                    if (dir == "..")
                    {
                        if (current == null)
                        {
                            return null;
                        }
                        current = current.parent;
                    }
                    else
                    {
                        var dirParts = dir.Split(',');
                        var name = Encoding.UTF8.GetString(Convert.FromBase64String(dirParts[0]));
                        var index = int.Parse(dirParts[1]);

                        if (current == null)
                        {
                            Transform result = null;
                            foreach (var transform in UnityEngine.Object.FindObjectsOfType<Transform>().Where(t => t.parent == null && t.name == name))
                            {
                                result = GetFromPath(string.Join("\\", pathParts.Skip(i + 1)), transform);
                                if (result != null)
                                    break;
                            }

                            return result;
                        }
                        current = current.GetChild(index);

                        if (current.name != name)
                        {
                            return null;
                        }
                    }
                }
                return current;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }
    }
}
