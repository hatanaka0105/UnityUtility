using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCustomExtension.Audio
{
    [CreateAssetMenu(fileName = "AudioClipList", menuName = "Audio/AudioClipList")]
    public class AudioClipList : ScriptableObject
    {
        [SerializeField]
        private AudioClipAsset _asset;

        [Serializable]
        private class StringAudioClipKeyValuePair : SerializableKeyValuePair<string, AudioClip> { }

        [Serializable]
        private class AudioClipAsset : SerializableDictionary<string, AudioClip, StringAudioClipKeyValuePair> { }

        public AudioClip GetClip(string key)
        {
            return _asset[key];
        }

        // NOTE:デバッグ用のメソッド
#if UNITY_EDITOR
        public void DebugLogClipKeyInfo(string keyName)
        {
            foreach (var pair in _asset)
            {
                if (pair.Key == keyName)
                {
                    Debug.Log($"対応するキーのクリップ名はこちらです。:{pair.Value}");
                }
            }
        }
        /// <summary>
        /// 指定したキーの AudioClip の存在確認と内容をデバッグログに出力します。
        /// </summary>
        public void DebugLogClipNameInfo(string clipName)
        {
            int count = 0;
            foreach (var pair in _asset)
            {
                if (pair.Value.name == clipName)
                {
                    Debug.Log($"対応するキーはこちらです。:{pair.Key}");
                    count++;
                }
            }
            if (count > 1)
            {

                Debug.LogWarning($"同一クリップに対して{count}件のキーが割り当てられています。");

            }
            else if (count == 0)
            {
                Debug.LogWarning($"AudioClip {clipName} は見つかりませんでした。");
            }

        }

        /// <summary>
        /// _asset 内の AudioClip の重複をチェックし、重複しているものがあればデバッグログに出力します。
        /// </summary>
        public void DebugCheckDuplicateClips()
        {
            // AudioClip ごとに対応するキーをリストで保持する Dictionary を作成
            Dictionary<AudioClip, List<string>> clipToKeys = new Dictionary<AudioClip, List<string>>();

            // _asset を foreach で回して、各ペアのキーと AudioClip を取得
            foreach (var pair in _asset)
            {
                AudioClip clip = pair.Value;
                if (clip == null)
                    continue;

                // すでに登録されていればキーリストに追加、なければ新規作成
                if (!clipToKeys.ContainsKey(clip))
                {
                    clipToKeys[clip] = new List<string>();
                }
                clipToKeys[clip].Add(pair.Key);
            }

            bool duplicateFound = false;
            // 各 AudioClip に対して、割り当てられているキーが複数あるかチェック
            foreach (var kv in clipToKeys)
            {
                if (kv.Value.Count > 1)
                {
                    duplicateFound = true;
                    // キー一覧をカンマ区切りの文字列に変換して出力
                    
                    Debug.LogWarning($"重複: {kv.Key.name} に割り当てられているキー: {string.Join(", ", kv.Value)} (合計 {kv.Value.Count} 件)");
                }
            }
            if (!duplicateFound)
            {
                Debug.Log("重複する AudioClip はありません。");
            }
        }
#endif
    }
}
