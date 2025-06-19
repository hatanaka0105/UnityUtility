using System;
using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// �ėp�N���X
    /// �w�肵�����Ԃɓ��B������R�[���o�b�N
    /// Tick��MonoBehaviour��ȂǂŕK���ĂԂ���
    /// 1�񔭓��������~����̂ŔC�ӂ̃^�C�~���O�ł������StartTimer()���Ă�
    /// </summary>
    public class TimerAction : ITickEvent
    {
        private float _timer;
        private float _waitTime;
        private Action _callback;
        private bool _isStarted;

        /// <summary>
        /// Timer������
        /// </summary>
        /// <param name="interval">�b��</param>
        /// <param name="callback">�^�C�}�[�o�ߎ���</param>
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