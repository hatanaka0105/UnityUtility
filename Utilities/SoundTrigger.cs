using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCustomExtension.Audio;

namespace UnityCustomExtension
{
    public class SoundTrigger : MonoBehaviour
    {
        [SerializeField, Header("鳴らすクリップのキー")]
        private string key;

        [SerializeField]
        private AudioSource _source;

        private void OnTriggerEnter(Collider other)
        {
            AudioClipDictionary.Instance.Play(key, _source);
        }
    }
}