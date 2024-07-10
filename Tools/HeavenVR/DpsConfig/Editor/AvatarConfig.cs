using HeavenVR.Tools.Extensions;
using HeavenVR.Tools.Utils;
using System;
using System.IO;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using AvatarDescriptor = VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;

namespace HeavenVR.Tools.DpsConfigurator
{
    internal static class AvatarConfig
    {
        public static JObject LoadConfigForAvatar(AvatarDescriptor avatar)
        {
            try
            {
                return JObject.Parse(File.ReadAllText(GetConfigPathFromAvatar(avatar)));
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load config for avatar {avatar.name}: {e.Message}");
                return null;
            }
        }
        public static bool SaveConfigForAvatar(AvatarDescriptor avatar, JObject config)
        {
            try
            {
                var configPath = GetConfigPathFromAvatar(avatar);
                if (string.IsNullOrEmpty(configPath))
                    return false;

                File.WriteAllText(configPath, config.ToString());
                
                // Refresh asset database
                AssetDatabase.Refresh();
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save config for avatar {avatar.name}: {e.Message}");
                return false;
            }
        }
        
        public static string GetConfigIdFromAvatar(AvatarDescriptor avatar)
        {
            if (avatar == null)
                return null;

            var dpsParent = avatar.transform.GetOrAddChild("__dps");

            string configId = null;
            foreach (Transform child in dpsParent)
                if (child.name.StartsWith("__id_"))
                    configId = child.name.Substring(5);

            if (configId == null)
            {
                configId = Guid.NewGuid().ToString();
                dpsParent.AddChild("__id_" + configId);
            }

            return configId;
        }
        public static string GetConfigPathFromAvatar(AvatarDescriptor avatar)
        {
            string configId = GetConfigIdFromAvatar(avatar);
            if (string.IsNullOrEmpty(configId))
                return null;

            var configDir = PersistantConfig.ProjectPath + "/Tools/HeavenVR/DPS Configurator/Generated/Configs/";

            // Create the directory if it doesn't exist
            Directory.CreateDirectory(configDir);

            return configDir + configId + ".json";
        }
        public static AvatarDescriptor GetAvatarFromConfigId(string configId)
        {
            if (string.IsNullOrEmpty(configId))
                return null;

            var id = GameObject.Find("__id_" + configId);
            if (id == null)
                return null;

            var avatarTransform = id.transform.GetParent(2);
            if (avatarTransform == null)
                return null;

            return avatarTransform.GetComponent<AvatarDescriptor>();
        }
    }
}
