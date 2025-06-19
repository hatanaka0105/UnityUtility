using System;
using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// 汎用クラス
    /// 指定した時間に到達したらコールバック
    /// TickをMonoBehaviour上などで必ず呼ぶこと
    /// 1回発動したら停止するので任意のタイミングでもう一回StartTimer()を呼ぶ
    /// </summary>
    public class TimerAction : ITickEvent
    {
        private float _timer;
        private float _waitTime;
        private Action _callback;
        private bool _isStarted;

        /// <summary>
        /// Timer初期化
        /// </summary>
        /// <param name="interval">秒間</param>
        /// <param name="callback">タイマー経過時に</param>
        public TimerAction(float waitTime, Action callback)
        {
            _waitTime = waitTime;
            _callback = callback;
            _timer = 0.0f;
            _isStarted = true;
        }

        public void StartTimer()
        {
            _timer = 0.0f;
            _isStarted = true;
        }

        public void Reset()
        {
            _timer = 0.0f;
            _isStarted = true;
        }

        public void Tick()
        {
            if (_isStarted)
            {
                _timer += Time.deltaTime;
                if (_timer >= _waitTime)
                {
                    _callback?.Invoke();
                    _timer = 0f;
                    _isStarted = false;
                }
            }
        }

        public void Stop()
        {

            _isStarted = false;
        }

        public void Complete()
        {
            _callback?.Invoke();
            _timer = 0f;
        }
    }
}