namespace UnityCustomExtension
{
    public interface ITickEvent
    {
        void Tick();
        void Reset();
        void StartTimer();
        void Stop();
        void Complete();
    }
}