using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;

namespace UnityCustomExtension.Audio
{
    public class AudioClipDictionary : DontDestroyOnLoadSingletonMonoBehaviour<AudioClipDictionary>
    {
        [SerializeField]
        private AudioClipList _audioList;

        // NOTE:テスト時のセットアップで使用
        public void SetAudlioList(AudioClipList list)
        {
            _audioList = list;
        }

        /// <summary>
        /// Clipを取得する
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AudioClip GetClip(string key)
        {
            return _audioList.GetClip(key);
        }

        /// <summary>
        /// 与えられたソースでclipを再生する
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Play(string key, AudioSource source)
        {
            var clip = _audioList.GetClip(key);
            source.clip = clip;
#if UNITY_EDITOR
            if (clip != null && source != null && source.gameObject != null)
            {
                // Debug.Log("Audio:key:" + key + " clip:" + clip.name + "を" + source.gameObject.name + "から再生");
            }
#endif
            source.Play();
        }

        public void PlayOneShot(string key, AudioSource source)
        {
            var clip = _audioList.GetClip(key);
#if UNITY_EDITOR
            Debug.Log("Audio:key:" + key + " clip:" + clip.name + "を" + source.gameObject.name + "から再生");
#endif
            source.PlayOneShot(clip);
        }

        /// <summary>
        /// 指定されたオブジェクトにSourceを追加して再生
        /// AddComponentが重いので連続実行は禁止
        /// </summary>
        /// <param name="key"></param>
        /// <param name=""></param>
        public void PlayInstant(string key, GameObject target, AudioMixerGroup output = null)
        {
            var source = target.GetComponent<AudioSource>();
            if (source == null)
            {
                source = target.AddComponent<AudioSource>();
            }
            var clip = _audioList.GetClip(key);
            if (output != null)
            {
                source.outputAudioMixerGroup = output;
            }
#if UNITY_EDITOR
            Debug.Log("Audio:key:" + key + " clip:" + clip.name + "を" + target.name + "から再生");
#endif
            source.PlayOneShot(clip);
        }

        /// <summary>
        /// このオブジェクト自体にSourceを追加して再生
        /// 大量使用は禁止
        /// </summary>
        /// <param name="key"></param>
        /// <param name="target"></param>
        public void PlayInstant(string key)
        {
            var source = gameObject.AddComponent<AudioSource>();
            var clip = _audioList.GetClip(key);
#if UNITY_EDITOR
            Debug.Log("Audio:key:" + key + " clip:" + clip.name + "を" + gameObject.name + "から再生");
#endif
            source.PlayOneShot(clip);
        }
    }
}