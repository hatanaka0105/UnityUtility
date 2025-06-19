using System;
using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// 汎用クラス
    /// 指定した時間に到達する毎にコールバックを実行する
    /// TickをMonoBehaviour上などで必ず呼ぶこと
    /// </summary>
    public class ScheduledAction : ITickEvent
    {
        private float _timer;
        private float _interval;
        private Action _callback;
        private bool _isStarted;

        /// <summary>
        /// Timer初期化
        /// </summary>
        /// <param name="interval">秒間</param>
        /// <param name="callback">タイマー経過時に</param>
        /// <param name="isOnce">一回だけ実行する</param>
        public ScheduledAction(float interval, Action callback)
        {
            _interval = interval;
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
        }

        public void Tick()
        {
            if (_isStarted)
            {
                _timer += Time.deltaTime;
                if (_timer >= _interval)
                {
                    _callback?.Invoke();
                    _timer = 0f;
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