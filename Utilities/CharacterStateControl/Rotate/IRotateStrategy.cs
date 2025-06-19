using UnityEngine;

namespace UnityCustomExtension
{
    public interface ICharacterRotateStrategy
    {
        void SetRotateStart(Vector3 target);
        void UpdateRotate();
    }
}