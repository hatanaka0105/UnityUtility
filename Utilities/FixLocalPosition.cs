#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class FixLocalPosition : MonoBehaviour
{
    private void Awake()
    {
        transform.localPosition = Vector3.zero;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FixLocalPosition))]
public class FixLocalPositionEditor : Editor
{
    private Vector3 originalPosition;

    private void OnEnable()
    {
        FixLocalPosition transform = (FixLocalPosition)target;

        originalPosition = Vector3.zero; // 初期位置を保存
    }

    public override void OnInspectorGUI()
    {
        // デフォルトのTransform Inspectorを表示
        DrawDefaultInspector();

        GUILayout.Label("このオブジェクトは移動禁止です");

        // ロック対象の場合のみ位置を元に戻す
        if (target is LockTransform lockTrans)
        {
            if (lockTrans.transform.localPosition != originalPosition)
            {
                lockTrans.transform.localPosition = originalPosition;
            }
        }
    }
}
#endif