using UnityEngine;
using TMPro;

public class InputFieldLimit_TMPro : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _inputField;

    [SerializeField]
    private int _limitNum;

    public void CheckTextCount()
    {
        if (_inputField.text.Length > _limitNum)
        {
            _inputField.text = _inputField.text[.._limitNum];
        }
    }
}
