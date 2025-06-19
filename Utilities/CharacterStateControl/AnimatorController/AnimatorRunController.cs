using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRunController : MonoBehaviour, IActionState
{
    [SerializeField]
    private Animator _animator;

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
            if (speed > 0.0f)
            {
                _animator.SetBool(_animRunBoolParamName, true);
                _canChangeAnotherState = false;
            }
            else
            {
                _animator.SetBool(_animRunBoolParamName, false);
                _canChangeAnotherState = true;
            }
        }
    }

    public void SetLock(bool islock)
    {
    }

    public bool IsLockAnotherState()
    {
        return !_canChangeAnotherState;
    }
}
