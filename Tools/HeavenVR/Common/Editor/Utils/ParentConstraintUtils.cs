using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace HeavenVR.Tools.Utils
{
    internal static class ParentConstraintUtils
    {
        public struct Constraint
        {
            public Constraint(ParentConstraint parentConstraint, int index)
            {
                var source = parentConstraint.GetSource(index);

                sourceWeight = source.weight;
                sourceTransform = source.sourceTransform;
                translationOffset = parentConstraint.GetTranslationOffset(index);
                rotationOffset = parentConstraint.GetRotationOffset(index);
            }
            public Constraint(float weight, Transform transform, Vector3 translation, Vector3 rotation)
            {
                sourceWeight = weight;
                sourceTransform = transform;
                translationOffset = translation;
                rotationOffset = rotation;
            }

            public float sourceWeight;
            public Transform sourceTransform;
            public Vector3 translationOffset;
            public Vector3 rotationOffset;
        }
        
        public static ParentConstraint Create(GameObject gameobject, IEnumerable<Constraint> constraints)
        {
            var parentConstriant = gameobject.GetOrAddComponent<ParentConstraint>();

            ParentConstraintUtils.ClearConstraints(parentConstriant);
            ParentConstraintUtils.Initialize(parentConstriant);
            ParentConstraintUtils.SetConstraints(parentConstriant, constraints);
            parentConstriant.enabled = true;

            return parentConstriant;
        }

        public static void Initialize(ParentConstraint parentConstraint)
        {
            parentConstraint.weight = 1f;
            parentConstraint.rotationAxis = Axis.X | Axis.Y | Axis.Z;
            parentConstraint.translationAxis = Axis.X | Axis.Y | Axis.Z;
            parentConstraint.locked = true;
        }
        public static List<Constraint> GetConstraints(ParentConstraint parentConstraint)
        {
            int sourceCount = parentConstraint.sourceCount;

            var sources = new List<Constraint>(sourceCount);

            for (int i = 0; i < sourceCount; i++)
                sources.Add(new Constraint(parentConstraint, i));

            return sources;
        }
        public static void SetConstraints(ParentConstraint parentConstraint, IEnumerable<Constraint> constraints)
        {
            ClearConstraints(parentConstraint);

            bool wasLocked = parentConstraint.locked;
            parentConstraint.locked = false;

            parentConstraint.SetSources(constraints.Select(s => new ConstraintSource { weight = s.sourceWeight, sourceTransform = s.sourceTransform }).ToList());

            int constraintCount = parentConstraint.sourceCount;

            var array = constraints.ToArray();
            for (int i = 0; i < constraintCount; i++)
            {
                parentConstraint.SetTranslationOffset(i, array[i].translationOffset);
                parentConstraint.SetRotationOffset(i, array[i].rotationOffset);
            }

            parentConstraint.locked = wasLocked;
        }
        public static void ClearConstraints(ParentConstraint parentConstraint)
        {
            bool wasLocked = parentConstraint.locked;
            parentConstraint.locked = false;

            parentConstraint.SetSources(new List<ConstraintSource>());

            parentConstraint.locked = wasLocked;
        }
        public static void AddConstraint(ParentConstraint parentConstraint, float sourceWeight, Transform sourceTransform, Vector3 translationOffset, Vector3 rotationOffset)
        {
            var constraints = GetConstraints(parentConstraint);

            constraints.Add(new Constraint(sourceWeight, sourceTransform, translationOffset, rotationOffset));

            SetConstraints(parentConstraint, constraints);
        }
    }
}
