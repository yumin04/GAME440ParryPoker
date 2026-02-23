using System;
using UnityEngine;
using UnityEngine.UI;


public class PriorityPanel : MonoBehaviour
{

    [Header("Attack Keep button")]
    [SerializeField] private Button attackButton;
    [SerializeField] private Button keepButton;
    
    public void Start()
    {
        attackButton.onClick.AddListener(OnAttackClicked);
        keepButton.onClick.AddListener(OnKeepClicked);
    }
    

    private void OnKeepClicked()
    {
        GameEvents.OnKeepClicked.Invoke();
        DisablePanel();
    }

    private void OnAttackClicked()
    {
        GameEvents.OnAttackClicked.Invoke();
        DisablePanel();
    }

    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}