using UnityEngine;

/// <summary>
/// 親の位置を追従したいが、回転は追従させたくないとき用
/// </summary>
public class FollowWithoutRotation : MonoBehaviour
{
    private Transform parentTransform;
    private Vector3 offset;

    void Start()
    {
        parentTransform = transform.parent;
        offset = transform.position - parentTransform.position;
        transform.SetParent(null); // 親子関係を解除
    }

    void Update()
    {
        if (parentTransform != null)
        {
            transform.position = parentTransform.position + offset;
        }
    }
}