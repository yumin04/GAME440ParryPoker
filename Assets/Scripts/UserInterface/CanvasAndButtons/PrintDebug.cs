using System;
using UnityEngine;
using UnityEngine.UI;


public class PrintDebug : MonoBehaviour
{
    [SerializeField] private Button startButton;

    public void Awake()
    {
        Debug.Log("Awake");
    }
    
    public void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);
    }

    private void OnStartClicked()
    {
        Debug.Log("StartButtonClicked But a little bit of twist");
    }
}
