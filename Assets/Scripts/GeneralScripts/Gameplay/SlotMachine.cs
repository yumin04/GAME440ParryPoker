using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Transform[] slots;

    private void OnEnable()
    {
        GameEvents.OnAllLeversDown += EvaluateSlots;
    }
    private void OnDisable()
    {
        GameEvents.OnAllLeversDown -= EvaluateSlots;
    }
    // Function 1: Get slot value
    // x-rotation 80 ~ 260 : Blue (1)
    // else : Red (0)
    public List<int> GetSlotValues()
    {
        List<int> values = new List<int>();

        foreach (Transform slot in slots)
        {
            float xRotation = slot.eulerAngles.x;

            if (xRotation >= 80f && xRotation <= 260f)
                values.Add(1); // Blue
            else
                values.Add(0); // Red
        }

        return values;
    }

    // Function 2: Evaluate slots and trigger event
    public void EvaluateSlots()
    {
        List<int> values = GetSlotValues();

        GameEvents.OnSlotMachineFinished?.Invoke();
    }
}
