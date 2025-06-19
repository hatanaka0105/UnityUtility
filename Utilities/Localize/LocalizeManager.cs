using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCustomExtension;
using System;
using TMPro;

public class LocalizeManager : DontDestroyOnLoadSingletonMonoBehaviour<LocalizeManager>
{
    public enum Language
    {
        Japanese,
        English,
        ChineseSimplified,   // 中国語「簡体字」
        ChineseTraditional,  // 中国語「繁体字」
        French,              // フランス語
        Italian,             // イタリア語
        German,              // ドイツ語
        Spanish,             // スペイン語 - スペイ
        Korean,              // 韓国語
        Polish               // ポーランド語
    }
    private Language _lang;
    public Language Lang {
        get 
        {
            return _lang; 
        }
        set 
        {
            _lang = value;
            StartCoroutine(nameof(LoadAndApplyFont));
        } 
    }

    [SerializeField]
    private LocalizeAsset _localizeAsset;

    //NOTE:毎回Findすると重いのでDictionaryにキャッシュし直す
    private Dictionary<string, LocalizeData> _localizeDataCache;

    public void CacheLocalizeData()
    {
        if (_localizeDataCache == null || _localizeDataCache.Count == 0)
        {
            _localizeDataCache = new Dictionary<string, LocalizeData>();
            foreach (var d in _localizeAsset.Data)
            {
                _localizeDataCache.Add(d.key, d);
            }
        }
    }

    public string GetTextFromKey(string key)
    {
        CacheLocalizeData();
        if(_localizeDataCache == null || _localizeDataCache.Count == 0)
        {
#if UNITY_EDITOR
            Debug.LogError("Error:LocalizeManager キャッシュ作成に失敗");
            return "Error";
#endif
            return "";
        }
        if (_localizeDataCache.ContainsKey(key))
        {
            var data = _localizeDataCache[key];
            switch (_lang)
            {
                case Language.English:
                    return data.en;
                case Language.ChineseSimplified:
                    return data.ch_simpl;
                case Language.ChineseTraditional:
                    return data.ch_tradi;
                case Language.French:
                    return data.fr;
                case Language.Italian:
                    return data.ita;
                case Language.German:
                    return data.ger;
                case Language.Spanish:
                    return data.spa;
                case Language.Korean:
                    return data.kr;
                case Language.Polish:
                    return data.pol;
                case Language.Japanese:
                default:
                    return data.jp;
            }
        }
        //何らかの理由で辞書にない場合は元アセットから無理やり引っ張ってくる
        //パフォーマンスは悪い
        else
        {
#if UNITY_EDITOR
            Debug.LogError("Localize Warning:辞書からのデータの取得に失敗したので元データから取得");
#endif
            var data = _localizeAsset.Data.Find(x => x.key == key);
            switch (_lang)
            {
                case Language.English:
                    return data.en;
                case Language.ChineseSimplified:
                    return data.ch_simpl;
                case Language.ChineseTraditional:
                    return data.ch_tradi;
                case Language.French:
                    return data.fr;
                case Language.Italian:
                    return data.ita;
                case Language.German:
                    return data.ger;
                case Language.Spanish:
                    return data.spa;
                case Language.Korean:
                    return data.kr;
                case Language.Polish:
                    return data.pol;
                case Language.Japanese:
                default:
                    return data.jp;
            }
        }
    }

    private IEnumerator LoadAndApplyFont()
    {
        var fontTask = LocalizeFontManager_Addressable.Instance.GetLanguageFontAsync();

        // Taskの完了を待つ
        yield return new WaitUntil(() => fontTask.IsCompleted);

        var texts = GameObject.FindObjectsOfType<LocalizedTextComponent>();
        foreach (var tx in texts)
        {
            tx.SetLocalizeText();
        }
    }
}