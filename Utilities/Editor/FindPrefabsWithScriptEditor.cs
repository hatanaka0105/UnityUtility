#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FindPrefabsWithScriptEditor : EditorWindow
{
    private MonoScript _scriptToFind;
    private List<GameObject> _matchingPrefabs = new List<GameObject>();
    private Vector2 _scrollPos;

    [MenuItem("Tools/Find Prefabs With Script")]
    public static void ShowWindow()
    {
        GetWindow<FindPrefabsWithScriptEditor>("Find Prefabs With Script");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Search for Prefabs with Script", EditorStyles.boldLabel);

        _scriptToFind = (MonoScript)EditorGUILayout.ObjectField("Script", _scriptToFind, typeof(MonoScript), false);

        if (_scriptToFind != null && GUILayout.Button("Search"))
        {
            FindPrefabsWithScript();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Results:", EditorStyles.boldLabel);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        foreach (var prefab in _matchingPrefabs)
        {
            if (prefab == null) continue;

            EditorGUILayout.ObjectField(prefab.name, prefab, typeof(GameObject), false);
        }
        EditorGUILayout.EndScrollView();
    }

    private void FindPrefabsWithScript()
    {
        _matchingPrefabs.Clear();

        string scriptClassName = _scriptToFind.GetClass()?.Name;
        if (string.IsNullOrEmpty(scriptClassName))
        {
            Debug.LogWarning("Script class could not be determined.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null) continue;

            Component[] components = prefab.GetComponentsInChildren<Component>(true);
            foreach (Component comp in components)
            {
                if (comp == null) continue; // Missing script対策
                if (comp.GetType() == _scriptToFind.GetClass())
                {
                    _matchingPrefabs.Add(prefab);
                    break;
                }
            }
        }

        Debug.Log($"Found {_matchingPrefabs.Count} prefabs with script {_scriptToFind.GetClass().Name}.");
    }
}
#endif