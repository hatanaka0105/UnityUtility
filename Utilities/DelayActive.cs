using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DelayActive : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private float _delayTime;

    [SerializeField, Header("このオブジェクトがアクティブになったとき開始するか")]
    private bool _isStartOnEnable;

    private Tween _tween;

    private void OnEnable()
    {
        if (_isStartOnEnable)
        {
            StartDelayActive();
        }
    }

    private void OnDisable()
    {
        _target.SetActive(false);
        _tween.Kill();
        _tween = null;
    }

    public void StartDelayActive()
    {
        _tween = DOVirtual.DelayedCall(_delayTime, Active);
    }

    private void Active()
    {
        _target.SetActive(true);
    }
}
