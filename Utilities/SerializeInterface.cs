using System;
using UnityEngine;

namespace UnityCustomExtension
{
    [Serializable]
    public class SerializeInterface<TInterface>
    {
        [SerializeField]
        private GameObject _gameobject;

        private TInterface _interface;

        public TInterface Interface
        {
            get
            {
                if (_gameobject == null)
                {
                    return default;
                }
                if (_interface == null)
                {
                    _interface = _gameobject.GetComponent<TInterface>();
                }
                return _interface;
            }
        }
    }
}