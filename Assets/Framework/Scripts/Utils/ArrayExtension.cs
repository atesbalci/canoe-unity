using System;

namespace Framework.Scripts.Utils
{
    public static class ArrayExtension
    {
        public static int GetFirstAvailablePosition(this Array array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                if (array.GetValue(i) == null) return i;
            }
            return -1;
        }
    }
}