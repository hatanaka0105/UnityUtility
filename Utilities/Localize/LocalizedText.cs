using System;
using UnityEngine;

[Serializable]
public class LocalizedText
{
    public string Japanese;
    public string English;
    public string ChineseSimplified;   // 中国語「簡体字」
    public string ChineseTraditional;  // 中国語「繁体字」
    public string French;              // フランス語
    public string Italian;             // イタリア語
    public string German;              // ドイツ語
    public string Spanish;             // スペイン語（スペイ）
    public string Korean;              // 韓国語
    public string Polish;              // ポーランド語

    public string GetText()
    {
        switch (LocalizeManager.Instance.Lang)
        {
            case LocalizeManager.Language.English:
                return English;
            case LocalizeManager.Language.ChineseSimplified:
                return ChineseSimplified;
            case LocalizeManager.Language.ChineseTraditional:
                return ChineseTraditional;
            case LocalizeManager.Language.French:
                return French;
            case LocalizeManager.Language.Italian:
                return Italian;
            case LocalizeManager.Language.German:
                return German;
            case LocalizeManager.Language.Spanish:
                return Spanish;
            case LocalizeManager.Language.Korean:
                return Korean;
            case LocalizeManager.Language.Polish:
                return Polish;
            case LocalizeManager.Language.Japanese:
            default:
                return Japanese;
        }
    }
}