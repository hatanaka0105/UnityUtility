using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCustomExtension.Effect
{
    [CreateAssetMenu(fileName = "EffectKeyList", menuName = "Effect/EffectKeyList")]
    public class EffectKeyList : ScriptableObject
    {
        [SerializeField]
        public List<string> _keyList;
    }
}