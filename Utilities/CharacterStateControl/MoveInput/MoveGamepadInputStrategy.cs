using UnityEngine;
using UnityEngine.InputSystem;
using UnityCustomExtension;

namespace UnityCustomExtension
{
    public class MoveGamepadInputStrategy : ICharacterMoveInputStrategy
    {
        Vector3 _currentSpeed;

        public MoveGamepadInputStrategy()
        {
        }

        public Vector3 InputMovement()
        {
            _currentSpeed = Vector3.zero;
            if (Gamepad.current != null)
            {
                _currentSpeed = UUtility.ConvertToVector3XZ(Gamepad.current.leftStick.ReadValue());
            }
            //DeadZone設定
            if (_currentSpeed.normalized.magnitude > 0.125f)
            {
                return _currentSpeed.normalized;
            }
            return Vector3.zero;
        }
    }
}