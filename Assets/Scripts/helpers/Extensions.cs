using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Helpers {
    internal static class Extensions {
        public static T Random<T>(this IEnumerable<T> list) {
            Random random = new Random();
            return list.ElementAt(random.Next(list.Count()));
        }

        public static void Times(this int count, Action<int> action) {
            for (int i = 0; i < count; i++) {
                action(i);
            }
        }

        public static string GetPath(this Enum @enum) {
            return $"{@enum.GetType()} {@enum.ToString()}";
        }

    }
}