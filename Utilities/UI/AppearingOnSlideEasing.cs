using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityCustomExtension.UI
{
    /// <summary>
    /// これをつけると登場するとき横からニュッて出てくるようになる
    /// 細かいことはできない
    /// アニメーションと違ってTransformに従属する
    /// </summary>
    public class AppearingOnSlideEasing : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _defaultPos;

        [SerializeField]
        private float _duration = 0.5f;

        [SerializeField]
        private Ease _easeType = 0;

        [SerializeField]
        private bool _ignoreTimeScale;

        [SerializeField]
        private Ease _inverseEaseType = 0;

        [SerializeField]
        private UnityEvent _onEaseComplete;

        private Vector3 _targetPos;
        private RectTransform _rect;
        private Button _button;
        private Vector3 _setDefaultPos;

        private Button[] _buttons;

        private void OnEnable()
        {
            if (_buttons == null)
            {
                _buttons = GetComponentsInChildren<Button>();
            }
            if(_buttons != null)
            {
                foreach(var button in _buttons)
                {
                    button.interactable = false;
                }
            }
            _button = GetComponent<Button>();
            if (_button != null)
            {
                _button.enabled = false;
            }

            _rect = GetComponent<RectTransform>();
            _setDefaultPos = _rect.position;
            if (_rect != null)
            {
                _targetPos = _rect.position;
                _rect.position = _rect.position + _defaultPos;

                //アニメーション
                _rect.DOMove(_targetPos, _duration)
                    .SetEase(_easeType)
                    .SetUpdate(_ignoreTimeScale)
                    .OnComplete(OnComplete);
            }
            else
            {
                _targetPos = transform.position;
                transform.position = transform.position + _defaultPos;

                //アニメーション
                transform.DOMove(_targetPos, _duration)
                    .SetEase(_easeType)
                    .SetUpdate(_ignoreTimeScale)
                    .OnComplete(OnComplete);
            }
        }

        private void OnDisable()
        {
            if (_buttons == null)
            {
                _buttons = GetComponentsInChildren<Button>();
            }
            if (_buttons != null)
            {
                foreach (var button in _buttons)
                {
                    button.interactable = false;
                }
            }
        }

        private void OnComplete()
        {
            if(_button != null)
            {
                _button.enabled = true;
            }

            if (_buttons == null)
            {
                _buttons = GetComponentsInChildren<Button>();
            }
            if (_buttons != null)
            {
                foreach (var button in _buttons)
                {
                    button.interactable = true;
                }
            }
            _onEaseComplete?.Invoke();
        }

        public void Invert()
        {
            if (_buttons == null)
            {
                _buttons = GetComponentsInChildren<Button>();
            }
            if (_buttons != null)
            {
                foreach (var button in _buttons)
                {
                    button.interactable = false;
                }
            }
            if (_button != null)
            {
                _button.enabled = false;
            }
            if (_rect != null)
            {
                //アニメーション
                _rect.DOMove(_rect.position + _defaultPos, _duration)
                    .SetEase(_inverseEaseType)
                    .SetUpdate(_ignoreTimeScale)
                    .OnComplete(OnCompleteInvert);
            }
            else
            {
                //アニメーション
                transform.DOMove(transform.position + _defaultPos, _duration)
                    .SetEase(_inverseEaseType)
                    .SetUpdate(_ignoreTimeScale)
                    .OnComplete(OnCompleteInvert);
            }
        }

        private void OnCompleteInvert()
        {
            gameObject.SetActive(false);
            _rect.transform.position = _setDefaultPos;
        }
    }
}