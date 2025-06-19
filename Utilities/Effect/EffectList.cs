using System;
using System.Linq;
using UnityEngine;

namespace UnityCustomExtension.Effect
{
    [CreateAssetMenu(fileName = "EffectList", menuName = "Effect/EffectList")]
    public class EffectList : ScriptableObject
    {
        [SerializeField]
        private EffectAsset _asset;

        [Serializable]
        private class StringEffectKeyValuePair : SerializableKeyValuePair<string, GameObject> { }

        [Serializable]
        private class EffectAsset : SerializableDictionary<string, GameObject, StringEffectKeyValuePair> { }

        public GameObject GetEffect(string key)
        {
            return _asset[key];
        }
    }
}