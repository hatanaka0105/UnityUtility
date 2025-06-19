using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;

// DebugAttribute を定義
[AttributeUsage(AttributeTargets.Class)]
public class DebugAttribute : Attribute { }

[CustomEditor(typeof(MonoBehaviour), true)]
public class AutoInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 通常のInspectorを表示
        base.OnInspectorGUI();

        // プレイモード中のみ追加情報を表示
        if (!EditorApplication.isPlaying)
            return;

        // ターゲットオブジェクトの型を取得し、DebugAttributeがついているか確認
        var targetObject = target;
        var objectType = targetObject.GetType();
        if (!Attribute.IsDefined(objectType, typeof(DebugAttribute)))
            return;

        // カスタムデバッグ情報の描画を追加
        DrawCustomInspector(targetObject);
    }

    private void DrawCustomInspector(object targetObject)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Information", EditorStyles.boldLabel);

        // すべての public フィールドを取得して表示
        FieldInfo[] fields = targetObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            object value = field.GetValue(targetObject);
            EditorGUILayout.LabelField($"{field.Name}: {value}");
        }

        // すべての public 関数を取得して、条件に応じて表示
        MethodInfo[] methods = targetObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (MethodInfo method in methods)
        {
            if (method.GetParameters().Length == 0) // 引数なしのメソッドのみ
            {
                Type returnType = method.ReturnType;

                // 戻り値が void の場合はボタン
                if (returnType == typeof(void))
                {
                    if (GUILayout.Button($"Execute {method.Name}"))
                    {
                        method.Invoke(targetObject, null);
                    }
                }
                // 戻り値が float, Vector3, bool, int の場合は LabelField に表示
                else if (returnType == typeof(float) || returnType == typeof(Vector3) ||
                         returnType == typeof(bool) || returnType == typeof(int))
                {
                    object result = method.Invoke(targetObject, null);
                    EditorGUILayout.LabelField($"{method.Name}: {result}");
                }
                // 戻り値が GameObject の場合は GameObject.name を LabelField に表示
                else if (returnType == typeof(GameObject))
                {
                    GameObject result = method.Invoke(targetObject, null) as GameObject;
                    string resultName = result != null ? result.name : "null";
                    EditorGUILayout.LabelField($"{method.Name}: {resultName}");
                }
            }
        }
    }
}
#endif