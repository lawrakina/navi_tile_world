﻿using System;
using Random = UnityEngine.Random;

namespace NavySpade.Modules.Extensions.CsharpTypes
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Shuffles an array of elements.
        /// </summary>
        /// <param name="array"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(this T[] array)
        {
            if (array.Length < 1)
            {
                return;
            }

            var n = array.Length;
            while (n > 1)
            {
                var k = Random.Range(0, n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
        
        public static bool AllElementsInCondition<T>(T[] elements, Func<T, bool> cond)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (cond.Invoke(elements[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static void Invoke<T>(T[] elements, Action<T> func)
        {
            Array.ForEach(elements, func.Invoke);
        }
    }
}