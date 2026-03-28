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

    public void OnEnable()
    {
        GameEvents.HideWaitAndAttackPanel += DisablePanel;
    }
    public void OnDisable()
    {
        GameEvents.HideWaitAndAttackPanel -= DisablePanel;
    }
    private void OnKeepClicked()
    {
        GameEvents.OnKeepClicked.Invoke();
    }

    private void OnAttackClicked()  
    {
        GameEvents.OnAttackClicked.Invoke();
    }

    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}