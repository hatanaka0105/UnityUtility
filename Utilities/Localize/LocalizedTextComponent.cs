using UnityEngine;
using TMPro;

namespace UnityCustomExtension
{
    public class LocalizedTextComponent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;

        [SerializeField, Header("Excelデータの参照key\n一致するkeyから自動で翻訳テキストを引っ張ってくる\n原則ここから入力する")]
        private string _key;

        [Header("Excelのkeyがない場合は↓の埋め込みデータを参照する\n!!!Excelへの移行が完了次第項目を消すのでここになるべくデータを入れないこと")]

        // --- 日本語 ---
        [Header("日本語")]
        [SerializeField, Multiline]
        private string _jpText;
        [SerializeField]
        private Color _jpColor = Color.black;
        [SerializeField]
        private Material _jpMat;

        // --- 英語 ---
        [Header("英語")]
        [SerializeField, Multiline]
        private string _enText;
        [SerializeField]
        private Color _enColor = Color.black;
        [SerializeField]
        private Material _enMat;

        // --- 中国語（簡体字） ---
        [Header("中国語（簡体字）")]
        [SerializeField, Multiline]
        private string _zhCnText;
        [SerializeField]
        private Color _zhCnColor = Color.black;
        [SerializeField]
        private Material _zhCnMat;

        // --- 中国語（繁体字） ---
        [Header("中国語（繁体字）")]
        [SerializeField, Multiline]
        private string _zhTwText;
        [SerializeField]
        private Color _zhTwColor = Color.black;
        [SerializeField]
        private Material _zhTwMat;

        // --- フランス語 ---
        [Header("フランス語")]
        [SerializeField, Multiline]
        private string _frText;
        [SerializeField]
        private Color _frColor = Color.black;
        [SerializeField]
        private Material _frMat;

        // --- イタリア語 ---
        [Header("イタリア語")]
        [SerializeField, Multiline]
        private string _itText;
        [SerializeField]
        private Color _itColor = Color.black;
        [SerializeField]
        private Material _itMat;

        // --- ドイツ語 ---
        [Header("ドイツ語")]
        [SerializeField, Multiline]
        private string _deText;
        [SerializeField]
        private Color _deColor = Color.black;
        [SerializeField]
        private Material _deMat;

        // --- スペイン語 ---
        [Header("スペイン語")]
        [SerializeField, Multiline]
        private string _esText;
        [SerializeField]
        private Color _esColor = Color.black;
        [SerializeField]
        private Material _esMat;

        // --- 韓国語 ---
        [Header("韓国語")]
        [SerializeField, Multiline]
        private string _krText;
        [SerializeField]
        private Color _krColor = Color.black;
        [SerializeField]
        private Material _krMat;

        // --- ポーランド語 ---
        [Header("ポーランド語")]
        [SerializeField, Multiline]
        private string _plText;
        [SerializeField]
        private Color _plColor = Color.black;
        [SerializeField]
        private Material _plMat;

        public void InsertLocalizeText(
            string jpText, string enText, string zhCnText = "",
            string zhTwText = "", string frText = "", string itText = "",
            string deText = "", string esText = "", string krText = "",
            string plText = ""
        )
        {
            _jpText  = jpText;
            _enText  = enText;
            _zhCnText = zhCnText;
            _zhTwText = zhTwText;
            _frText   = frText;
            _itText   = itText;
            _deText   = deText;
            _esText   = esText;
            _krText   = krText;
            _plText   = plText;
            SetLocalizeText();
        }
        
        private void OnEnable()
        {
            SetLocalizeText();
        }

        /// <summary>
        /// 設定されている LocalizeManager の言語に応じて
        /// テキスト・カラー・マテリアルを切り替える
        /// </summary>
        public void SetLocalizeText()
        {
            if (_text == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"[{nameof(LocalizedTextComponent)}] TMP_Textコンポーネントが設定されていません。");
#endif
                return;
            }

            if (LocalizeManager.Instance == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Error: LocalizeManagerがありません！");
#endif
                return;
            }

            TMP_FontAsset font = null;

            if (!string.IsNullOrEmpty(_key))
            {
                var text = LocalizeManager.Instance.GetTextFromKey(_key);

                // 現在の言語を取得して分岐
                switch (LocalizeManager.Instance.Lang)
                {
                    case LocalizeManager.Language.English:
                        ApplyLocalization(text, _enColor, _enMat);
                        break;

                    case LocalizeManager.Language.ChineseSimplified:
                        ApplyLocalization(text, _zhCnColor, _zhCnMat);
                        break;

                    case LocalizeManager.Language.ChineseTraditional:
                        ApplyLocalization(text, _zhTwColor, _zhTwMat);
                        break;

                    case LocalizeManager.Language.French:
                        ApplyLocalization(text, _frColor, _frMat);
                        break;

                    case LocalizeManager.Language.Italian:
                        ApplyLocalization(text, _itColor, _itMat);
                        break;

                    case LocalizeManager.Language.German:
                        ApplyLocalization(text, _deColor, _deMat);
                        break;

                    case LocalizeManager.Language.Spanish:
                        ApplyLocalization(text, _esColor, _esMat);
                        break;

                    case LocalizeManager.Language.Korean:
                        ApplyLocalization(text, _krColor, _krMat);
                        break;

                    case LocalizeManager.Language.Polish:
                        ApplyLocalization(text, _plColor, _plMat);
                        break;

                    case LocalizeManager.Language.Japanese:
                    default:
                        ApplyLocalization(text, _jpColor, _jpMat);
                        break;
                }
                return;
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log("Localize Log:" + gameObject.name + "にkeyが未設定のため埋め込みテキストを表示");
            }
#endif

            // 現在の言語を取得して分岐
            switch (LocalizeManager.Instance.Lang)
            {
                case LocalizeManager.Language.English:
                    ApplyLocalization(_enText, _enColor, _enMat);
                    break;

                case LocalizeManager.Language.ChineseSimplified:
                    ApplyLocalization(_zhCnText, _zhCnColor, _zhCnMat);
                    break;

                case LocalizeManager.Language.ChineseTraditional:
                    ApplyLocalization(_zhTwText, _zhTwColor, _zhTwMat);
                    break;

                case LocalizeManager.Language.French:
                    ApplyLocalization(_frText, _frColor, _frMat);
                    break;

                case LocalizeManager.Language.Italian:
                    ApplyLocalization(_itText, _itColor, _itMat);
                    break;

                case LocalizeManager.Language.German:
                    ApplyLocalization(_deText, _deColor, _deMat);
                    break;

                case LocalizeManager.Language.Spanish:
                    ApplyLocalization(_esText, _esColor, _esMat);
                    break;

                case LocalizeManager.Language.Korean:
                    ApplyLocalization(_krText, _krColor, _krMat);
                    break;

                case LocalizeManager.Language.Polish:
                    ApplyLocalization(_plText, _plColor, _plMat);
                    break;

                case LocalizeManager.Language.Japanese:
                default:
                    ApplyLocalization(_jpText, _jpColor, _jpMat);
                    break;
            }
        }

        /// <summary>
        /// テキスト、カラー、マテリアルを適用する。
        /// textValueがnullまたは空の場合はスキップする
        /// </summary>
        private void ApplyLocalization(string textValue, Color colorValue, Material matValue)
        {
            if (!string.IsNullOrEmpty(textValue)) _text.text = textValue;
            
            _text.color = colorValue;
            if (LocalizeFontManager_Addressable.Instance != null)
            {
                TMP_FontAsset font = LocalizeFontManager_Addressable.Instance.GetLanguageFont();
                if (font != null)
                    _text.font = font;
            }

            if (matValue != null)
                _text.fontMaterial = matValue;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_text == null)
            {
                _text = GetComponent<TMP_Text>();
            }
        }
#endif
    }
}
