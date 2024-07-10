using UnityEngine;

namespace HeavenVR.DpsConf.CustomElements.RadialMenu.Items
{
    public class ToggleItem : RadialMenuItemElement
    {
        public ToggleItem(string text, Texture2D icon) : base(text, icon, playable: true)
        {
        }

        public bool IsToggled
        {
            get => IsPlaying;
            set => IsPlaying = value;
        }

        public override void OnMouseDown()
        {
            IsPlaying = !IsPlaying;
        }
    }
}
