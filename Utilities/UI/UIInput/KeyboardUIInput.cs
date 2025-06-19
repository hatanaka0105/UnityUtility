using UnityEngine;

namespace UnityCustomExtension
{
    public class KeyboardUIInput : IUIInput
    {
        public bool InputMenuOpen()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return true;
            }
            return false;
        }

        public bool InputMove()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.RightArrow)
                || Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.W)
                || Input.GetKeyDown(KeyCode.S)
                || Input.GetKeyDown(KeyCode.A)
                || Input.GetKeyDown(KeyCode.D))
            {
                return true;
            }
            return false;
        }
    }
}