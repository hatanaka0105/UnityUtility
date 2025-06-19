using UnityEngine;

namespace UnityCustomExtension
{
    public class DirectlyRotateStrategy : ICharacterRotateStrategy
    {
        private Transform _trans;
        private Quaternion _currentRotate;

        public DirectlyRotateStrategy(Transform trans)
        {
            _trans = trans;
        }

        public void SetRotateStart(Vector3 target)
        {
            var rotate = Quaternion.LookRotation(target, Vector3.up);
            if (_currentRotate == rotate)
            {
                return;
            }
            _trans.rotation = rotate;
            _currentRotate = _trans.rotation;
        }

        public void UpdateRotate()
        {

        }
    }
}