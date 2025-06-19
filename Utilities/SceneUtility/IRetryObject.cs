namespace UnityCustomExtension
{
    /// <summary>
    /// リトライ時に元の状態に戻すオブジェクト
    /// 「元」の解釈は各オブジェクトに依存
    /// </summary>
    public interface IRetryObject
    {
        void Retry();
    }
}