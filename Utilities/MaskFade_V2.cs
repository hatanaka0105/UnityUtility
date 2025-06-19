using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityCustomExtension.Audio;
using UnityEditor;
using LittleCheeseWorks;

namespace UnityCustomExtension.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class MaskFade_V2 : SingletonMonoBehaviour<MaskFade_V2>
    {
        [SerializeField, Header("暗転した状態から始める")]
        private bool _firstBlackout;

        [SerializeField, Header("動かす画像")]
        private Image _image;

        [SerializeField, Header("フェードにかかる時間")]
        private float _fadeDuration;

        [SerializeField, Header("フェードアウト時の目標スケール")]
        private float _scaleDefault;

        [SerializeField, Header("背景(黒部分)")]
        private Image _backImage;

        private AudioSource _source;

        private void Awake()
        {
            Cleanup();
            if (_scaleDefault > 0)
            {
            }
            else
            {
                _scaleDefault = _image.transform.localScale.x;
            }

            if (_firstBlackout)
            {
                SetScaleZero();
                Debug.Log("_image.transform.localScale:" + _image.transform.localScale);
            }
            _source = GetComponent<AudioSource>();
        }

        public void SetScaleZero()
        {
            _image.transform.localScale = Vector3.zero;
        }

        public void FadeIn(System.Action onFadeComplete = null)
        {
            _image.transform.DOScale(Vector3.zero, _fadeDuration)
                .OnComplete(() => { onFadeComplete?.Invoke(); });

            AudioClipDictionary.Instance?.Play("FadeIn", _source);
        }

        public void FadeOut(System.Action onFadeComplete = null)
        {
            _image.transform.DOScale(Vector3.one * _scaleDefault, _fadeDuration)
                .OnComplete(() => { onFadeComplete?.Invoke(); });

            AudioClipDictionary.Instance?.Play("FadeOut", _source);
        }

        public void Cleanup()
        {
            _backImage.color = new Color(1, 1, 1, 1);
        }

        public void Invisible()
        {
            _backImage.color = new Color(0, 0, 0, 0);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!EditorApplication.isPlaying)
            {
                Invisible();
            }
        }

        [ExecuteAlways]
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        [ExecuteAlways]
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= PlayModeStateChanged;
            Cleanup();
        }

        private void PlayModeStateChanged(PlayModeStateChange state)
        {
            if(state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.ExitingPlayMode)
            {
                Invisible();
            }
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.EnteredPlayMode)
            {
                Cleanup();
            }
        }
#endif
    }
}