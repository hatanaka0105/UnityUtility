using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "LanguageFont", menuName = "Utility/LanguageFont")]
public class LanguageFont : ScriptableObject
{
    public TMP_FontAsset JpFont;
    public TMP_FontAsset EnFont;
    public TMP_FontAsset ChSimplifiedFont;   // 中国語（簡体字）
    public TMP_FontAsset ChTraditionalFont;  // 中国語（繁体字）
    public TMP_FontAsset FrFont;             // フランス語
    public TMP_FontAsset ItFont;             // イタリア語
    public TMP_FontAsset DeFont;             // ドイツ語
    public TMP_FontAsset EsFont;             // スペイン語
    public TMP_FontAsset KrFont;             // 韓国語
    public TMP_FontAsset PlFont;             // ポーランド語
}
