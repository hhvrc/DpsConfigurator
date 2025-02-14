﻿using System.Collections.Generic;

namespace HeavenVR.DpsConf.Extensions
{
    public static class StackExtensions
    {
        public static void TryPop<T>(this Stack<T> stack)
        {
            if (stack.Count > 0)
            {
                _ = stack.Pop();
            }
        }
        public static bool TryPop<T>(this Stack<T> stack, out T value)
        {
            if (stack.Count > 0)
            {
                value = stack.Pop();
                return true;
            }

            value = default;
            return false;
        }
        public static T PeekOrDefault<T>(this Stack<T> stack, T defaultValue = default)
        {
            if (stack.Count > 0)
            {
                return stack.Peek();
            }

            return defaultValue;
        }
    }
}
