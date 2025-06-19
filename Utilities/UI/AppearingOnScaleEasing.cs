using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UnityCustomExtension.UI
{
    /// <summary>
    /// これをつけると登場するとき拡縮でニュッて出てくるようになる
    /// 細かいことはできない
    /// アニメーションと違ってTransformに従属する
    /// </summary>
    public class AppearingOnScaleEasing : MonoBehaviour
    {
        [SerializeField]
        private float _scale = 1f;

        [SerializeField]
        private float _scaleDuration = 0.5f;

        [SerializeField]
        private Ease _easeType = 0;

        [SerializeField]
        private bool _ignoreTimeScale;

        private Tween _tween;

        private void OnEnable()
        {
            var button = gameObject.GetComponent<Button>();
            if(button != null)
            {
                button.enabled = false;
            }

            //アニメーション
            transform.localScale = Vector3.zero;
            _tween = transform.DOScale(Vector3.one * _scale, _scaleDuration)
                .SetEase(_easeType)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(OnComplete);
        }

        private void OnDisable()
        {
            // Tween破棄
            if (DOTween.instance != null)
            {
                _tween?.Kill();
            }
        }

        private void OnComplete()
        {
            if (gameObject != null)
            {
                gameObject.GetComponent<Button>().enabled = true;
            }
        }
    }
}