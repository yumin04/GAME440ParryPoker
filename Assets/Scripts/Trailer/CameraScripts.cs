using UnityEngine;
using System.Collections;

public class CameraScripts : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Timing")]
    [SerializeField] private float moveDuration = 1.5f;
    [SerializeField] private float stayDuration = 1.0f;

    private void Start()
    {
        StartCoroutine(PlayCameraSequence());
    }

    private IEnumerator PlayCameraSequence()
    {
        foreach (CameraPosition pos in System.Enum.GetValues(typeof(CameraPosition)))
        {
            Transform target = TrailerCameraPosition.cameraPositions[(int)pos];

            if (target == null) continue;

            yield return StartCoroutine(MoveCamera(target, moveDuration));
            Debug.Log("[DEBUG] Position: " + pos);
            yield return new WaitForSeconds(stayDuration);
        }
    }

    private IEnumerator MoveCamera(Transform target, float duration)
    {
        Vector3 startPos = cameraTransform.position;
        Quaternion startRot = cameraTransform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            cameraTransform.position = Vector3.Lerp(startPos, endPos, t);
            cameraTransform.rotation = Quaternion.Slerp(startRot, endRot, t);

            time += Time.deltaTime;
            yield return null;
        }

        // 마지막 보정
        cameraTransform.position = endPos;
        cameraTransform.rotation = endRot;
    }
}