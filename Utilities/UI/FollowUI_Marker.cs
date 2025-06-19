using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCustomExtension.UI
{
    public class FollowUI_Marker : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _target;

        [SerializeField]
        private GameObject _followTarget;

        [SerializeField]
        private Vector3 _offset;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private LayerMask _layer;

        private RectTransform _parent;

        private void Awake()
        {
            _parent = _target.parent.GetComponent<RectTransform>();
        }

        public void InitializeMarker(Color color, GameObject target)
        {
            _image.color = color;
            _followTarget = target;
        }

        private void Update()
        {
            if (_followTarget == null)
            {
                return;
            }
            bool isShowMarker 
                = !Camera.main.transform.IsNoObstacleBetweenTarget(
                    _followTarget
                    , Vector3.up
                    , _layer);

            if (isShowMarker)
            {
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
            else
            {
                gameObject.transform.localScale = Vector3.zero;
            }
        }

#if UNITY_EDITOR
        [SerializeField, Header("持つ操作の当たり判定をデバッグ表示するか")]
        bool isDebugEnable = false;

        void OnDrawGizmos()
        {
            if (isDebugEnable == false)
            {
                return;
            }

            var isHit = Physics.Raycast(Camera.main.transform.position, _followTarget.transform.position, out RaycastHit hit);
            if (isHit)
            {
                if(hit.collider.gameObject == _followTarget)
                {
                    Vector3 posDelta = _followTarget.transform.position - Camera.main.transform.position;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(Camera.main.transform.position, posDelta * hit.distance);
                }
                else
                {
                    Vector3 posDelta = _followTarget.transform.position - Camera.main.transform.position;

                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(Camera.main.transform.position, posDelta * hit.distance);
                }
            }
            else
            {
                Vector3 posDelta = _followTarget.transform.position - Camera.main.transform.position;

                Gizmos.color = Color.red;
                Gizmos.DrawRay(Camera.main.transform.position, posDelta * hit.distance);
            }
        }
#endif
    }
}