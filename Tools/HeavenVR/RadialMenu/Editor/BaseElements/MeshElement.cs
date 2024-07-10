using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf
{
    public abstract class MeshElement : VisualElement
    {
        public MeshElement()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        readonly Dictionary<string, Func<UIMesh>> _meshGenerators = new Dictionary<string, Func<UIMesh>>();

        public void AddMesh(string key, Func<UIMesh> meshGen)
        {
            _meshGenerators.Add(key, meshGen);
        }
        public void RemoveMesh(string key)
        {
            _meshGenerators.Remove(key);
        }

        bool _dirtyMesh = true;
        protected void MarkMeshDirtyRepaint()
        {
            _dirtyMesh = true;
            MarkDirtyRepaint();
        }
        bool _dirtyColor = false;
        protected void MarkColorDirtyRepaint()
        {
            _dirtyColor = true;
            MarkDirtyRepaint();
        }

        protected abstract UIMesh GenerateUIMesh();
        protected virtual void ColorUIMesh(UIMesh mesh) { }

        UIMesh _uiMesh = null;
        void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            if (_dirtyMesh || _uiMesh == null)
            {
                _uiMesh = GenerateUIMesh();

                foreach (var meshGen in _meshGenerators.Values)
                {
                    _uiMesh.AddMesh(meshGen());
                }

                Debug.Log($"Created mesh with: {_uiMesh.Indices.Length / 3} Polygons, and {_uiMesh.Vertices.Length} Vertices");
            }
            else if (_dirtyColor)
            {
                ColorUIMesh(_uiMesh);
            }

            _dirtyMesh = false;
            _dirtyColor = false;

            _uiMesh.WriteTo(ctx);
        }
    }
}
