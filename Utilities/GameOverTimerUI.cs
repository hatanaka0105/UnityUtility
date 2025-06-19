using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UnityCustomExtension.UI
{
    public class GameOverTimerUI : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        private bool _isCountTime = false;
        private float _currentTime = 0;

        public UnityEvent OnTimeOver;

        public void SetupBar(float duration)
        {
            _currentTime = _slider.value = _slider.maxValue = duration;
            _isCountTime = true;
        }

        public void CancelTimer()
        {
            _isCountTime = false;
        }

        private void Update()
        {
            if (_isCountTime)
            {
                _currentTime -= Time.deltaTime;
                _slider.value = _currentTime;
                if (_currentTime <= 0.0f)
                {
                    _isCountTime = false;
                    OnTimeOver?.Invoke();
                }
            }
        }
    }
}