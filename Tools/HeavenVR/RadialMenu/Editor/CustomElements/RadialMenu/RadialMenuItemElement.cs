using HeavenVR.DpsConf.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace HeavenVR.DpsConf.CustomElements.RadialMenu
{
    public abstract class RadialMenuItemElement : VisualElement
    {
        public const float ItemSize = RadialMenuElement.RingWidth;
        public const float IconSize = ItemSize * 0.5f;

        public string Text
        {
            get => _tmpElement.Text;
            set => _tmpElement.Text = value;
        }

        Texture2D _icon;
        public Texture2D Icon
        {
            get => _icon; set
            {
                if (_icon != value)
                {
                    _icon = value;
                    _iconElement.style.backgroundImage = value;
                    MarkDirtyRepaint();
                }
            }
        }

        Texture2D _subIcon = null;
        public Texture2D SubIcon
        {
            get => _subIcon; set
            {
                if (_subIcon != value)
                {
                    _subIcon = value;
                    if (value != null)
                    {
                        _subIconElement.style.backgroundImage = value;
                    }
                    else
                    {
                        _subIconElement.visible = false;
                    }
                    MarkDirtyRepaint();
                }
            }
        }

        public bool IsPlaying { get; set; }

        readonly VisualElement _subIconElement;
        readonly VisualElement _iconElement;
        readonly TextMeshProElement _tmpElement;
        readonly VisualElement _playingElement;

        protected RadialMenuItemElement(string text, Texture2D icon = null, Texture2D subIcon = null, bool playable = false)
        {
            this.SizeSet(ItemSize);
            name = TextMeshProElement.TMPTagMatchRegex.Replace(text, string.Empty);
            pickingMode = PickingMode.Ignore;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.Center;
            style.backgroundColor = Color.clear;

            _icon = icon;
            Add(_iconElement = new VisualElement {
                name = "icon",
                pickingMode = PickingMode.Ignore,
                style = {
                    top = ItemSize * 0.138f,
                    position = Position.Absolute,
                    width = IconSize,
                    height = IconSize,
                    backgroundImage = _icon
                }
            });
            
            _subIcon = subIcon;
            Add(_subIconElement = new VisualElement { 
                name = "subIcon",
                pickingMode =
                PickingMode.Ignore,
                style = {
                    position = Position.Absolute,
                    width = IconSize,
                    height = IconSize,
                    opacity = 0.7f
                }
            });

            if (_subIcon != null)
            {
                _subIconElement.style.backgroundImage = _subIcon;
            }
            else
            {
                _subIconElement.visible = false;
            }

            Add(_tmpElement = new TextMeshProElement());
            _tmpElement.style.top = ItemSize * 0.62f;
            _tmpElement.Text = text;

            if (playable)
            {
                Add(_playingElement = new VisualElement {
                    name = "playingIndicator",
                    pickingMode = PickingMode.Ignore,
                    visible = false,
                    style = {
                        position = Position.Absolute,
                        width = IconSize * 1.4f,
                        height = IconSize * 1.4f,
                        backgroundImage = VRCIcons.RadialMenu.Icons.Playing
                    }
                });
                Helpers.OnInspectorUpdate += HandleOnInspectorUpdateEvent;
            }
        }
        ~RadialMenuItemElement()
        {
            if (_playingElement != null)
            {
                Helpers.OnInspectorUpdate -= HandleOnInspectorUpdateEvent;
            }
        }

        void HandleOnInspectorUpdateEvent(object sender, System.EventArgs e)
        {
            if (IsPlaying)
            {
                _playingElement.visible = true;
                _playingElement.RotationSet((float)EditorApplication.timeSinceStartup * 180f);
            }
            else
            {
                _playingElement.visible = false;
            }
            OnInspectorUpdate();
        }

        public virtual void OnFocus() { }
        public virtual void OnBlur() { }
        public virtual void OnMouseDown() { }
        public virtual void OnMouseUp() { }
        public virtual void OnInspectorUpdate() { }
    }
}