#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Applies the ParryPokerENV prefab instance in RoundScene onto the prefab asset
/// (same as selecting the instance and choosing Overrides → Apply All).
/// </summary>
public static class ApplyParryPokerEnvFromRoundScene
{
    const string ScenePath = "Assets/Scenes/RoundScene.unity";
    const string PrefabAssetPath = "Assets/Prefabs/ParryPokerENV.prefab";

    [MenuItem("Tools/Apply ParryPokerENV From RoundScene")]
    public static void ApplyFromMenu()
    {
        if (!ApplyInternal())
            Debug.LogError("Apply ParryPokerENV: failed (see previous errors).");
        else
            Debug.Log("Apply ParryPokerENV: prefab updated from RoundScene.");
    }

    /// <summary>For Unity batch mode: -executeMethod ApplyParryPokerEnvFromRoundScene.ApplyFromBatchMode</summary>
    public static void ApplyFromBatchMode()
    {
        try
        {
            if (!ApplyInternal())
                EditorApplication.Exit(1);
            else
                EditorApplication.Exit(0);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            EditorApplication.Exit(1);
        }
    }

    static bool ApplyInternal()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        GameObject instanceRoot = null;
        foreach (var root in scene.GetRootGameObjects())
        {
            if (PrefabUtility.GetPrefabInstanceStatus(root) != PrefabInstanceStatus.Connected)
                continue;
            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(root);
            if (path == PrefabAssetPath)
            {
                instanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(root);
                break;
            }
        }

        if (instanceRoot == null)
        {
            Debug.LogError("Apply ParryPokerENV: no root prefab instance matching " + PrefabAssetPath + " in " + ScenePath);
            return false;
        }

        PrefabUtility.ApplyPrefabInstance(instanceRoot, InteractionMode.AutomatedAction);
        AssetDatabase.SaveAssets();
        return true;
    }
}
#endif
