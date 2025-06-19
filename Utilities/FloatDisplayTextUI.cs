using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class FloatDisplayTextUI : MonoBehaviour
{
    [Serializable]
    public class OnClickEvent : UnityEvent<TMP_Text, string> { }
    
    [SerializeField]
    OnClickEvent OnClick = null;

    private TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        OnClick.Invoke(_text, _text.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}