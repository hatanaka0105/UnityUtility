using UnityEngine;

namespace UnityCustomExtension
{
    public interface ICharacterMoveStrategy
    {
        void Move(Vector3 dir);
        Vector3 CurrentSpeed();
        Vector3 GetPosition();
        float GetPower();
        void Boost(float multiply);
        void ResetBoost();
        float GetBoostMultiplier();
    }
}