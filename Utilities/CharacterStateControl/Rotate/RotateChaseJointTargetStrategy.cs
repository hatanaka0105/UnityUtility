using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCustomExtension
{
    public class RotateChaseJointTargetStrategy : ICharacterRotateStrategy
    {
        private Transform _transform;
        private HingeJoint _joint;
        private Rigidbody _rigidbody;
        private Vector3 _latestPos;

        public RotateChaseJointTargetStrategy(Transform transform, Rigidbody rigidbody, HingeJoint joint)
        {
            _transform = transform;
            _joint = joint;
            _rigidbody = rigidbody;
        }

        public void SetRotateStart(Vector3 target)
        {
        }

        public void UpdateRotate()
        {
            if (_joint.connectedAnchor == null)
            {
                return;
            }
            //前フレームとの位置の差から進行方向を割り出してその方向に回転します。
            Vector3 differenceDis = _joint.connectedAnchor - _transform.position;
            _latestPos = _transform.position;
            if (Mathf.Abs(differenceDis.x) > 0.001f || Mathf.Abs(differenceDis.z) > 0.001f)
            {
                //_rigidbody.angularVelocity = new Vector3(0, differenceDis.magnitude * 1000f, 0);
                Quaternion rot = Quaternion.LookRotation(differenceDis);
                rot = Quaternion.Slerp(_rigidbody.transform.rotation, rot, 0.2f);
                _rigidbody.MoveRotation(rot);
            }
        }
    }
}