using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.Utils
{
    internal static class PathUtils
    {
        static bool HandleError(string msg, string err)
        {
            if (string.IsNullOrEmpty(err))
                return true;

            Debug.LogWarning(msg + err);

            return false;
        }

        // This is probably bad
        public static bool FileExistsScuffed(string path)
        {
            return !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path));
        }
        public static string GenerateNonConflictingPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return GUID.Generate().ToString();

            if (!FileExistsScuffed(path))
                return path;

            string ext = path.Substring(path.LastIndexOf('.'));

            var parts = path.Split('_').ToList();
            if (!int.TryParse(parts[parts.Count - 1], out int i))
            {
                i = 0;
                parts.Add("0");
            }

            while (FileExistsScuffed(path))
            {
                parts[parts.Count - 1] = (++i).ToString();
                path = string.Join("_", parts);
            }

            return path;
        }
        public static string GetFileName(string filePath)
        {
            int lastSlash = filePath.LastIndexOf('/');
            if (lastSlash <= 0)
                return string.Empty;

            return filePath.Substring(lastSlash + 1);
        }
        public static string GetFileDirectoryPath(string filePath)
        {
            int lastSlash = filePath.LastIndexOf('/');
            if (lastSlash <= 0)
                return string.Empty;

            return filePath.Substring(0, lastSlash);
        }
        public static bool EnsureFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            if (AssetDatabase.IsValidFolder(path))
                return true;

            string[] parts = path.Split('/');

            if (parts.Length < 2)
                return false;

            string currentPath = parts[0];

            if (currentPath != "Assets")
                return false;

            for (int i = 1; i < parts.Length; i++)
            {
                string newPath = currentPath + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(newPath))
                {
                    if (string.IsNullOrEmpty(AssetDatabase.CreateFolder(currentPath, parts[i])))
                    {
                        return false;
                    }
                }
                currentPath = newPath;
            }

            return AssetDatabase.IsValidFolder(path);
        }
        public static bool TryMoveFile(string src, string dst)
        {
            if (!EnsureFolder(GetFileDirectoryPath(dst)))
                return false;

            if (!HandleError("[1] TryMoveFile: ", AssetDatabase.ValidateMoveAsset(src, dst)))
                return false;

            return HandleError("[2] TryMoveFile: ", AssetDatabase.MoveAsset(src, dst));
        }

    }
}