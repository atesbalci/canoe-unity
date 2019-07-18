using System;

namespace Framework.Scripts.Utils
{
    public static class ArrayExtension
    {
        public static int GetFirstNullPosition(this Array array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                if (array.GetValue(i) == null) return i;
            }
            return -1;
        }

        public static int CountWithoutNull(this Array array)
        {
            var count = 0;
            for (var i = 0; i < array.Length; i++)
            {
                if (array.GetValue(i) != null) count++;
            }
            return count;
        }
    }
}