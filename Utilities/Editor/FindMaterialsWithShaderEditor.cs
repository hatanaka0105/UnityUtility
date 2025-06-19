#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FindMaterialsWithShaderEditor : EditorWindow
{
    private Shader _shaderToFind;
    private List<Material> _matchingMaterials = new List<Material>();
    private Vector2 _scrollPos;

    [MenuItem("Tools/Find Materials With Shader")]
    public static void ShowWindow()
    {
        GetWindow<FindMaterialsWithShaderEditor>("Find Materials With Shader");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Search for Materials with Shader", EditorStyles.boldLabel);

        _shaderToFind = (Shader)EditorGUILayout.ObjectField("Shader", _shaderToFind, typeof(Shader), false);

        if (_shaderToFind != null && GUILayout.Button("Search"))
        {
            FindMaterialsWithShader();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Results:", EditorStyles.boldLabel);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        foreach (var mat in _matchingMaterials)
        {
            if (mat == null) continue;
            EditorGUILayout.ObjectField(mat.name, mat, typeof(Material), false);
        }
        EditorGUILayout.EndScrollView();
    }

    private void FindMaterialsWithShader()
    {
        _matchingMaterials.Clear();

        if (_shaderToFind == null)
        {
            Debug.LogWarning("Shader is null.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && mat.shader == _shaderToFind)
            {
                _matchingMaterials.Add(mat);
            }
        }

        Debug.Log($"Found {_matchingMaterials.Count} materials using shader {_shaderToFind.name}.");
    }
}
#endif