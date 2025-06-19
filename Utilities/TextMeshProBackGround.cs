using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextMeshProBackGround : MonoBehaviour
{
    [SerializeField]
    private float _paddingTop;

    [SerializeField]
    private float _paddingBottom;

    [SerializeField]
    private float _paddingLeft;

    [SerializeField]
    private float _paddingRight;

    [SerializeField]
    private RectTransform _background;

    [SerializeField]
    private TMP_Text _text;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var bounds = _text.bounds;

        var xpos = GetComponent<RectTransform>().rect.width / 2;

        // 描画位置の計算
        //var xOffset = -(_paddingLeft / 2) + (_paddingRight / 2);
        //var yOffset = -(_paddingBottom / 2) + (_paddingTop / 2);
        ////_text.rectTransform.anchoredPosition = new Vector2(_text.preferredWidth / 2, 0);
        //var pos = _text.rectTransform.anchoredPosition + new Vector2(xpos, bounds.center.y);
        //_background.anchoredPosition = new Vector2(_text.preferredWidth + xOffset, pos.y + yOffset);

        // 描画サイズの計算
        var wOffset = _paddingLeft + _paddingRight;
        var hOffset = _paddingTop + _paddingBottom;
        _background.sizeDelta = new Vector2(_text.preferredWidth + wOffset, _text.preferredHeight + hOffset);
    }
}