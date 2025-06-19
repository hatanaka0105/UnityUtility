using DG.Tweening;
using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// 発動してから設定した秒数経過するまでロックする汎用アクション
    /// </summary>
    public class IntervalAction
    {
        private bool _isLocked;
        private Tween _tween;

        public void InvokeWithInterval(System.Action action, float interval)
        {
            if (_isLocked)
            {
#if UNITY_EDITOR
                //Debug.Log("Lock:" + action.Method.Name);
#endif
                return;
            }
            action?.Invoke();
            _isLocked = true;

            _tween = DOVirtual.DelayedCall(interval, Unlock);
        }

        public void Lock(float interval)
        {
            if (_isLocked)
            {
                return;
            }
            _isLocked = true;

            _tween = DOVirtual.DelayedCall(interval, Unlock);
        }

        public void Unlock()
        {
            _isLocked = false;
        }

        public void OnDisable()
        {
            if (DOTween.instance != null)
            {
                _tween?.Kill();
            }
        }
    }
}