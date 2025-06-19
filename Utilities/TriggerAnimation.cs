using UnityEngine;

public class TriggerAnimation : MonoBehaviour, ITriggerAnimation
{
    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private string _stateName;

    [SerializeField]
    private string _triggerName;

    public void Animate()
    {
        _anim.Play(_stateName);
    }

    public void SetTrigger()
    {
        if (gameObject == null)
        {
            return;
        }
        if(_anim == null)
        {
            _anim = GetComponent<Animator>();
        }
        _anim.SetTrigger(_triggerName);
    }
}