using UnityEditor;
using UnityEngine;

namespace HeavenVR.Tools.DpsConfigurator
{
    public class DpsPreview : MonoBehaviour
    {
        public bool rotate;

        public float rotationSpeed;
        public float rotationVerticalAngle;
        public float rotationHorizontalAngle;

        public float penetrationSpeed;
        public float penetrationMin;
        public float penetrationMax;

        public Transform target;

        bool selfIsOrifice;
        bool targetIsOrifice;
        Transform lastTarget;
        Transform trackerLight;
        Transform normalTrackerLight;
        void GetTrackersFromTransform(Transform trackerParent)
        {
            foreach (var light in trackerParent.GetComponentsInChildren<Light>(true))
            {
                var lighttype = Helpers.GetDpsLightType(light);
                switch (lighttype)
                {
                    case Helpers.DpsLightType.OrifaceTracker:
                        trackerLight = light.transform;
                        break;
                    case Helpers.DpsLightType.OrifaceNormalTracker:
                        normalTrackerLight = light.transform;
                        break;
                    case Helpers.DpsLightType.Undefined:
                    case Helpers.DpsLightType.PenetratorTip:
                    default:
                        break;
                }
            }
        }
        Vector3 GetOrifaceDirection()
        {
            if (lastTarget != target)
            {
                GetTrackersFromTransform(target);
                targetIsOrifice = trackerLight != null && normalTrackerLight != null;
                lastTarget = target;

                if (targetIsOrifice)
                {
                    selfIsOrifice = false;
                }
                else
                {
                    GetTrackersFromTransform(transform);
                    selfIsOrifice = trackerLight != null && normalTrackerLight != null;
                }
            }

            if (!targetIsOrifice && !selfIsOrifice)
            {
                trackerLight = null;
                normalTrackerLight = null;
                return default;
            }

            return (normalTrackerLight.position - trackerLight.position).normalized;
        }
        float GetSinTime(float magnitude = 1f)
        {
            return Mathf.Sin(Time.time * magnitude);
        }
        float GetCosTime(float magnitude = 1f)
        {
            return Mathf.Cos(Time.time * magnitude);
        }

        void Start()
        {
        }

        void Update()
        {
            if (target == null) return;

            Vector3 orifaceDirection = GetOrifaceDirection();

            if (!targetIsOrifice && !selfIsOrifice)
                return;

            // Set the penetrator position
            float penetrationDepth = (GetSinTime(penetrationSpeed * Mathf.PI) + 1f) / 2f;
            Vector3 penetratorOffset = (targetIsOrifice ? orifaceDirection : target.forward) * Mathf.Lerp(penetrationMin, penetrationMax, penetrationDepth);

            if (rotate)
            {
                float verticalOffset = GetSinTime(rotationSpeed) * rotationVerticalAngle * penetrationDepth;
                float horizontalOffset = GetCosTime(rotationSpeed) * rotationHorizontalAngle * penetrationDepth;

                penetratorOffset += ((targetIsOrifice ? target.forward : target.up) * horizontalOffset) + (target.right * verticalOffset);
            }

            transform.position = target.position + penetratorOffset;
            transform.LookAt(target);
            
            if (selfIsOrifice)
            {
                transform.Rotate(Vector3.right, -90f);
            }
        }

        // TODO: implement gizmos to visualize entrace angle cone
        /*
        Mesh mesh;
        void OnDrawGizmos()
        {
            if (mesh == null)
                mesh = Helpers.CreateViewCone(45f, 3f, 60);

            GetOrifaceDirection();
            Gizmos.DrawWireMesh(mesh, oriface.position, Quaternion.FromToRotation(normalTrackerLight.position, trackerLight.position), new Vector3(1f, 1f, 1f));
        }
        */
    }

}