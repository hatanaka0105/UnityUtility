using DG.Tweening;

namespace UnityCustomExtension
{
    /// <summary>
    /// 解除するまで、1回しか発動しない汎用アクション
    /// 解除しても指定の秒数待ってから発動可能になる
    /// </summary>
    public class OneTimeIntervalAction
    {
        private bool _isLocked;
        private float _interval;

        public OneTimeIntervalAction(float interval)
        {
            _interval = interval;
        }

        public void Invoke(System.Action action)
        {
            if (_isLocked)
            {
                return;
            }
            action?.Invoke();
            _isLocked = true;
        }

        /// <summary>
        /// 解除予約 設定した秒数待ったら再度発動可能になる
        /// </summary>
        /// <param name="forceUnlock">true</param>
        public void UnlockReservation(bool forceUnlock = false)
        {
            if (forceUnlock)
            {
                Unlock();
            }
            else
            {
                DOVirtual.DelayedCall(_interval, Unlock);
            }
        }

        private void Unlock()
        {
            _isLocked = false;
        }
    }
}