using HeavenVR.DpsConf.Generators;
using UnityEngine;

namespace HeavenVR.DpsConf
{
    public class CircleElement : MeshElement
    {
        public const int CircleResolution = 64;

        public CircleElement(float progress, float borderWidth, Color innerColor, Color outerColor, Color borderColor)
        {
            _progress = progress;
            _borderWidth = borderWidth;
            _innerColor = innerColor;
            _outerColor = outerColor;
            _borderColor = borderColor;
        }
        public CircleElement(float borderWidth, Color innerColor, Color outerColor, Color borderColor)
            : this(1f, borderWidth, innerColor, outerColor, borderColor)
        {
        }
        public CircleElement(float progress, float borderWidth, Color fillColor, Color borderColor)
            : this(progress, borderWidth, fillColor, fillColor, borderColor)
        {
        }
        public CircleElement(float borderWidth, Color fillColor, Color borderColor)
            : this(1f, borderWidth, fillColor, fillColor, borderColor)
        {
        }
        public CircleElement()
            : this(1f, 2f, Color.white, Color.black, Color.white)
        {
        }

        float _progress;
        public float Progress
        {
            get => _progress; set
            {
                if (!Mathf.Approximately(_progress, value))
                {
                    _progress = value;
                    MarkDirtyRepaint();
                }
            }
        }

        float _borderWidth;
        public float BorderWidth
        {
            get => _borderWidth; set
            {
                if (!Mathf.Approximately(_borderWidth, value))
                {
                    _borderWidth = value;
                    MarkDirtyRepaint();
                }
            }
        }

        Color _innerColor;
        public Color InnerColor
        {
            get => _innerColor; set
            {
                if (!Helpers.Approximately(_innerColor, value))
                {
                    _innerColor = value;
                    MarkDirtyRepaint();
                }
            }
        }

        Color _outerColor;
        public Color OuterColor
        {
            get => _outerColor; set
            {
                if (!Helpers.Approximately(_outerColor, value))
                {
                    _outerColor = value;
                    MarkDirtyRepaint();
                }
            }
        }

        Color _borderColor;
        public Color BorderColor
        {
            get => _borderColor; set
            {
                if (!Helpers.Approximately(_borderColor, value))
                {
                    _borderColor = value;
                    MarkDirtyRepaint();
                }
            }
        }

        protected override UIMesh GenerateUIMesh()
        {
            if (Mathf.Approximately(BorderWidth, 0f))
            {
                return UIMeshGenerators.GenerateCircle(contentRect, 0f, Progress, CircleResolution, _innerColor, _outerColor);
            }
            else
            {
                return UIMeshGenerators.GenerateCircleBordered(contentRect, 0f, Progress, _borderWidth, CircleResolution, _innerColor, _outerColor, _borderColor);
            }
        }

        protected override void ColorUIMesh(UIMesh mesh)
        {
        }
    }
}