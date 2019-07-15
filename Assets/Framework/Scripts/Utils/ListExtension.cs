using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Framework.Scripts.Utils
{
    public static class ListExtension
    {
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list.Swap(i, Random.Range(0, list.Count));
            }
        }

        public static List<T> Shuffled<T>(this List<T> list)
        {
            var shuffled = new List<T>(list);

            for (int i = 0; i < shuffled.Count; i++)
            {
                shuffled.Swap(i, Random.Range(0, shuffled.Count));
            }

            return shuffled;
        }

        public static void Swap<T>(this List<T> list, int index0, int index1)
        {
            var temp = list[index0];
            list[index0] = list[index1];
            list[index1] = temp;
        }
    }
}