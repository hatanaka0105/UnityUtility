using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFallGroundStateMachineBehaviour : StateMachineBehaviour
{
    public System.Action OnStartFall;

    public System.Action OnEndFall;

    //状態が変わった時に実行
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStartFall?.Invoke();
    }

    //状態が終わる時(変わる直前)に実行
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnEndFall?.Invoke();
    }
}