using System;
using UnityEngine;

public class AnimatorSwitch : MonoBehaviour, ISwitchAnimator
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _onSwitchStateName;

    [SerializeField]
    private string _offSwitchStateName;

    private Action _switchOnEvent;
    private Action _switchOffEvent;

    public void InitializeCallback(Action switchOnEvent, Action switchOffEvent)
    {
        _switchOnEvent += switchOnEvent;
        _switchOffEvent += switchOffEvent;
    }

    public void OnAnimation()
    {
        _animator.Play(_onSwitchStateName);
    }

    public void OffAnimation()
    {
        _animator.Play(_offSwitchStateName);
    }

    private void OnCompleteOnAnimation()
    {
        _switchOnEvent?.Invoke();
    }

    private void OnCompleteOffAnimation()
    {
        _switchOffEvent?.Invoke();
    }
}
