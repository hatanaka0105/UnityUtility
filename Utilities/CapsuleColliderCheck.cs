using UnityEngine;

namespace UnityCustomExtension
{
    public class CapsuleColliderCheck
    {
        private CapsuleCollider capsuleCollider;
        private Vector3 cachedCenter;
        private float cachedRadius;
        private float cachedHeight;

        public CapsuleColliderCheck(CapsuleCollider collider)
        {
            capsuleCollider = collider;
            CacheColliderInfo();
        }

        private void CacheColliderInfo()
        {
            cachedCenter = capsuleCollider.center;
            cachedRadius = capsuleCollider.radius;
            cachedHeight = capsuleCollider.height;
        }

        public bool CheckCapsuleCollision()
        {
            if (capsuleCollider == null)
            {
#if UNITY_EDITOR
                Debug.LogError("CapsuleCollider is null!");
#endif
                return false;
            }

            // キャッシュされた値を使う
            if (capsuleCollider.center != cachedCenter || capsuleCollider.radius != cachedRadius || capsuleCollider.height != cachedHeight)
            {
                CacheColliderInfo(); // キャッシュが無効な場合はキャッシュを更新
            }

            // CapsuleColliderの情報を元に衝突判定を行う
            Vector3 point1 = capsuleCollider.transform.TransformPoint(cachedCenter + Vector3.up * (cachedHeight * 0.5f - cachedRadius));
            Vector3 point2 = capsuleCollider.transform.TransformPoint(cachedCenter - Vector3.up * (cachedHeight * 0.5f - cachedRadius));

            // 他のオブジェクトとの衝突判定を行う
            return Physics.CheckCapsule(point1, point2, cachedRadius, capsuleCollider.gameObject.layer, QueryTriggerInteraction.Ignore);
        }
    }
}