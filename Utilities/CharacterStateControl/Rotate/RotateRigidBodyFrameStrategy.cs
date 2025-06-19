using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// 入力から角度を変える RigidBodyの場合
    /// </summary>
    public class RotateRigidBodyFrameStrategy : ICharacterRotateStrategy
    {
        private Rigidbody _rigidbody;
        private float _time;

        private Quaternion _rot;

        public RotateRigidBodyFrameStrategy(Rigidbody rigidbody, float time)
        {
            _rigidbody = rigidbody;
            _time = time;
        }

        public void SetRotateStart(Vector3 target)
        {
            if (target.magnitude == 0.0f)
            {
                return;
            }
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            var angle = Quaternion.Angle(_rigidbody.rotation, Quaternion.LookRotation(target * _time));
            if(angle <= 0.1f)
            {
                return;
            }
            _rot = Quaternion.AngleAxis(angle, Vector3.up);

            _rigidbody.MoveRotation(_rot);
        }

        public void UpdateRotate()
        {
        }
    }
}