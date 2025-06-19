using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityCustomExtension.Audio;

public class TriggerFeedback : MonoBehaviour
{
    [SerializeField]
    private MMSquashAndStretch _squash;

    [SerializeField]
    private MMPositionShaker _shaker;

    public void Animate()
    {
        _shaker.Play();
        AudioClipDictionary.Instance.PlayInstant("VendingMachine", gameObject);
    }
}
