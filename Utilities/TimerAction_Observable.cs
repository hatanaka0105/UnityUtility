using System;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System.Collections;

namespace UnityCustomExtension
{
    /// <summary>
    /// TimerActionのObservable版
    /// 指定した時間に到達したらコールバック
    /// TickをMonoBehaviour上などで必ず呼ぶこと
    /// 1回発動したら停止するので任意のタイミングでもう一回StartTimer()を呼ぶ
    /// </summary>
    public class TimerAction_Observable
    {
        private IDisposable _timer;
        private float _waitTime;
        private Action _callback;

        /// <summary>
        /// Timer初期化
        /// </summary>
        /// <param name="interval">秒間</param>
        /// <param name="callback">タイマー経過時に</param>
        public TimerAction_Observable(float waitTime, System.Action callback)
        {
            _waitTime = waitTime;
            _callback = callback;
        }

        public void StartTimer()
        {
            _timer = Observable.Timer(TimeSpan.FromSeconds(_waitTime))
                .Subscribe(_ => _callback?.Invoke());
        }

        public void Reset()
        {
            Stop();
            StartTimer();
        }

        public void Tick()
        {
        }

        public void Stop()
        {
            _timer?.Dispose(); // 既存のタイマーをリセット
        }

        public void Complete()
        {
            _callback?.Invoke();
            Stop();
        }
    }
}