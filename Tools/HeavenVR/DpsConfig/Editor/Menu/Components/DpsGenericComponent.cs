using Unity.Plastic.Newtonsoft.Json.Linq;

namespace HeavenVR.Tools.DpsConfigurator.Components
{
    internal class DpsGenericComponent : GUI.CollapsibleMenu
    {
        public DpsGenericComponent() : base("Uninitialized")
        {
        }
        public DpsGenericComponent(string name) : base(name)
        {
        }

        public GUI.ComponentList<ConstraintComponent> Constraints { get; } = new GUI.ComponentList<ConstraintComponent>("Constraints");

        override protected void DrawMenuContents()
        {
            Constraints.DrawGUI();
        }

        protected override void CleanupMenuContents()
        {
            Constraints.Cleanup();
        }

        protected override JObject SerializeMenuContents()
        {
            if (Constraints.Items.Count == 0)
                return null;

            return new JObject
            {
                ["constraints"] = Constraints.ToJson()
            };
        }
        protected override void DeserializeMenuContents(JToken json)
        {
            if (json == null)
                return;

            Constraints.FromJson(json["constraints"]);
        }
    }
}
