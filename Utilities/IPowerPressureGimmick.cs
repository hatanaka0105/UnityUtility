
namespace UnityCustomExtension
{
    public interface IPowerPressureGimmick
    {
        void Setup(float max = 100f, float min = 0, float waitDecrease = 1f, float decreaseRate = 1);
        void SendPower(float power);
        float GetPower();
    }
}