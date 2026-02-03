using UnityEngine;

public class ApplicationManager : Singleton<ApplicationManager>
{
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("ApplicationManager Awake");
    }
    // Already Declared as Singleton from "GenericHelpers"
    public void QuitApplication()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}