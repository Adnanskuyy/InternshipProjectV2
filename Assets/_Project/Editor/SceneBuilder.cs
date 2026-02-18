using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneBuilder
{
    [MenuItem("Tools/Build Prediction Scene")]
    public static void BuildPredictionScene()
    {
        // 1. Create a new scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        
        // 2. Create Camera
        GameObject cameraObj = new GameObject("Main Camera");
        Camera cam = cameraObj.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.2f, 0.2f, 0.2f); // Dark background
        cameraObj.tag = "MainCamera";

        // 3. Create UI Document
        GameObject uiObj = new GameObject("Prediction UI");
        UIDocument uiDoc = uiObj.AddComponent<UIDocument>();
        
        // Load Assets
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Project/UI/Layouts/PredictionLayout.uxml");
        PanelSettings panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>("Assets/UI Toolkit/PanelSettings.asset"); 
        // Note: PanelSettings might not exist there, so we check or create one.
        
        if (panelSettings == null)
        {
            // Try finding any panel settings
            string[] guids = AssetDatabase.FindAssets("t:PanelSettings");
            if (guids.Length > 0)
            {
                panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }
        }

        uiDoc.visualTreeAsset = visualTree;
        uiDoc.panelSettings = panelSettings;

        // 4. Add Controller
        PredictionUIController controller = uiObj.AddComponent<PredictionUIController>();
        
        // Find GameSessionSO
        string[] sessionGuids = AssetDatabase.FindAssets("t:GameSessionSO");
        if (sessionGuids.Length > 0)
        {
            GameSessionSO session = AssetDatabase.LoadAssetAtPath<GameSessionSO>(AssetDatabase.GUIDToAssetPath(sessionGuids[0]));
            // We use SerializedObject to set private fields if needed, but here we can just use the public property setter if we exposed it,
            // or use SerializedObject.
            SerializedObject serializedController = new SerializedObject(controller);
            serializedController.FindProperty("gameSession").objectReferenceValue = session;
            serializedController.FindProperty("uiDocument").objectReferenceValue = uiDoc;
            serializedController.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogWarning("SceneBuilder: No GameSessionSO found to assign.");
        }

        // 5. Save Scene
        string scenePath = "Assets/_Project/Scenes/PredictionScene.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log($"SceneBuilder: Saved scene to {scenePath}");
        
        // Add to Build Settings (Optional)
        // var original = EditorBuildSettings.scenes;
        // var newSettings = new EditorBuildSettingsScene[original.Length + 1];
        // System.Array.Copy(original, newSettings, original.Length);
        // newSettings[newSettings.Length - 1] = new EditorBuildSettingsScene(scenePath, true);
        // EditorBuildSettings.scenes = newSettings;
    }
}
