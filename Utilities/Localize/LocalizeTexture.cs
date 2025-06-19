using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCustomExtension
{
    public class LocalizeTexture : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Sprite _jpSprite;

        [SerializeField]
        private Sprite _enSprite;

        [SerializeField]
        private Sprite _krSprite;

        [SerializeField]
        private Sprite _ch_SimpleSprite;

        [SerializeField]
        private Sprite _ch_TradiSprite;

        // Start is called before the first frame update
        void Start()
        {
            //端末の設定言語を取得して分岐する
            switch (LocalizeManager.Instance.Lang)
            {
                case LocalizeManager.Language.English:
                    _image.sprite = _enSprite;
                    return;
                case LocalizeManager.Language.ChineseSimplified:
                    _image.sprite = _ch_SimpleSprite;
                    return;
                case LocalizeManager.Language.ChineseTraditional:
                    _image.sprite = _ch_TradiSprite;
                    return;
                case LocalizeManager.Language.Korean:
                    _image.sprite = _krSprite;
                    return;
                case LocalizeManager.Language.Japanese:
                    _image.sprite = _jpSprite;
                    return;
                default:
                    _image.sprite = _enSprite;
                    return;
            }
        }
    }
}