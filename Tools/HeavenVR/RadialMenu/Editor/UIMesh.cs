using System;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf
{
    public class UIMesh
    {
        public UIMesh()
        {
            m_vertices = new Vertex[0];
            m_indices = new ushort[0];
        }
        public UIMesh(int vertices, int indices)
        {
            m_vertices = new Vertex[vertices];
            m_indices = new ushort[indices];
        }
        public UIMesh(Vertex[] vertices, ushort[] indices)
        {
            m_vertices = vertices;
            m_indices = indices;
        }

        Vertex[] m_vertices;
        public Vertex[] Vertices { get => m_vertices; set => m_vertices = value; }
        public ushort VertexCount => (ushort)(m_vertices?.Length ?? 0);

        ushort[] m_indices;
        public ushort[] Indices { get => m_indices; set => m_indices = value; }


        public void WriteTo(MeshGenerationContext ctx)
        {
            var writeData = ctx.Allocate(m_vertices.Length, m_indices.Length);
            writeData.SetAllVertices(m_vertices);
            writeData.SetAllIndices(m_indices);
        }

        public ushort Resize(ushort n)
        {
            var oldLength = m_vertices.Length;
            int newLength = oldLength + n;

            if (newLength > UInt16.MaxValue)
                throw new OverflowException($"Mesh size exceeds hard-cap of {UInt16.MaxValue} vertices!");

            Array.Resize(ref m_vertices, newLength);

            return (ushort)oldLength;
        }

        public ushort AddVertices(Vertex[] vertices)
        {
            if (vertices.Length == 0)
                return 0;
            if (m_vertices.Length > UInt16.MaxValue)
                throw new OverflowException($"Input mesh size exceeds hard-cap of {UInt16.MaxValue} vertices!");

            var startIndex = Resize((ushort)vertices.Length);

            Array.Copy(vertices, 0, m_vertices, startIndex, vertices.Length);

            return startIndex;
        }

        public int AllocIndices(int n)
        {
            int oldLength = m_indices.Length;

            ushort[] newIndices = new ushort[oldLength + n];
            Array.Copy(m_indices, 0, newIndices, 0, oldLength);
            m_indices = newIndices;

            return oldLength;
        }

        public void AddIndices(ushort[] indices)
        {
            int offset = AllocIndices(indices.Length);

            Array.Copy(indices, 0, m_indices, offset, indices.Length);
        }

        void AddIndicesShifted(ushort[] indices, ushort shiftAmount)
        {
            int offset = m_indices.Length;

            Array.Resize(ref m_indices, m_indices.Length + indices.Length);

            for (int i = 0; i < indices.Length; i++)
            {
                m_indices[offset + i] = (ushort)(indices[i] + shiftAmount);
            }
        }

        public ushort AddMesh(UIMesh mesh)
        {
            var startIndex = AddVertices(mesh.m_vertices);

            AddIndicesShifted(mesh.m_indices, startIndex);

            return startIndex;
        }

        public UIMesh Clone()
        {
            return new UIMesh((Vertex[])m_vertices.Clone(), (ushort[])m_indices.Clone());
        }

        public static UIMesh operator +(UIMesh a, UIMesh b)
        {
            var newMesh = a.Clone();
            newMesh.AddMesh(b);
            return newMesh;
        }
    }
}
