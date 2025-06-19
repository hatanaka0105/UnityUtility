#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace UnityCustomExtension.Audio
{
    [CustomEditor(typeof(AudioClipList))]
    public class AudioClipListEditor : Editor
    {
        // エディタ上で入力するクリップ名
        private string clipName = "";

        public override void OnInspectorGUI()
        {
            // 既存のインスペクター表示を描画
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("AudioClip存在確認", EditorStyles.boldLabel);
            // テキストフィールドでクリップ名を入力
            clipName = EditorGUILayout.TextField("キーorクリップ名", clipName);

            if (GUILayout.Button("キーのクリップ名確認"))
            {
                // 対象のAudioClipListオブジェクトを取得
                AudioClipList audioClipList = (AudioClipList)target;

                audioClipList.DebugLogClipKeyInfo(clipName);
            }

            if (GUILayout.Button("クリップの存在確認"))
            {
                // 対象のAudioClipListオブジェクトを取得
                AudioClipList audioClipList = (AudioClipList)target;

                audioClipList.DebugLogClipNameInfo(clipName);
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("重複チェック"))
            {
                // 対象のAudioClipListオブジェクトを取得
                AudioClipList audioClipList = (AudioClipList)target;
                audioClipList.DebugCheckDuplicateClips();
            }
        }
    }
}
#endif
