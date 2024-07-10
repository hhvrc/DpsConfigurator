using HeavenVR.DpsConf.Extensions;
using HeavenVR.DpsConf.Generators;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.CustomElements.RadialMenu
{
    public class RadialMenuElement : MeshElement
    {
        public const float MenuDiameter = 250f;
        public const float RingWidth = MenuDiameter * (1f - InnerRatio) * 0.5f;
        const float InnerRatio = 0.385f;
        readonly Vector2 MenuSize = new Vector2(MenuDiameter, MenuDiameter);

        readonly List<RadialMenuItemElement> DefaultMenuParams = new List<RadialMenuItemElement>() { new Items.BackButtonItem() };

        public RadialMenuElement()
        {
            this.SizeSet(MenuDiameter);
            style.overflow = Overflow.Hidden;

            Add(RadialCursor = new RadialCursorElement());
            RadialCursor.PositionSetCenter(contentRect.position + (MenuSize * 0.5f));

            Items = DefaultMenuParams;

            RegisterCallback<MouseMoveEvent>(HandleMouseMove, TrickleDown.TrickleDown);
            RegisterCallback<MouseLeaveEvent>(HandleMouseLeave, TrickleDown.TrickleDown);
            RegisterCallback<MouseDownEvent>(HandleMouseDown, TrickleDown.TrickleDown);
            RegisterCallback<MouseUpEvent>(HandleMouseUp, TrickleDown.TrickleDown);
        }

        public RadialCursorElement RadialCursor { get; }

        object m_meshCtx = null;
        List<RadialMenuItemElement> m_items;
        public List<RadialMenuItemElement> Items
        {
            get => m_items; set
            {
                if (m_items != null)
                {
                    foreach (var item in m_items)
                    {
                        Remove(item);
                    }
                }

                m_items = value ?? DefaultMenuParams;

                if (SelectedSegmentIndex > m_items.Count)
                {
                    SelectedSegmentIndex = -1;
                }

                var halfSize = MenuSize * 0.5f;
                var baseOffset = contentRect.position + halfSize;
                var segmentAngle = Mathf.PI * 2f / m_items.Count;
                var angle = 0f;
                for (int i = 0; i < m_items.Count; i++)
                {
                    var item = m_items[i];

                    var currentOffset = baseOffset + (new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle)) * halfSize * 0.7f);

                    item.PositionSetCenter(currentOffset);
                    Add(item);

                    angle += segmentAngle;
                }

                RadialCursor.BringToFront();

                MarkDirtyRepaint();
            }
        }

        int _selectedSegmentIndex = -1;
        public int SelectedSegmentIndex
        {
            get => _selectedSegmentIndex; private set
            {
                if (value != _selectedSegmentIndex)
                {
                    if (_selectedSegmentIndex >= 0)
                    {
                        m_items[_selectedSegmentIndex].OnBlur();
                        _selectedSegmentIndex = -1;
                    }

                    if (value >= 0 && value < m_items.Count)
                    {
                        _selectedSegmentIndex = value;
                        m_items[value].OnFocus();
                    }

                    MarkColorDirtyRepaint();
                }
            }
        }

        private void HandleMouseMove(MouseMoveEvent evt)
        {
            const float CursorLimit = (MenuDiameter * InnerRatio) * 0.38f;

            var pos = evt.localMousePosition;
            var center = contentRect.center;
            var diff = (pos - center).normalized;

            var distance = Vector2.Distance(pos, center);
            if (distance >= CursorLimit)
            {
                pos = center + (diff * CursorLimit);
            }

            RadialCursor.PositionSetCenter(pos);

            if (distance * 2f > CursorLimit)
            {
                const float PI2 = Mathf.PI * 2f;
                float segmentAngle = PI2 / m_items.Count;
                float currentAngle = -Mathf.Atan2(-diff.x, -diff.y) + PI2 + (segmentAngle / 2);

                var index = (int)(currentAngle / segmentAngle) % m_items.Count;

                SelectedSegmentIndex = index;
            }
            else
            {
                SelectedSegmentIndex = -1;
            }
        }
        private void HandleMouseLeave(MouseLeaveEvent evt)
        {
            const float MenuRadius = MenuDiameter * 0.5f;

            var center = contentRect.center;

            var distance = Vector2.Distance(evt.localMousePosition, center);
            if (distance > MenuRadius)
            {
                RadialCursor.PositionSetCenter(contentRect.center);
                SelectedSegmentIndex = -1;
            }
        }
        private void HandleMouseDown(MouseDownEvent evt)
        {
            if (SelectedSegmentIndex != -1)
            {
                m_items[SelectedSegmentIndex].OnMouseDown();
            }
        }
        private void HandleMouseUp(MouseUpEvent evt)
        {
            if (SelectedSegmentIndex != -1)
            {
                m_items[SelectedSegmentIndex].OnMouseUp();
            }
        }

        protected override UIMesh GenerateUIMesh()
        {
            UIMesh mesh;
            (mesh, m_meshCtx) = UIMeshGenerators.GenerateRadialMenu(contentRect, m_items.Count, InnerRatio, 64, SelectedSegmentIndex);
            return mesh;
        }

        protected override void ColorUIMesh(UIMesh m) => UIMeshGenerators.RecolorRadialMenu(m, m_meshCtx, SelectedSegmentIndex);
    }
}