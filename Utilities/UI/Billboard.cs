using UnityEngine;

namespace UnityCustomExtension.UI
{
    /// <summary>
    /// 常にカメラの方を向くオブジェクト回転をカメラに固定
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        void LateUpdate()
        {
            if(transform != null && Camera.main != null)
            {
                // 回転をカメラと同期させる
                transform.rotation = Camera.main.transform.rotation;
            }
        }
    }
}