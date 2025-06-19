using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TimeBarUI : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    private Slider _slider;

    private Tween _tween;

    private void OnEnable()
    {
        _slider.value = 1f;
        StartSlider();
    }

    private void OnDisable()
    {
        // Tween破棄
        if (DOTween.instance != null)
        {
            _tween?.Kill();
        }
    }

    private void StartSlider()
    {
        _tween = DOVirtual.Float(1f, 0, _duration, OnValueUpdate)
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }

    private void OnValueUpdate(float val)
    {
        _slider.value = val;
    }

    public void SetSliderDuration(float duration)
    {
        _slider.value = 1f;
        _duration = duration;
        StartSlider();
    }
}
