using UnityEngine;
using DG.Tweening;

namespace UnityCustomExtension
{
    /// <summary>
    /// 入力から角度を変える RigidBodyの場合
    /// </summary>
    public class RotateRigidBodyFromInputStrategy : ICharacterRotateStrategy
    {
        private Rigidbody _rigidbody;
        private Tween _tween;
        private float _time;

        private Quaternion _rot;

        public RotateRigidBodyFromInputStrategy(Rigidbody rigidbody, float time)
        {
            _rigidbody = rigidbody;
            _time = time;
        }

        public void SetRotateStart(Vector3 target)
        {
            _rot = Quaternion.LookRotation(target);
            _tween = _rigidbody.DORotate(_rot.eulerAngles, _time);
        }

        public void UpdateRotate()
        {
        }

        public void Stop()
        {
            _tween.Kill();
            _tween = null;
        }
    }
}