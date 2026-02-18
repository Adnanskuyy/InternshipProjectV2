using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneBuilder
{
    [MenuItem("Tools/Build Play Scene")]
    public static void BuildPlayScene()
    {
        // 1. Create a new scene
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        
        // 2. Setup Camera
        GameObject cameraObj = new GameObject("Main Camera");
        Camera cam = cameraObj.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cameraObj.tag = "MainCamera";
        cameraObj.transform.position = new Vector3(0, 0, -10);

        // 3. Create Fluid Background (Quad)
        GameObject bgObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        bgObj.name = "FluidBackground";
        bgObj.transform.position = new Vector3(0, 0, 5);
        bgObj.transform.localScale = new Vector3(20, 12, 1); // Scale to fill 16:9 view roughly
        
        Material fluidMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/FluidBackground.mat");
        if (fluidMat != null)
            bgObj.GetComponent<Renderer>().material = fluidMat;
        else
            Debug.LogWarning("Fluid material not found!");

        // 4. Setup UI Toolkit
        GameObject uiObj = new GameObject("UI Document");
        UIDocument uiDoc = uiObj.AddComponent<UIDocument>();
        
        VisualTreeAsset layout = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Project/UI/Layouts/PlayLayout.uxml");
        uiDoc.visualTreeAsset = layout;

        // 5. Setup Controller
        PlayUIController controller = uiObj.AddComponent<PlayUIController>();
        
        // Assign Data
        string[] guids = AssetDatabase.FindAssets("t:GameSessionSO");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            GameSessionSO so = AssetDatabase.LoadAssetAtPath<GameSessionSO>(path);
            
            SerializedObject serializedController = new SerializedObject(controller);
            serializedController.FindProperty("sessionData").objectReferenceValue = so;
            
            // Assign Template
            VisualTreeAsset cardTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Project/UI/Layouts/SuspectCard.uxml");
            serializedController.FindProperty("suspectCardTemplate").objectReferenceValue = cardTemplate;
            
            serializedController.ApplyModifiedProperties();
        }

        EditorSceneManager.SaveScene(scene, "Assets/_Project/Scenes/PlayScene.unity");
        Debug.Log("PlayScene built successfully!");
    }

    [MenuItem("Tools/Build Prediction Scene")]
    public static void BuildPredictionScene()
    {
       // Kept for legacy if needed, but we are focusing on PlayScene
       Debug.Log("Legacy build command.");
    }
}
