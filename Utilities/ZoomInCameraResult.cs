using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class ZoomInCameraResult : MonoBehaviour
{
    [SerializeField]
    private float _targetFOV;

    [SerializeField]
    private float _durationFOV;

    [SerializeField]
    private Transform _clearPosition;

    [SerializeField]
    private MMF_Player _feedBack;

    private Vector3 _initialPos;
    private float _initialFOV;

    private void Awake()
    {
        _initialPos = transform.position;
        _initialFOV = GetComponent<Camera>().fieldOfView;
    }

    public void Clear()
    {
        _feedBack?.PlayFeedbacks();

        var cam = GetComponent<Camera>();
        var currentFOV = cam.fieldOfView;
        DOVirtual.Float(currentFOV, _targetFOV, _durationFOV, (x) => { cam.fieldOfView = x; });
        transform.DOMove(_clearPosition.position, 2f).SetEase(Ease.OutCirc);
    }

    public void Retry()
    {
        transform.position = _initialPos;
        GetComponent<Camera>().fieldOfView = _initialFOV;
    }
}
