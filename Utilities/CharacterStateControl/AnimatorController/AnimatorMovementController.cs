using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMovementController : MonoBehaviour, IActionState
{
    [SerializeField, Header("歩く↔走るモーションがどの速度のときに切り替わるか")]
    private float _walkToRunThreshold = 0.0f;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _animWalkBoolParamName;

    [SerializeField]
    private string _animRunBoolParamName;

    private IMoveSpeed _speedGetter;

    private bool _canChangeAnotherState;

    // Start is called before the first frame update
    void Start()
    {
        _speedGetter = gameObject.GetComponent<IMoveSpeed>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_speedGetter != null)
        {
            float speed = _speedGetter.GetSpeed();
            if (speed > _walkToRunThreshold)
            {
                _animator.SetBool(_animWalkBoolParamName, false);
                _animator.SetBool(_animRunBoolParamName, true);
                _canChangeAnotherState = false;
            }
            else if (speed > 0.0f)
            {
                _animator.SetBool(_animWalkBoolParamName, true);
                _animator.SetBool(_animRunBoolParamName, false);
                _canChangeAnotherState = false;
            }
            else
            {
                _animator.SetBool(_animWalkBoolParamName, false);
                _animator.SetBool(_animRunBoolParamName, false);
                _canChangeAnotherState = true;
            }
        }
    }

    public void Clear()
    {
        _animator.SetBool(_animWalkBoolParamName, false);
        _animator.SetBool(_animRunBoolParamName, false);
    }

    public void SetLock(bool islock)
    {
    }

    public bool IsLockAnotherState()
    {
        return !_canChangeAnotherState;
    }
}
