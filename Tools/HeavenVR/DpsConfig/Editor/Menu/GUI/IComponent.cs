using Unity.Plastic.Newtonsoft.Json.Linq;

namespace HeavenVR.Tools.GUI
{
    internal interface IComponent
    {
        void DrawGUI();
        void Cleanup();
        JObject ToJson();
        void FromJson(JToken json);
    }
}
