using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayOnetimeAction : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _action;

    [SerializeField]
    private float _delay;

    private Tween _tween;

    private void OnEnable()
    {
        _tween = DOVirtual.DelayedCall(_delay, Action);
    }

    private void OnDisable()
    {
        // Tween破棄
        if (DOTween.instance != null)
        {
            _tween?.Kill();
        }
    }

    public void Action()
    {
        _action?.Invoke();
    }
}
