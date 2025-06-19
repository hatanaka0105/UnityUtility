using System;
using UniRx;
using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// 定期的に圧力を加えないと自然に減衰する内部ゲージ
    /// ポンプなどのギミックに
    /// </summary>
    public class PowerPressure : IPowerPressure
    {
        private float _currentPressure = 0f;
        private float _maxPressure = 100f;
        private float _minPressure = 0f;
        private float _waitDecrease = 1f;
        private float _decreaseRate = 1f;
        private IDisposable _decreaseTimer;
        private bool _isDecreasing = false;

        public void Setup(float max = 100f, float min = 0f, float waitDecrease = 1f, float decreaseRate = 1f)
        {
            _maxPressure = max;
            _minPressure = min;
            _waitDecrease = waitDecrease;
            _decreaseRate = decreaseRate;
            ResetTimer();
        }

        public void Tick()
        {
            if (_isDecreasing)
            {
                Decrease();
            }
        }

        public void SendPower(float power)
        {
            _currentPressure = Mathf.Clamp(_currentPressure + power, _minPressure, _maxPressure);
            ResetTimer();
            _isDecreasing = false;
        }

        public void Decrease()
        {
            _currentPressure = Mathf.Max(_currentPressure - _decreaseRate, _minPressure);
        }

        private void ResetTimer()
        {
            _decreaseTimer?.Dispose(); // 既存のタイマーをリセット
            _decreaseTimer = Observable.Timer(TimeSpan.FromSeconds(_waitDecrease))
                .Subscribe(_ => _isDecreasing = true);
        }

        public void Reset()
        {
            _currentPressure = _minPressure;
            _decreaseTimer?.Dispose();
        }

        public float GetPower()
        {
            return _currentPressure;
        }
    }
}
