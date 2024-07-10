using HeavenVR.Tools.GUI;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace HeavenVR.Tools.Utils
{
    internal static class JsonUtils
    {
        public static void AssignIfNotNull(JObject jobj, string key, IComponent component)
        {
            var json = component.ToJson();

            if (json != null)
                jobj[key] = json;
        }
        public static JToken Serialize(Vector3 vec)
        {
            return new JArray(vec.x, vec.y, vec.z);
        }
        public static JToken Serialize(Vector4 vec)
        {
            return new JArray(vec.x, vec.y, vec.z, vec.w);
        }
        public static JToken Serialize(Quaternion quat)
        {
            return new JArray(quat.x, quat.y, quat.z, quat.w);
        }
        public static JToken Serialize(Color color)
        {
            return new JArray(color.r, color.g, color.b, color.a);
        }
        public static Vector3 DeserializeVector3(JToken token)
        {
            var arr = token as JArray;
            return new Vector3(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>());
        }
        public static Vector4 DeserializeVector4(JToken token)
        {
            var arr = token as JArray;
            return new Vector4(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>(), arr[3].Value<float>());
        }
        public static Quaternion DeserializeQuaternion(JToken token)
        {
            var arr = token as JArray;
            return new Quaternion(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>(), arr[3].Value<float>());
        }
        public static Color DeserializeColor(JToken token)
        {
            var arr = token as JArray;
            return new Color(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>(), arr[3].Value<float>());
        }
    }
}
