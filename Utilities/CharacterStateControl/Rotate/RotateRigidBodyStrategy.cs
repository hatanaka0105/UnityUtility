using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// Rigidbodyの移動量から向きを算出する
    /// </summary>
    public class RotateRigidBodyStrategy : ICharacterRotateStrategy
    {
        private Transform _transform;
        private Rigidbody _rigidbody;
        private Vector3 _latestPos;

        public RotateRigidBodyStrategy(Transform transform, Rigidbody rigidbody)
        {
            _transform = transform;
            _rigidbody = rigidbody;
        }

        public void SetRotateStart(Vector3 target)
        {
        }

        public void UpdateRotate()
        {
            //前フレームとの位置の差から進行方向を割り出してその方向に回転します。
            Vector3 differenceDis = new Vector3(_transform.position.x, 0, _transform.position.z) - new Vector3(_latestPos.x, 0, _latestPos.z);
            _latestPos = _transform.position;
            if (Mathf.Abs(differenceDis.x) > 0.001f || Mathf.Abs(differenceDis.z) > 0.001f)
            {
                //_rigidbody.angularVelocity = new Vector3(0, differenceDis.magnitude * 1000f, 0);
                Quaternion rot = Quaternion.LookRotation(differenceDis);
                rot = Quaternion.Slerp(_rigidbody.transform.rotation, rot, 0.5f);
                _rigidbody.MoveRotation(rot);
            }
        }
    }
}