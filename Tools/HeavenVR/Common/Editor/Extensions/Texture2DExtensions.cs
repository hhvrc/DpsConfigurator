using UnityEngine;

namespace Assets.Tools.HeavenVR.Scripts.Editor.Extensions
{
    internal static class Texture2DExtensions
    {
        // Guesses if a texture is a gradient based on its dimensions
        public static bool ProbablyIsGradient(this Texture2D self)
        {
            int maxDim = Mathf.Max(self.height, self.width);
            int minDim = Mathf.Min(self.height, self.width);
            return (minDim == 1 || minDim == 4) && maxDim >= 32;
        }
    }
}
