using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UnityCustomExtension
{
    public class BGMIntroLoopConnector : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _firstSource;

        [SerializeField]
        private AudioSource _loopSource;

        [SerializeField]
        private float _buffer;

        [SerializeField]
        private bool _isStartOnAwake;

        private bool _isPlaying;
        private double _introEndTime;
        private float _initVolume;

        private void Awake()
        {
            _firstSource.loop = false;
            _loopSource.loop = true;

            if (_isStartOnAwake)
            {
                Play();
            }
        }

        public void Play()
        {
            _firstSource.Play();
            _introEndTime = AudioSettings.dspTime + _firstSource.clip.length;
            _loopSource.PlayScheduled(_introEndTime + _buffer);
            _isPlaying = true;
        }

        public void PlayFade(float duration)
        {
            _firstSource.DOFade(_firstSource.volume, duration);
            _firstSource.volume = 0f;
            _firstSource.Play();
            _introEndTime = AudioSettings.dspTime + _firstSource.clip.length;
            _loopSource.PlayScheduled(_introEndTime + _buffer);
            _isPlaying = true;
        }

        public void StopFade(float duration)
        {
            if (_firstSource.isPlaying)
            {
                _firstSource.DOFade(0.0f, duration).OnComplete(() => _firstSource.Stop());
            }

            if (_loopSource.isPlaying)
            {
                _loopSource.DOFade(0.0f, duration).OnComplete(() => _loopSource.Stop());
            }
            _isPlaying = false;
        }

        private void Update()
        {
            if (_isPlaying && !_loopSource.isPlaying && AudioSettings.dspTime >= _introEndTime)
            {
                // 必要に応じて微調整を行う
                double timeSinceScheduled = AudioSettings.dspTime - _introEndTime;
                _loopSource.PlayScheduled(AudioSettings.dspTime + _buffer - timeSinceScheduled);
            }
        }

        public void FadeOut(Action onFadeOutComplete, float fadeDuration)
        {
            _initVolume = _firstSource.volume;
            StartCoroutine(FadeOutAndIn(onFadeOutComplete, fadeDuration));
        }

        private IEnumerator FadeOutAndIn(Action onFadeOutComplete, float fadeDuration)
        {
            float fadeOutTime = 0;

            // クロスフェード処理
            while (fadeOutTime < fadeDuration)
            {
                fadeOutTime += Time.deltaTime;
                if (_firstSource.isPlaying && _firstSource.volume > 0.0f)
                {
                    _firstSource.volume = Mathf.Lerp(_initVolume, 0.0f, fadeOutTime / fadeDuration);
                    _loopSource.Stop();
                }
                if (_loopSource.isPlaying)
                {
                    _loopSource.volume = Mathf.Lerp(_initVolume, 0.0f, fadeOutTime / fadeDuration);
                }
                yield return null;
            }

            onFadeOutComplete?.Invoke();

            // フェード完了後の設定
            _firstSource.volume = 0.0f;
            _firstSource.Stop();
            _loopSource.volume = 0.0f;
            _loopSource.Stop();
        }
    }
}
