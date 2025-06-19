using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UnityCustomExtension
{
    /// <summary>
    /// フェードイン、アウトの汎用機能
    /// 新しいフェード方法を実装したいときは基本的にはストラテジーパターンで実装
    /// IFadeを継承して、入り口を作るだけでいい
    /// </summary>
    public class FadeInOut : MonoBehaviour
    {
        [SerializeField]
        private Image _fade;

        private IFade _fadeStrategy;

        public void StartFadeIn(float speed, System.Action onComplete = null)
        {
            _fadeStrategy = new FadeStrategy_In();
            _fadeStrategy.FadeStart(_fade, speed, onComplete);
        }

        public void StartFadeOut(float speed, System.Action onComplete = null)
        {
            _fadeStrategy = new FadeStrategy_Out();
            _fadeStrategy.FadeStart(_fade, speed, onComplete);
        }

        void Update()
        {
            if (_fadeStrategy != null)
            {
                _fadeStrategy.FadeUpdate();
            }
        }

        /// <summary>
        /// フェード用インターフェイス
        /// </summary>
        public interface IFade
        {
            void FadeStart(Image image, float speedModifier, System.Action onComplete);
            void FadeUpdate();
        }

        public class FadeStrategy_Out : IFade
        {
            private bool _isFading;
            private System.Action _onComplete;
            private Image _image;
            private float _speedModifier;

            public void FadeStart(Image image, float speedModifier, System.Action onComplete)
            {
                _isFading = true;
                _onComplete = onComplete;
                _image = image;
                var color = _image.color;
                color.a = 1f;
                _image.color = color;
                _speedModifier = speedModifier;
                image.gameObject.SetActive(true);
            }

            public void FadeUpdate()
            {
                if(_isFading)
                {
                    var color = _image.color;
                    color.a -= Time.deltaTime * _speedModifier;
                    _image.color = color;
                    if(_image.color.a <= 0f)
                    {
                        _onComplete?.Invoke();
                        _isFading = false;
                    }
                }
            }
        }

        public class FadeStrategy_In : IFade
        {
            private bool _isFading;
            private System.Action _onComplete;
            private Image _image;
            private float _speedModifier;

            public void FadeStart(Image image, float speedModifier, System.Action onComplete)
            {
                _isFading = true;
                _onComplete = onComplete;
                _image = image;
                var color = _image.color;
                color.a = 0f;
                _image.color = color;
                _speedModifier = speedModifier;
                image.gameObject.SetActive(true);
            }

            public void FadeUpdate()
            {
                if (_isFading)
                {
                    var color = _image.color;
                    color.a += Time.deltaTime * _speedModifier;
                    _image.color = color;
                    if (_image.color.a >= 1f)
                    {
                        _onComplete?.Invoke();
                        _isFading = false;
                    }
                }
            }
        }
    }
}