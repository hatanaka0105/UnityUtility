namespace UnityCustomExtension
{
    /// <summary>
    /// 解除するまで、一回しか発動しない汎用アクション
    /// </summary>
    public class OneTimeAction
    {
        private bool _isLocked;

        public void Invoke(System.Action action)
        {
            if (_isLocked)
            {
                return;
            }
            action?.Invoke();
            _isLocked = true;
        }

        public void Unlock()
        {
            _isLocked = true;
        }
    }
}