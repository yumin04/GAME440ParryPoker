using System;
using System.Collections.Generic;
using UnityEngine;

public static class TrailerCameraPosition
{
    public static int enumLength = Enum.GetValues(typeof(CameraPosition)).Length;
    public static List<Transform> cameraPositions = new List<Transform>(new Transform[enumLength]);
    
}