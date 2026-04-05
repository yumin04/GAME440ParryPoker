using UnityEngine;

public class SlotMachineInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject slotMachinePrefab;

    private GameObject currentSlotMachine;
    
    public void InstantiateSlotMachine()
    {
        currentSlotMachine = Instantiate(
            slotMachinePrefab,
            transform
        );
    }

    public void DestroySlotMachine()
    {
        if (currentSlotMachine != null)
        {
            Destroy(currentSlotMachine);
            currentSlotMachine = null;
        }
    }
}