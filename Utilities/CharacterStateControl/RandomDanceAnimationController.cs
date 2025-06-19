using System.Collections.Generic;
using UnityEngine;

public class RandomDanceAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private List<AnimationClip> _animations;

    /// <summary>
    /// animationsに登録されている中からランダムに再生する
    /// </summary>
    public void RandomDancePlay()
    {
        int index = Random.Range(0, _animations.Count);
        _animator.Play(_animations[index].name, 0);
    }
}
