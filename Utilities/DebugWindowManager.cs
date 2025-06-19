using UnityEngine;
using UnityEngine.UI;

public class DebugWindowManager : MonoBehaviour
{
    [SerializeField]
    private Text _debugInput;

    public void SetDebugText(string text)
    {
        _debugInput.text = text;
    }

    public void SetDebugText(string text, Color color, int fontSize)
    {
        _debugInput.text = text;
        _debugInput.color = color;
        _debugInput.fontSize = fontSize;
    }

    public void AddDebugText(string text)
    {
        _debugInput.text += text;
    }

    public void AddDebugText(string text, Color color, int fontSize)
    {
        _debugInput.text += text;
        _debugInput.color = color;
        _debugInput.fontSize = fontSize;
    }
}