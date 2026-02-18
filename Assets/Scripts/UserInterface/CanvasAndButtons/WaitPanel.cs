using System;
using UnityEngine;
using UnityEngine.UI;


public class WaitPanel : MonoBehaviour
{


    public void Awake()
    {
        Debug.Log("Wait.Awake");
        // gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        GameEvents.OnKeepClicked += DisablePanel;
        GameEvents.OnAttackClicked += DisablePanel;
    }

    public void OnDisable()
    {
        GameEvents.OnKeepClicked -= DisablePanel;
        GameEvents.OnAttackClicked -= DisablePanel;
    }
    
    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}