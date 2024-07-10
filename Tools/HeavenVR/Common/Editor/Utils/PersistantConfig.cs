using System;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace HeavenVR.Tools.Utils
{
    internal static class PersistantConfig
    {
        static string ConfigPath => Application.temporaryCachePath + "/tools/heavenvr/config.json";
        static PersistantConfig()
        {
            try
            {
                _jsonData = JObject.Parse(File.ReadAllText(ConfigPath));
            }
            catch (Exception)
            {
                _jsonData = new JObject
                {
                    ["custom"] = new JObject()
                };
            }
        }

        static void InternalSave()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath));
            File.WriteAllText(ConfigPath, _jsonData.ToString(Formatting.None));
        }


        static readonly JObject _jsonData;
        static string _projectPath = "";
        public static string ProjectPath
        {
            get
            {
                lock (_projectPath)
                {
                    if (string.IsNullOrEmpty(_projectPath))
                    {
                        lock (_jsonData)
                        {
                            if (!Application.isPlaying)
                            {
                                _projectPath = Application.dataPath;
                                _jsonData["projectPath"] = _projectPath;
                                InternalSave();
                            }
                            else
                            {
                                _projectPath = _jsonData["projectPath"]?.ToString();
                            }
                        }
                    }

                    return _projectPath;
                }
            }
        }


        public static void SetCustomData(string key, JToken value)
        {
            lock (_jsonData)
            {
                _jsonData["custom"][key] = value;
                InternalSave();
            }
        }
        public static JToken GetCustomData(string key)
        {
            lock (_jsonData)
            {
                return _jsonData["custom"][key]?.DeepClone();
            }
        }

        public static void Save()
        {
            lock (_jsonData)
            {
                InternalSave();
            }
        }
    }
}
