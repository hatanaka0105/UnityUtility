using System;
using UnityEngine;
using DG.Tweening;

public class DOClockAnimator : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    private Ease _easeType;

    [SerializeField]
    private float _limitAngleEuler;

    public float LimitAngleEuler { get { return _limitAngleEuler; } }

    private Vector3 _initRot;
    private Tween _tween;

    void Awake()
    {
        _initRot = transform.rotation.eulerAngles;
    }

    public void Rotate(float x = 0, float y = 0, float z = 0)
    {
        Vector3 targetRotate = new Vector3(x, y, z);

        _tween = transform.DORotate(_initRot + targetRotate, _duration)
            .OnComplete(OnCompleteOnAnimation)
            .SetEase(_easeType);
    }

    public void ReturnAnimation()
    {
        _tween = transform.DORotate(_initRot, _duration)
            .OnComplete(OnCompleteOffAnimation)
            .SetEase(_easeType);
    }

    public void InstantReturn()
    {
        transform.rotation = Quaternion.Euler(_initRot);
    }

    private void OnCompleteOnAnimation()
    {
        _tween = null;
    }

    private void OnCompleteOffAnimation()
    {
        _tween = null;
    }
}
