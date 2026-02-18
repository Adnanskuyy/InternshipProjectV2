using UnityEngine;
using UnityEditor;

public class MaterialFactory
{
    [MenuItem("Tools/Create Fluid Material")]
    public static void CreateFluidMat()
    {
        Shader shader = Shader.Find("Custom/FluidDistortion");
        if (shader == null)
        {
            Debug.LogError("Shader 'Custom/FluidDistortion' not found!");
            return;
        }

        Material mat = new Material(shader);
        AssetDatabase.CreateAsset(mat, "Assets/_Project/Materials/FluidBackground.mat");
        AssetDatabase.SaveAssets();
        Debug.Log("Created FluidBackground.mat");
    }
}
