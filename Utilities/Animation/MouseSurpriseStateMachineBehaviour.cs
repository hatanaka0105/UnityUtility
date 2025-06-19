using UnityEngine;

public class MouseSurpriseStateMachineBehaviour : StateMachineBehaviour
{
    public System.Action OnStartSurprise;

    public System.Action OnEndSurprise;

    //状態が変わった時に実行
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStartSurprise?.Invoke();
    }

    //状態が終わる時(変わる直前)に実行
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnEndSurprise?.Invoke();
    }
}
