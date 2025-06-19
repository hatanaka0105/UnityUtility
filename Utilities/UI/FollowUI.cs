using UnityEngine;

namespace UnityCustomExtension.UI
{
    public class FollowUI : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _target;

        [SerializeField]
        private GameObject _followTarget;

        [SerializeField]
        private Vector3 _offset;

        private RectTransform _parent;

        private void Awake()
        {
            _parent = _target.parent.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_followTarget == null)
            {
                return;
            }

            gameObject.transform.localScale = Vector3.one;
            // オブジェクトのワールド座標→スクリーン座標変換
            var targetScreenPos = Camera.main.WorldToScreenPoint(_offset + _followTarget.transform.position);

            // スクリーン座標→UIローカル座標変換
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parent,
                targetScreenPos,
                null, // オーバーレイモードの場合はnull
                out var uiLocalPos
            );

            // RectTransformのローカル座標を更新
            _target.localPosition = uiLocalPos;
        }

        public void SetFollowTarget(GameObject target)
        {
            _followTarget = target;
        }
    }
}