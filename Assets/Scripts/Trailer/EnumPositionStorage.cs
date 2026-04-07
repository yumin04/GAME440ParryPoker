using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnumPositionStorage<T> where T : Enum
{
    public static readonly int Length = Enum.GetValues(typeof(T)).Length;
    public static readonly List<Transform> Positions = new List<Transform>(new Transform[Length]);
    public static int[] player1Cards = new[] { 7, 10 }; // 8, 9, 11
    public static int[] player2Cards = new[] {1, 14}; // 27, 40
    public static int[] roundCards = new[] { 9, 50, 27, 26, 11, 40, 13, 8, 19, 36};
}