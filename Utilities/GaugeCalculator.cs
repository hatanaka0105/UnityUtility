namespace UnityCustomExtension.Gauge
{
    /// <summary>
    /// ゲージ用独自パラメータ規格
    /// </summary>
    public abstract class GaugeParam
    {

    }

    /// <summary>
    /// ゲージの増加量を決めるinterface
    /// </summary>
    public interface IGaugeAddCalc<T> where T : GaugeParam
    {
        int Calc(T param);
    }
}