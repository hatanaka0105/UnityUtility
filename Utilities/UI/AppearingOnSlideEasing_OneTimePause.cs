using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UnityCustomExtension.UI
{
    public class AppearingOnSlideEasing_OneTimePause : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _defaultPos;

        [SerializeField]
        private float _firstDuration = 0.5f;

        [SerializeField]
        private Ease _firstEaseType = 0;

        [SerializeField]
        private bool _ignoreTimeScale;

        [SerializeField]
        private Vector3 _pausePos;

        [SerializeField]
        private float _pauseDuration;

        [SerializeField]
        private float _secondDuration = 0.5f;

        [SerializeField]
        private Ease _secondEaseType;

        private Vector3 _firstTargetPos;
        private Vector3 _secondTargetPos;
        private RectTransform _rect;
        private Tween _tween;
        private Tween _waitTween;

        private void OnEnable()
        {
            _firstTargetPos = _pausePos;
            _rect = GetComponent<RectTransform>();
            if (_rect != null)
            {
                _secondTargetPos = _rect.position;
                _rect.localPosition = _defaultPos;

                //アニメーション
                _tween = _rect.DOLocalMove(_firstTargetPos, _firstDuration)
                    .SetEase(_firstEaseType)
                    .SetUpdate(_ignoreTimeScale)
                    .OnComplete(OnComplete);
            }
            else
            {
                _secondTargetPos = transform.position;
                transform.position = transform.position + _defaultPos;

                //アニメーション
                _tween = transform.DOMove(_firstTargetPos, _firstDuration)
                    .SetEase(_firstEaseType)
                    .SetUpdate(_ignoreTimeScale)
                    .OnComplete(OnComplete);
            }
        }

        private void OnDisable()
        {
            // Tween破棄
            if (DOTween.instance != null)
            {
                _tween?.Kill();
                _waitTween?.Kill();
            }
        }

        private void OnComplete()
        {
            _waitTween = DOVirtual.DelayedCall(_pauseDuration, SecondMove);
        }

        private void SecondMove()
        {
            _rect = GetComponent<RectTransform>();
            if (_rect != null)
            {
                //アニメーション
                _tween = _rect.DOMove(_secondTargetPos, _secondDuration)
                    .SetEase(_secondEaseType)
                    .SetUpdate(_ignoreTimeScale);
            }
            else
            {
                //アニメーション
                _tween = transform.DOMove(_secondTargetPos, _secondDuration)
                    .SetEase(_secondEaseType)
                    .SetUpdate(_ignoreTimeScale);
            }
        }
    }
}