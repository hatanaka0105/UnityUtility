#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LockScale : MonoBehaviour
{
}
#if UNITY_EDITOR
[CustomEditor(typeof(LockScale))]
public class LockScaleEditor : Editor
{
    private void OnEnable()
    {
        Transform transform = ((LockScale)target).transform;
    }

    public override void OnInspectorGUI()
    {
        // デフォルトのTransform Inspectorを表示
        DrawDefaultInspector();

        GUILayout.Label("このオブジェクトはスケール禁止です");

        // ロック対象の場合のみ位置を元に戻す
        if (target is LockScale lockTrans)
        {
            if (lockTrans.transform.localScale != Vector3.one)
            {
                lockTrans.transform.localScale = Vector3.one;
            }
        }
    }
}
#endif