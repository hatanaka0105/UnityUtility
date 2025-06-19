using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "LanguageFont", menuName = "Utility/LanguageFont_Addressable")]
public class LanguageFont_Addressable : ScriptableObject
{
    public AssetReferenceT<TMP_FontAsset> JpFont;
    public AssetReferenceT<TMP_FontAsset> EnFont;
    public AssetReferenceT<TMP_FontAsset> ChSimplifiedFont;
    public AssetReferenceT<TMP_FontAsset> ChTraditionalFont;
    public AssetReferenceT<TMP_FontAsset> FrFont;
    public AssetReferenceT<TMP_FontAsset> ItFont;
    public AssetReferenceT<TMP_FontAsset> DeFont;
    public AssetReferenceT<TMP_FontAsset> EsFont;
    public AssetReferenceT<TMP_FontAsset> KrFont;
    public AssetReferenceT<TMP_FontAsset> PlFont;
}