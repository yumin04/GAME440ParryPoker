using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Transform[] slots;
    [SerializeField] private float rotateSpeed = 300f;

    private bool[] isSpinning;
    private int stopIndex = 0;

    private void Start()
    {
        isSpinning = new bool[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            isSpinning[i] = true;
        }
    }

    private void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (isSpinning[i])
            {
                // up and down
                slots[i].Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            }
        }
    }

    private void OnMouseDown()
    {
        if (stopIndex >= slots.Length) return;

        isSpinning[stopIndex] = false;
        stopIndex++;

        // if all slots stopped
        if (stopIndex >= slots.Length)
        {
            GameEvents.OnAllLeversDown?.Invoke();
        }
    }
}