using UnityEngine;
using TMPro;

public class LocalizeFontManager : DontDestroyOnLoadSingletonMonoBehaviour<LocalizeFontManager>
{
    [SerializeField]
    private LanguageFont _languageFont;

    // NOTE:テスト時のセットアップで使用
    public void SetAudlioList(LanguageFont languageFont)
    {
        _languageFont = languageFont;
    }

    public TMP_FontAsset GetLanguageFont()
    {
        if (_languageFont == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("LanguageFontが設定されていません。");
#endif
            return null;
        }

        // 現在の言語を取得して分岐する
        switch (LocalizeManager.Instance.Lang)
        {
            case LocalizeManager.Language.English:
                return _languageFont.EnFont;
            case LocalizeManager.Language.ChineseSimplified:
                return _languageFont.ChSimplifiedFont;
            case LocalizeManager.Language.ChineseTraditional:
                return _languageFont.ChTraditionalFont;
            case LocalizeManager.Language.French:
                return _languageFont.FrFont;
            case LocalizeManager.Language.Italian:
                return _languageFont.ItFont;
            case LocalizeManager.Language.German:
                return _languageFont.DeFont;
            case LocalizeManager.Language.Spanish:
                return _languageFont.EsFont;
            case LocalizeManager.Language.Korean:
                return _languageFont.KrFont;
            case LocalizeManager.Language.Polish:
                return _languageFont.PlFont;
            case LocalizeManager.Language.Japanese:
            default:
                return _languageFont.JpFont;
        }
    }
}
