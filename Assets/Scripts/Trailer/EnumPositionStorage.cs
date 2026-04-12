using System;
using UnityEngine;

namespace Trailer {
    public static class EnumPositionStorage<T> where T : Enum
    {
        private static readonly int Length = Enum.GetValues(typeof(T)).Length;
        public static readonly Transform[] Positions = new Transform[Length];
    }
}
