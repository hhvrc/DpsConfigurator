using Unity.Plastic.Newtonsoft.Json.Linq;

namespace HeavenVR.Tools.GUI
{
    internal abstract class CollapsibleMenu : IComponent
    {
        public CollapsibleMenu(string name)
        {
            MenuName = name;
        }

        public string MenuName { get; set; }
        public bool MenuIsExpanded { get; set; }

        public void DrawGUI()
        {
            if (MenuIsExpanded = CustomGUILayout.BeginFoldout(MenuName, MenuIsExpanded))
            {
                DrawMenuContents();
            }
            CustomGUILayout.EndFoldout();
        }

        protected abstract void DrawMenuContents();
        protected abstract void CleanupMenuContents();
        protected abstract JObject SerializeMenuContents();
        protected abstract void DeserializeMenuContents(JToken jObject);

        void IComponent.Cleanup()
        {
            CleanupMenuContents();
        }

        public JObject ToJson()
        {
            // Get component data
            var menuContent = SerializeMenuContents();
            if (menuContent == null)
                return null;

            // Create JObject
            return new JObject
            {
                ["$name"] = MenuName,
                ["$isOpen"] = MenuIsExpanded,
                ["content"] = menuContent
            };
        }

        public void FromJson(JToken json)
        {
            if (json == null)
                return;

            MenuName = json["$name"].ToString();
            MenuIsExpanded = json["$isOpen"].ToObject<bool>();
            DeserializeMenuContents(json["content"]);
        }
    }
}
