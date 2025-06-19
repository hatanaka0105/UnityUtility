using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UnityCustomExtension
{
    public interface IPoolReturn
    {
        void ReturnToPool();
    }

    /// <summary>
    /// 終了時にプールに帰る
    /// </summary>
    public class ReturnToPoolObject : MonoBehaviour, IPoolReturn
    {
        public IObjectPool<GameObject> _pool;

        void Start()
        {
        }

        public void ReturnToPool()
        {
            _pool.Release(gameObject);
        }
    }
}