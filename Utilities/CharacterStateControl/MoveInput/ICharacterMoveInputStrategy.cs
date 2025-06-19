using UnityEngine;

namespace UnityCustomExtension
{
    public interface ICharacterMoveInputStrategy
    {
        Vector3 InputMovement();
    }
}