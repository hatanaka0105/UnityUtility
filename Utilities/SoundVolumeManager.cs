using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// 音量コンフィグ系機能まとめ
    /// </summary>
    public class SoundVolumeManager : MonoBehaviour
    {
        [SerializeField]
        protected AudioListener listener = null;

        public void SoundActive(bool enable)
        {
            if(enable)
            {
                AudioListener.volume = 1f;
            }
            else
            {
                AudioListener.volume = 0f;
            }
        }
    }
}