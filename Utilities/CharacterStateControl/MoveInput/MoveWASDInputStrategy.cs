using UnityEngine;

namespace UnityCustomExtension
{
    public class MoveWASDInputStrategy : ICharacterMoveInputStrategy
    {
        Vector3 _currentSpeed;

        public MoveWASDInputStrategy()
        {
        }

        public Vector3 InputMovement()
        {
            _currentSpeed = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                _currentSpeed += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                _currentSpeed += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                _currentSpeed += Vector3.back;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _currentSpeed += Vector3.right;
            }

            return _currentSpeed.normalized;
        }
    }
}