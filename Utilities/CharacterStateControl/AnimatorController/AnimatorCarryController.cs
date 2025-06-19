using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCarryController : MonoBehaviour, IActionState
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _animConditionBoolParamName;

    private bool _canChangeAnotherState;

    private ICarryCondition _carryCondition;

    // Start is called before the first frame update
    void Start()
    {
        _carryCondition = gameObject.GetComponent<ICarryCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_carryCondition != null)
        {
            var isCarry = _carryCondition.IsCarrying();
            if (isCarry)
            {
                _animator.SetBool(_animConditionBoolParamName, true);
                _canChangeAnotherState = false;
            }
            else
            {
                _animator.SetBool(_animConditionBoolParamName, false);
                _canChangeAnotherState = true;
            }
        }
    }

    public void Clear()
    {
        _animator.SetBool(_animConditionBoolParamName, false);
    }

    public void SetLock(bool islock)
    {
    }

    public bool IsLockAnotherState()
    {
        return !_canChangeAnotherState;
    }

}
