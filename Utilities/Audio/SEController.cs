using UnityEngine;

namespace UnityCustomExtension.Audio
{
    /// <summary>
    /// SEを鳴らすためのコンポーネント
    /// UIとかで主に使用
    /// </summary>
    public class SEController : MonoBehaviour
    {
        [SerializeField, Header("鳴らすクリップのキー")]
        private string key;

        public void Play()
        {
            AudioClipDictionary.Instance.PlayInstant(key, gameObject);
        }
    }
}
