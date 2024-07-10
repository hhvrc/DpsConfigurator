using HeavenVR.DpsConf.Extensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using UIEPosition = UnityEngine.UIElements.Position;

namespace HeavenVR.DpsConf.CustomElements
{
    public class TextMeshProElement : VisualElement
    {
        public static readonly Regex TMPTagMatchRegex = new Regex(@"<\\?[^\\\s=>]+\s*=?[^>]*>", RegexOptions.Compiled);
        static readonly Regex StringMatchRegex = new Regex(@"([^<]*)<(\\?[^\s=>]*)\s*=?([^>]*)>(.*)", RegexOptions.Compiled);

        static readonly Length Length_SUX = new Length(8);
        static readonly Length DefaultTmpSize = default(Length);
        static readonly Color32 DefaultTmpFGC = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        static readonly Color32 DefaultTmpBGC = new Color32(0x00, 0x00, 0x00, 0x00);

        int _bold = 0;
        int _italic = 0;
        readonly Stack<Color32> _fgColorStack = new Stack<Color32>();
        readonly Stack<Color32> _bgColorStack = new Stack<Color32>();
        readonly Stack<Length> _sizeStack = new Stack<Length>();

        string _tmpBuffer = string.Empty;
        FontStyle _tmpStyle = FontStyle.Normal;
        Length _tmpSize = DefaultTmpSize;
        Color32 _tmpFGC = DefaultTmpFGC;
        Color32 _tmpBGC = DefaultTmpBGC;

        string _text;
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    ParseText(_text = value);
                }
            }
        }

        public FontStyle FontStyle => (FontStyle)(((_bold > 0) ? 1 : 0) | ((_italic > 0) ? 2 : 0));
        public Length FontSize => _sizeStack.PeekOrDefault(DefaultTmpSize);
        public Color32 ForegroundColor => _fgColorStack.PeekOrDefault(DefaultTmpFGC);
        public Color32 BackgroundColor => _bgColorStack.PeekOrDefault(DefaultTmpBGC);

        public TextMeshProElement()
        {
            pickingMode = PickingMode.Ignore;
            style.alignItems = Align.Center;
            style.alignContent = Align.Center;
            style.justifyContent = Justify.Center;
            style.flexDirection = FlexDirection.Row;
            style.position = UIEPosition.Absolute;
        }

        void ClearText()
        {
            _bold = 0;
            _italic = 0;
            _fgColorStack.Clear();
            _bgColorStack.Clear();
            _sizeStack.Clear();
            _tmpBuffer = "";
            _tmpStyle = FontStyle.Normal;
            _tmpSize = DefaultTmpSize;
            _tmpFGC = DefaultTmpFGC;
            _tmpBGC = DefaultTmpBGC;

            for (int i = childCount; i-- > 0;)
            {
                RemoveAt(i);
            }
        }
        void ParseText(string input)
        {
            ClearText();
            string remainingText = input;
            while (!string.IsNullOrEmpty(remainingText))
            {
                var match = StringMatchRegex.Match(remainingText);
                if (!match.Success)
                {
                    AddText(remainingText);
                    break;
                }

                var matchGroups = match.Groups;

                AddText(matchGroups[1].Value);

                var tag = matchGroups[2].Value;
                if (!string.IsNullOrEmpty(tag))
                {
                    if (tag[0] == '/')
                    {
                        CloseTag(tag.Substring(1));
                    }
                    else
                    {
                        OpenTag(tag, matchGroups[3].Value);
                    }
                }

                remainingText = matchGroups[4].Value;
            }

            FlushText();
            MarkDirtyRepaint();
        }

        void OpenTag(string tag, string attribute)
        {
            switch (tag)
            {
                case "b":
                    _bold++;
                    return;
                case "i":
                    _italic++;
                    return;
                case "color":
                    if (Helpers.TryParseTMPColor(attribute, out var color)) _fgColorStack.Push(color);
                    return;
                case "mark":
                    if (Helpers.TryParseTMPColor(attribute, out color)) _bgColorStack.Push(color);
                    return;
                case "size":
                    if (int.TryParse(attribute, out var size)) _sizeStack.Push(new Length(size));
                    return;
                case "sup":
                case "sub":
                    _sizeStack.Push(Length_SUX);
                    return;
                case "sprite": // Fancy stuff
                    //FlushText();
                    AddText("☻");
                    return;
                case "voffset":
                default:
                    return;
            }
        }
        void CloseTag(string tag)
        {
            switch (tag)
            {
                case "b":
                    _bold--;
                    return;
                case "i":
                    _italic--;
                    return;
                case "color":
                    _fgColorStack.TryPop();
                    return;
                case "mark":
                    _bgColorStack.TryPop();
                    return;
                case "sup":
                case "sub":
                case "size":
                    _sizeStack.TryPop();
                    return;
                case "sprite": // Fancy stuff
                    //FlushText();
                    return;
                case "voffset":
                default:
                    return;
            }
        }

        void AddText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var style = FontStyle;
                var size = FontSize;
                var fgCol = ForegroundColor;
                var bgCol = BackgroundColor;

                if (style != _tmpStyle || size != _tmpSize || !fgCol.Equals(_tmpFGC) || !bgCol.Equals(_tmpBGC))
                {
                    FlushText();
                    _tmpStyle = style;
                    _tmpSize = size;
                    _tmpFGC = fgCol;
                    _tmpBGC = bgCol;
                }

                _tmpBuffer += text;
            }
        }

        void FlushText()
        {
            if (!string.IsNullOrEmpty(_tmpBuffer))
            {
                Add(new TextElement
                {
                    text = _tmpBuffer,
                    pickingMode = PickingMode.Ignore,
                    style = {
                        unityFontStyleAndWeight = _tmpStyle,
                        fontSize = _tmpSize,
                        color = (Color)_tmpFGC,
                        backgroundColor = (Color)_tmpBGC
                    }
                });
            }
            _tmpBuffer = string.Empty;
        }
    }
}