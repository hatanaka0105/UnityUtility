using UnityEngine.InputSystem;

namespace UnityCustomExtension
{
    public class GamePadUIInput : IUIInput
    {
        public bool InputMenuOpen()
        {
            if (Gamepad.current != null)
            {
                if (Gamepad.current.selectButton.wasPressedThisFrame)
                {
                    return true;
                }
            }
            return false;
        }

        public bool InputMove()
        {
            if (Gamepad.current != null)
            {
                if (Gamepad.current.leftStick.ReadValue().magnitude > 0.5f)
                {
                    return true;
                }
            }
            return false;
        }
    }
}