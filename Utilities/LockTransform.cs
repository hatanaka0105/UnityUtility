#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LockTransform : MonoBehaviour
{
}
#if UNITY_EDITOR
[CustomEditor(typeof(LockTransform))]
public class LockTransformEditor : Editor
{
    private Vector3 originalPosition;

    private void OnEnable()
    {
        LockTransform transform = (LockTransform)target;

        originalPosition = transform.transform.position; // 初期位置を保存
    }

    public override void OnInspectorGUI()
    {
        // デフォルトのTransform Inspectorを表示
        DrawDefaultInspector();

        GUILayout.Label("このオブジェクトは移動禁止です");

        // ロック対象の場合のみ位置を元に戻す
        if (target is LockTransform lockTrans)
        {
            if (lockTrans.transform.position != originalPosition)
            {
                lockTrans.transform.position = originalPosition;
            }
        }
    }
}
#endif