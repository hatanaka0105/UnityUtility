using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizeFontManager_Addressable : DontDestroyOnLoadSingletonMonoBehaviour<LocalizeFontManager_Addressable>
{
    [SerializeField]
    private LanguageFont_Addressable _fontConfig;

    // キャッシュ用辞書
    private Dictionary<LocalizeManager.Language, TMP_FontAsset> _fontCache = new Dictionary<LocalizeManager.Language, TMP_FontAsset>();
    private Dictionary<LocalizeManager.Language, AsyncOperationHandle<TMP_FontAsset>> _loadHandles = new Dictionary<LocalizeManager.Language, AsyncOperationHandle<TMP_FontAsset>>();

    public void SetFontConfig(LanguageFont_Addressable fontConfig)
    {
        _fontConfig = fontConfig;
    }
    public async Task<TMP_FontAsset> GetLanguageFontAsync()
    {
        if (_fontConfig == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("LanguageFontAddressableConfigが設定されていません。");
#endif
            return null;
        }

        var currentLang = LocalizeManager.Instance.Lang;

        // キャッシュから取得を試行
        if (_fontCache.TryGetValue(currentLang, out TMP_FontAsset cachedFont))
        {
            return cachedFont;
        }

        // 既にロード中の場合は待機
        if (_loadHandles.TryGetValue(currentLang, out AsyncOperationHandle<TMP_FontAsset> existingHandle))
        {
            return await existingHandle.Task;
        }

        // 動的ロード
        AssetReference fontRef = GetFontReference(currentLang);
        if (fontRef == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"言語 {currentLang} のフォント参照がnullです。ScriptableObjectで設定してください。");
#endif
            return null;
        }

//        if (!fontRef.IsValid())
//        {
//#if UNITY_EDITOR
//            Debug.LogError($"=== フォント参照が無効 ===");
//            Debug.LogError($"言語: {currentLang}");
//            Debug.LogError($"AssetGUID: {fontRef.AssetGUID}");
//            Debug.LogError($"SubObjectName: {fontRef.SubObjectName}");
//            Debug.LogError($"RuntimeKey: {fontRef.RuntimeKey}");
//            Debug.LogError($"対応策:");
//            Debug.LogError($"1. Project ウィンドウでアセットを選択");
//            Debug.LogError($"2. Inspector で Addressable のチェックを外して再度チェック");
//            Debug.LogError($"3. Addressables Groups で該当アセットが存在するか確認");
//            Debug.LogError($"4. Build > Clean Build > All を実行");
//#endif
//            return null;
//        }

        // Addressableでロード
        var handle = fontRef.LoadAssetAsync<TMP_FontAsset>();
        _loadHandles[currentLang] = handle;

        TMP_FontAsset font = await handle.Task;

        if (font == null)
        {
#if UNITY_EDITOR
            Debug.LogError($"フォントの読み込みに失敗しました: {fontRef}");
#endif
            return null;
        }

        // キャッシュに保存
        _fontCache[currentLang] = font;
        return font;
    }

    // 同期版（既にロード済みの場合のみ）
    public TMP_FontAsset GetLanguageFont()
    {
        var currentLang = LocalizeManager.Instance.Lang;
        _fontCache.TryGetValue(currentLang, out TMP_FontAsset cachedFont);
        return cachedFont;
    }

    public async Task PreloadCurrentLanguageFont()
    {
        await GetLanguageFontAsync();
    }

    private AssetReference GetFontReference(LocalizeManager.Language language)
    {
        switch (language)
        {
            case LocalizeManager.Language.English:
                return _fontConfig.EnFont;
            case LocalizeManager.Language.ChineseSimplified:
                return _fontConfig.ChSimplifiedFont;
            case LocalizeManager.Language.ChineseTraditional:
                return _fontConfig.ChTraditionalFont;
            case LocalizeManager.Language.French:
                return _fontConfig.FrFont;
            case LocalizeManager.Language.Italian:
                return _fontConfig.ItFont;
            case LocalizeManager.Language.German:
                return _fontConfig.DeFont;
            case LocalizeManager.Language.Spanish:
                return _fontConfig.EsFont;
            case LocalizeManager.Language.Korean:
                return _fontConfig.KrFont;
            case LocalizeManager.Language.Polish:
                return _fontConfig.PlFont;
            case LocalizeManager.Language.Japanese:
            default:
                return _fontConfig.JpFont;
        }
    }

    // 言語変更時に不要なフォントをアンロード
    public async Task OnLanguageChanged(LocalizeManager.Language newLanguage)
    {
        var keysToRemove = new List<LocalizeManager.Language>();

        foreach (var kvp in _loadHandles)
        {
            if (kvp.Key != newLanguage)
            {
                keysToRemove.Add(kvp.Key);
                // Addressableアセットをリリース
                if (kvp.Value.IsValid())
                {
                    Addressables.Release(kvp.Value);
                }
            }
        }

        foreach (var key in keysToRemove)
        {
            _loadHandles.Remove(key);
            _fontCache.Remove(key);
        }

        // 新しい言語のフォントを事前ロード
        await GetLanguageFontAsync();
    }

    private void OnDestroy()
    {
        // すべてのハンドルをリリース
        foreach (var handle in _loadHandles.Values)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
        _loadHandles.Clear();
        _fontCache.Clear();
    }

    // NOTE:テスト時のセットアップで使用
    public void SetAudlioList(LanguageFont languageFont)
    {
        //_fontConfig = languageFont;
    }
}
