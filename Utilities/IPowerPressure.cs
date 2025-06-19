
namespace UnityCustomExtension
{
    public interface IPowerPressure
    {
        void Setup(float max = 100f, float min = 0, float waitDecrease = 1f, float decreaseRate = 1);
        void Tick();
        void SendPower(float power);
        void Decrease();
        void Reset();
        float GetPower();
    }
}