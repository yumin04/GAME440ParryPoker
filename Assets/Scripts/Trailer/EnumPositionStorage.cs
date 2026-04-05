using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnumPositionStorage<T> where T : Enum
{
    public static readonly int Length = Enum.GetValues(typeof(T)).Length;
    public static readonly List<Transform> Positions = new List<Transform>(new Transform[Length]);
}