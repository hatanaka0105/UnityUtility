using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCustomExtension.Audio;

namespace LittleCheeseWorks
{
    public class AnimationSleepCallback : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            if (_source == null)
            {
                gameObject.AddComponent<AudioSource>();
            }
        }

        public void OnBubbleBigger()
        {
            if (_source == null)
            {
                gameObject.AddComponent<AudioSource>();
            }
            AudioClipDictionary.Instance.Play("Hanatyoutin", _source);
        }
    }
}