using System;
using UnityEngine;

public class CameraPositionStorage : MonoBehaviour
{
    [SerializeField] private CameraPosition cameraPosition;

    private void Awake()
    {
        // // enum 길이만큼 리스트 초기화 안되어있으면 초기화
        // if (TrailerCameraPosition.cameraPositions == null || 
        //     TrailerCameraPosition.cameraPositions.Count == 0)
        // {
        //     TrailerCameraPosition.Initialize();
        // }

        Debug.Log("[DEBUG] This Has Been Called");
        int index = (int)cameraPosition;
        
        // 해당 위치에 transform 저장
        TrailerCameraPosition.cameraPositions[index] = transform;
    }
}