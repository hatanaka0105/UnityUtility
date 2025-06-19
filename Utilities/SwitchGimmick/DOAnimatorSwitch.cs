using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Photon.Pun;
using UnityEditor;
using UnityCustomExtension;
using TMPro;
using Newtonsoft.Json.Bson;

#if UNITY_EDITOR
[DebugAttribute]
#endif
public class DOAnimatorSwitch : MonoBehaviour, ISwitchAnimator
{
    [SerializeField]
    private AnimType _animType;

    [SerializeField]
    private Vector3 _targetPos;

    [SerializeField]
    private Vector3 _targetRotate;

    [SerializeField]
    private Vector3 _targetScale;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private Ease _ease;

    [SerializeField]
    private bool _isLocal = false;

    [SerializeField]
    private bool _isAdditive = true;

    [SerializeField]
    private RotateMode _rotateMode = RotateMode.LocalAxisAdd;

    [SerializeField]
    private UnityEvent _onAnimationEnd;

    [SerializeField]
    private bool _isAutoActivate = false;

    [SerializeField]
    private bool _useCustomCurve = false;

    [SerializeField]
    private AnimationCurve _customEaseCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private enum AnimType
    {
        Position,
        Rotation,
        Scale,
    }

    private Vector3 _initPos;
    private Vector3 _initRot;
    private Vector3 _initLocalPos;
    private Vector3 _initLocalRot;
    private Vector3 _initScale;
    private Tween _tween;

    private Action _switchOnEvent;
    private Action _switchOffEvent;
    private Action _onStartOnEvent;

    private PhotonView _view;

    void Awake()
    {
        _initPos = transform.position;
        _initRot = NormalizeEulerAngles(transform.rotation.eulerAngles);
        _initLocalPos = transform.localPosition;
        _initLocalRot = NormalizeEulerAngles(transform.localRotation.eulerAngles);
        _initScale = transform.localScale;
        _view = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (_isAutoActivate)
        {
            OnAnimation();
        }
    }

    private void OnDisable()
    {
        transform.position = _initPos;
        transform.rotation = Quaternion.Euler(_initRot);
        transform.localPosition = _initLocalPos;
        transform.localRotation = Quaternion.Euler(_initLocalRot);
        transform.localScale = _initScale;
        // Tween破棄
        if (DOTween.instance != null)
        {
            _tween?.Kill();
        }
    }

    private Tween ApplyEase(Tween tween)
    {
        if (_useCustomCurve)
            return tween.SetEase(_customEaseCurve);
        else
            return tween;
    }

    private Vector3 NormalizeEulerAngles(Vector3 euler)
    {
        return new Vector3(Mathf.Repeat(euler.x, 360f), Mathf.Repeat(euler.y, 360f), Mathf.Repeat(euler.z, 360f));
    }

    public void InitializeCallback(Action switchOnEvent, Action switchOffEvent)
    {
        _switchOnEvent += switchOnEvent;
        _switchOffEvent += switchOffEvent;
    }

    public void OnAnimation()
    {
        if (_view == null || PhotonNetwork.OfflineMode)
        {
            _onStartOnEvent?.Invoke();

            if (_isAdditive)
            {
                if (_isLocal)
                {
                    if (_animType == AnimType.Position)
                    {
                        _tween = ApplyEase(transform.DOLocalMove(_targetPos + _initLocalPos, _duration))
                            .OnComplete(OnCompleteOnAnimation);
                    }
                    if (_animType == AnimType.Rotation)
                    {
                        _tween = ApplyEase(transform.DOLocalRotate(_targetRotate + _initLocalRot, _duration, _rotateMode))
                            .OnComplete(OnCompleteOnAnimation);
                    }
                }
                else
                {
                    if (_animType == AnimType.Position)
                    {
                        _tween = ApplyEase(transform.DOMove(_targetPos + _initPos, _duration))
                            .OnComplete(OnCompleteOnAnimation);
                    }
                    if (_animType == AnimType.Rotation)
                    {
                        _tween = ApplyEase(transform.DORotate(_targetRotate + _initRot, _duration, _rotateMode))
                            .OnComplete(OnCompleteOnAnimation);
                    }
                }
                if (_animType == AnimType.Scale)
                {
                    _tween = ApplyEase(transform.DOScale(_targetScale + _initScale, _duration))
                        .OnComplete(OnCompleteOnAnimation);
                }
            }
            else
            {
                if (_isLocal)
                {
                    if (_animType == AnimType.Position)
                    {
                        _tween = ApplyEase(transform.DOLocalMove(_targetPos, _duration))
                            .OnComplete(OnCompleteOnAnimation);
                    }
                    if (_animType == AnimType.Rotation)
                    {
                        _tween = ApplyEase(transform.DOLocalRotateQuaternion(Quaternion.Euler(_targetRotate), _duration))
                            .OnComplete(OnCompleteOnAnimation);
                        //_tween = ApplyEase(transform.DOLocalRotate(_targetRotate, _duration, _rotateMode)
                    }
                }
                else
                {
                    if (_animType == AnimType.Position)
                    {
                        _tween = ApplyEase(transform.DOMove(_targetPos, _duration))
                            .OnComplete(OnCompleteOnAnimation);
                    }
                    if (_animType == AnimType.Rotation)
                    {
                        _tween = ApplyEase(transform.DORotate(_targetRotate, _duration, _rotateMode))
                            .OnComplete(OnCompleteOnAnimation);
                    }
                }
                if (_animType == AnimType.Scale)
                {
                    _tween = ApplyEase(transform.DOScale(_targetScale, _duration))
                        .OnComplete(OnCompleteOnAnimation);
                }
            }
        }
        else
        {
            _view.RPC(nameof(DoAnimRPC), RpcTarget.All);
        }
    }

    [PunRPC]
    private void DoAnimRPC()
    {
        _onStartOnEvent?.Invoke();

        if (_isAdditive)
        {
            if (_isLocal)
            {
                if (_animType == AnimType.Position)
                {
                    _tween = ApplyEase(transform.DOLocalMove(_targetPos + _initLocalPos, _duration))
                        .OnComplete(OnCompleteOnAnimation);
                }
                if (_animType == AnimType.Rotation)
                {
                    _tween = ApplyEase(transform.DOLocalRotate(_targetRotate + _initLocalRot, _duration, _rotateMode))
                        .OnComplete(OnCompleteOnAnimation);
                }
            }
            else
            {
                if (_animType == AnimType.Position)
                {
                    _tween = ApplyEase(transform.DOMove(_targetPos + _initPos, _duration))
                        .OnComplete(OnCompleteOnAnimation);
                }
                if (_animType == AnimType.Rotation)
                {
                    _tween = ApplyEase(transform.DORotate(_targetRotate + _initRot, _duration, _rotateMode))
                        .OnComplete(OnCompleteOnAnimation);
                }
            }
            if (_animType == AnimType.Scale)
            {
                _tween = ApplyEase(transform.DOScale(_targetScale + _initScale, _duration))
                    .OnComplete(OnCompleteOnAnimation);
            }
        }
        else
        {
            if (_isLocal)
            {
                if (_animType == AnimType.Position)
                {
                    _tween = ApplyEase(transform.DOLocalMove(_targetPos, _duration))
                        .OnComplete(OnCompleteOnAnimation);
                }
                if (_animType == AnimType.Rotation)
                {
                    _tween = ApplyEase(transform.DOLocalRotateQuaternion(Quaternion.Euler(_targetRotate), _duration))
                        .OnComplete(OnCompleteOnAnimation);
                    //_tween = ApplyEase(transform.DOLocalRotate(_targetRotate, _duration, _rotateMode))
                }
            }
            else
            {
                if (_animType == AnimType.Position)
                {
                    _tween = ApplyEase(transform.DOMove(_targetPos, _duration))
                        .OnComplete(OnCompleteOnAnimation);
                }
                if (_animType == AnimType.Rotation)
                {
                    _tween = ApplyEase(transform.DORotate(_targetRotate, _duration, _rotateMode))
                        .OnComplete(OnCompleteOnAnimation);
                }
            }
            if (_animType == AnimType.Scale)
            {
                _tween = ApplyEase(transform.DOScale(_targetScale, _duration))
                    .OnComplete(OnCompleteOnAnimation);
            }
        }
    }

    public void DelayedOnAnimation(float delay)
    {
        DOVirtual.DelayedCall(delay, OnAnimation);
    }

    public void OffAnimation()
    {
        if (_isLocal)
        {
            if (_animType == AnimType.Position)
            {
                _tween = ApplyEase(transform.DOLocalMove(_initLocalPos, _duration))
                    .OnComplete(OnCompleteOnAnimation);
            }
            if (_animType == AnimType.Rotation)
            {
                _tween = ApplyEase(transform.DOLocalRotateQuaternion(Quaternion.Euler(_initLocalRot), _duration))
                    .OnComplete(OnCompleteOffAnimation);
            }
        }
        else
        {
            if (_animType == AnimType.Position)
            {
                _tween = ApplyEase(transform.DOMove(_initPos, _duration))
                    .OnComplete(OnCompleteOnAnimation);
            }
            if (_animType == AnimType.Rotation)
            {
                _tween = ApplyEase(transform.DORotate(_initRot, _duration, _rotateMode))
                    .OnComplete(OnCompleteOffAnimation);
            }
        }
        if (_animType == AnimType.Scale)
        {
            _tween = ApplyEase(transform.DOScale(_initScale, _duration))
                .OnComplete(OnCompleteOffAnimation);
        }
    }

    private void OnCompleteOnAnimation()
    {
        _onAnimationEnd?.Invoke();
        _switchOnEvent?.Invoke();
        _tween = null;

        if (_animType == AnimType.Rotation)
        {
            if (_isLocal)
            {
                transform.localRotation = Quaternion.Euler(NormalizeEulerAngles(transform.localRotation.eulerAngles));
            }
            else
            {
                transform.rotation = Quaternion.Euler(NormalizeEulerAngles(transform.rotation.eulerAngles));
            }
        }
    }

    private void OnCompleteOffAnimation()
    {
        _switchOffEvent?.Invoke();
        _tween = null;
    }

    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    public void SetOnCompleteOnAnimation(Action onStartOnEvent, Action switchOnEvent)
    {
        _onStartOnEvent += onStartOnEvent;
        _switchOnEvent += switchOnEvent;
    }

#if UNITY_EDITOR
    private MeshFilter[] _meshCache;
    private void OnDrawGizmos()
    {
        if (_meshCache == null)
        {
            _meshCache = transform.GetComponentsInChildren<MeshFilter>();
        }
        if (_targetPos != Vector3.zero)
        {
            transform.DrawSceneViewMeshPreview(_meshCache, _targetPos, Color.white);
        }
        else if (_targetRotate != Vector3.zero)
        {
            transform.DrawSceneViewMeshPreview(Quaternion.Euler(_targetRotate));
        }

    }
#endif
}
