using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class PlayFromFirstScene
{
    // The key used to save the preference in EditorPrefs
    private const string EnableOptionPref = "PlayFromFirstScene_Enabled";
    private const string LastScenePref = "PlayFromFirstScene_LastScene";

    // The menu path
    private const string MenuPath = "Tools/Always Play From Scene 0";

    static PlayFromFirstScene()
    {
        // Subscribe to the play mode state change event
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    [MenuItem(MenuPath)]
    private static void ToggleAction()
    {
        bool isEnabled = EditorPrefs.GetBool(EnableOptionPref, false);
        EditorPrefs.SetBool(EnableOptionPref, !isEnabled);
    }

    [MenuItem(MenuPath, true)]
    private static bool ToggleActionValidate()
    {
        Menu.SetChecked(MenuPath, EditorPrefs.GetBool(EnableOptionPref, false));
        return true;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // Only run logic if the feature is enabled in the menu
        if (!EditorPrefs.GetBool(EnableOptionPref, false))
            return;

        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                // User pressed the Play button, about to enter Play Mode
                SaveCurrentSceneAndSwitchToFirst();
                break;

            case PlayModeStateChange.EnteredEditMode:
                // User pressed Stop, back in Edit Mode
                RestoreLastScene();
                break;
        }
    }

    private static void SaveCurrentSceneAndSwitchToFirst()
    {
        // 1. Save the current open scene so we don't lose work
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // If user cancels saving, cancel play mode
            EditorApplication.isPlaying = false;
            return;
        }

        // 2. Store the path of the currently active scene
        string currentScenePath = SceneManager.GetActiveScene().path;

        // If the scene hasn't been saved yet (Untitled), we can't restore it reliably
        if (string.IsNullOrEmpty(currentScenePath))
        {
            Debug.LogWarning("PlayFromFirstScene: Current scene is not saved. Cannot ensure return to this scene.");
        }
        else
        {
            EditorPrefs.SetString(LastScenePref, currentScenePath);
        }

        // 3. Get the path of the scene at Build Index 0
        string startScenePath = SceneUtility.GetScenePathByBuildIndex(0);

        if (string.IsNullOrEmpty(startScenePath))
        {
            Debug.LogError("PlayFromFirstScene: No scene found at Build Index 0. Please add scenes to Build Settings.");
            EditorApplication.isPlaying = false;
            return;
        }

        // 4. Open the start scene if we aren't already there
        if (currentScenePath != startScenePath)
        {
            EditorSceneManager.OpenScene(startScenePath);
        }
    }

    private static void RestoreLastScene()
    {
        // Check if we have a stored previous scene
        if (EditorPrefs.HasKey(LastScenePref))
        {
            string lastScenePath = EditorPrefs.GetString(LastScenePref);

            // Verify the file still exists before trying to open
            if (!string.IsNullOrEmpty(lastScenePath) && System.IO.File.Exists(lastScenePath))
            {
                EditorSceneManager.OpenScene(lastScenePath);
            }

            // Clean up the key
            EditorPrefs.DeleteKey(LastScenePref);
        }
    }
}