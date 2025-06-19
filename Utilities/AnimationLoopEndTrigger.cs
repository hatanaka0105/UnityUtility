using UnityEngine;

namespace UnityCustomExtension
{
    public class AnimationLoopEndTrigger : MonoBehaviour
    {
        [SerializeField]
        private Animator _anim;

        [SerializeField]
        private string _startLoopTriggerName;

        [SerializeField]
        private string _endTriggerName;

        public void LoopStart()
        {
            _anim.SetTrigger(_startLoopTriggerName);
        }

        public void LoopEnd()
        {
            _anim.SetTrigger(_endTriggerName);
        }
    }
}