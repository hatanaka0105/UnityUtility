using UnityCustomExtension;
using UnityEngine;

namespace UnityCustomExtension
{
    public class AccelerateMove : ICharacterMoveStrategy
    {
        private Accelerate _accele;
        private float _power;
        private float _minSpeed;
        private float _maxSpeed;
        private Vector3 _beforeDir;
        private Transform _transform;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="rigidbody">本体のRigidbody</param>
        /// <param name="power">移動量にかける係数</param>
        public AccelerateMove(Transform transform, float power, float maxSpeed, float minSpeed)
        {
            _accele = new Accelerate(transform);
            _power = power;
            _maxSpeed = maxSpeed;
            _minSpeed = minSpeed;
            _transform = transform;
        }

        public void Move(Vector3 dir)
        {
            if (dir.magnitude == 0f)
            {
                _accele.Reset();
                return;
            }

            if (_beforeDir == Vector3.zero)
            {
                _accele.InitializeVelocity(dir * _minSpeed);
                _beforeDir = dir;
                return;
            }

            _accele.InitializeVelocity(dir * _power);
            _accele.Update();
            _beforeDir = dir;

            var beforeVelocity = _accele.GetVelocity();

            if (_accele.GetVelocity().magnitude > _maxSpeed)
            {
                _accele.InitializeVelocity(beforeVelocity);
            }
        }

        public Vector3 CurrentSpeed()
        {
            if (_accele == null)
            {
                return Vector3.zero;
            }
            return _accele.GetVelocity();
        }

        public Vector3 GetPosition()
        {
            return _transform.position;
        }

        public float GetPower()
        {
            return _power;
        }

        public void Boost(float multiply)
        {

        }

        public void ResetBoost()
        {
        }

        public float GetBoostMultiplier()
        {
            return 0f;
        }
    }
}