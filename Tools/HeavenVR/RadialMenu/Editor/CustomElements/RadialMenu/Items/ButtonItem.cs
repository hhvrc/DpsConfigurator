using UnityEngine;

namespace HeavenVR.DpsConf.CustomElements.RadialMenu.Items
{
    public class ButtonItem : RadialMenuItemElement
    {
        public ButtonItem(string text, Texture2D icon) : base(text, icon, playable: true)
        {
        }

        public override void OnMouseDown()
        {
            IsPlaying = true;
        }
        public override void OnMouseUp()
        {
            IsPlaying = false;
        }
    }
}
