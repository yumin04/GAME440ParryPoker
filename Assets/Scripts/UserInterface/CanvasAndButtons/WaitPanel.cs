using System;
using UnityEngine;
using UnityEngine.UI;


public class WaitPanel : MonoBehaviour
{
    public void OnEnable()
    {
        GameEvents.OnSubRoundEnd += DisablePanel;
        // If keep clicked, it will come to SubRoundEnd naturally, so no need to add that
        
        // If attack is clicked, it needs to move on to attack phase
        // GameEvents.OnAttackClicked += DisablePanel;
    }

    public void OnDisable()
    {
        GameEvents.OnSubRoundEnd -= DisablePanel;
        // GameEvents.OnAttackClicked -= DisablePanel;
    }
    
    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}