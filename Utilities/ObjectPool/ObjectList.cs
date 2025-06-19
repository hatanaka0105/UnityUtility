using System;
using UnityEngine;

namespace UnityCustomExtension
{
    [CreateAssetMenu(fileName = "ObjectList", menuName = "Object/ObjectList")]
    public class ObjectList : ScriptableObject
    {
        [SerializeField]
        private ObjectAsset _asset;

        [Serializable]
        private class StringObjectKeyValuePair : SerializableKeyValuePair<string, GameObject> { }

        [Serializable]
        private class ObjectAsset : SerializableDictionary<string, GameObject, StringObjectKeyValuePair> { }

        public GameObject GetObject(string key)
        {
            return _asset[key];
        }
    }
}