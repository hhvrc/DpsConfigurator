using HeavenVR.Tools.DpsConfigurator;
using HeavenVR.Tools.DpsConfigurator.Menus;
using HeavenVR.Tools.GUI;
using HeavenVR.Tools.Utils;
using System;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using AvatarDescriptor = VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;

namespace Assets.Tools.HeavenVR.DPS_Configurator
{
    internal class MainMenu : EditorWindow
    {
        #region Boilerplate Garbage
        MainMenu()
        {
            Instance = this;
        }
        static MainMenu()
        {
            EditorApplication.playModeStateChanged += ModeStateChanged;
        }
        private static void ModeStateChanged(PlayModeStateChange state)
        {
            if (Instance == null) return;

            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                case PlayModeStateChange.ExitingEditMode:
                    Instance.ConfigSave();
                    Instance.SelectionSave();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    Instance.SelectionLoad();
                    Instance.ConfigLoad();
                    break;
                default:
                    break;
            }
        }

        [MenuItem("Tools/HeavenVR/DPS Configurator")]
        public static void Init()
        {
            GetWindow<MainMenu>(false, "DPS Config", true).Show();
        }
        #endregion
        public static MainMenu Instance { get; private set; }
        public static AvatarDescriptor SelectedAvatar { get; private set; }

        ConstraintsMenu _constraintsMenu;
        ParametersMenu _parametersMenu;

        private void OnEnable()
        {
            _constraintsMenu = new ConstraintsMenu();
            _parametersMenu = new ParametersMenu("DPS");
            SelectionLoad();
            ConfigLoad();
        }
        void OnGUI()
        {
            var avatar = CustomGUILayout.ObjectSelectionField("Avatar", SelectedAvatar, true);
            if (SelectedAvatar != avatar)
            {
                ConfigSave();
                SelectedAvatar = avatar;
                if (avatar != null)
                {
                    ConfigLoad();
                }
            }
            if (avatar == null)
            {
                return;
            }

            if (MenuScopeContext.Any())
            {
                MenuScopeContext.DrawGUI();
            }
            else
            {
                MainMenuGUI();
            }
        }
        private void OnDisable()
        {
            ConfigSave();
            SelectionSave();
        }
        void MainMenuGUI()
        {
            if (GUILayout.Button("Constraints"))
            {
                MenuScopeContext.Push("Constraints", _constraintsMenu);
            }
            if (GUILayout.Button("Parameters"))
            {
                MenuScopeContext.Push("Parameters", _parametersMenu);
            }
        }

        public bool SelectionSave()
        {
            try
            {
                PersistantConfig.SetCustomData("DPSConfigurator.SelectedAvatarPath", AvatarConfig.GetConfigIdFromAvatar(SelectedAvatar));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SelectionLoad()
        {
            try
            {
                SelectedAvatar = AvatarConfig.GetAvatarFromConfigId(PersistantConfig.GetCustomData("DPSConfigurator.SelectedAvatarPath")?.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ConfigSave()
        {
            try
            {
                if (SelectedAvatar == null) return false;

                var json = new JObject();

                // TODO: Populate JSON with data from the UI

                AvatarConfig.SaveConfigForAvatar(SelectedAvatar, json);
            }
            catch (Exception e)
            {
                Debug.LogError("Error saving config: " + e.Message);
                return false;
            }

            return true;
        }
        public bool ConfigLoad()
        {
            try
            {
                var avatarConfig = AvatarConfig.LoadConfigForAvatar(SelectedAvatar);

                // TODO: Populate UI with data from the JSON
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error loading config: " + e.Message);
                return false;
            }

            return true;
        }
    }
}
