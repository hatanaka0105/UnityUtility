using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UnityCustomExtension
{
    public class LocalizeFont : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;

        void Start()
        {
            TMP_FontAsset font = LocalizeFontManager_Addressable.Instance.GetLanguageFont();
            _text.font = font;
        }
    }
}
