using UnityEngine;

namespace UnityCustomExtension
{
    public class ConstantVelocityMove : ICharacterMoveStrategy
    {
        private Transform _transform;
        private Vector3 _currentSpeed;
        public ConstantVelocityMove(Transform transform)
        {
            _transform = transform;
        }

        public void Move(Vector3 dir)
        {
            _currentSpeed = dir * Time.deltaTime;
            _transform.position += _currentSpeed;
        }
        public Vector3 CurrentSpeed()
        {
            return _currentSpeed;
        }

        public Vector3 GetPosition()
        {
            return _transform.position;
        }

        public float GetPower()
        {
            return _currentSpeed.magnitude;
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