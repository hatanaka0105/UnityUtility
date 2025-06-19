using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCustomExtension
{
    public class RetryPositionRotate : MonoBehaviour, IRetryObject
    {
        private Vector3 _initialPos;
        private Quaternion _initialRotate;

        private void Awake()
        {
            _initialPos = transform.position;
            _initialRotate = transform.rotation;
        }

        public void Retry()
        {
            transform.position = _initialPos;
            transform.rotation = _initialRotate;
        }
    }
}