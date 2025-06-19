using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UnityCustomExtension
{
    /// <summary>
    /// オブジェクトプール　大量に出す用
    /// </summary>
    public class ObjectPoolManager : DontDestroyOnLoadSingletonMonoBehaviour<ObjectPoolManager>
    {
        [SerializeField]
        private ObjectList _objectList;

        public enum PoolType
        {
            Stack,
            LinkedList
        }

        [SerializeField]
        private PoolType poolType;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        private bool collectionChecks = true;
        public int maxPoolSize = 20;

        //オブジェクトプールはオブジェクト単体毎に必要なのでDictionaryで管理
        private Dictionary<string, IObjectPool<GameObject>> _poolDictionary = new Dictionary<string, IObjectPool<GameObject>>();

        /// <summary>
        /// 指定キーのオブジェクトプールを返す
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IObjectPool<GameObject> GetObjectPool(string key)
        {
            IObjectPool<GameObject> pool = null;
            //プールが存在するならそれを返す
            if (_poolDictionary.ContainsKey(key))
            {
                pool = _poolDictionary[key];
            }
            //まだプールが無いなら作って渡す
            else
            {
                pool = new ObjectPool<GameObject>(() => { return CreatePooledItem(key); }, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                _poolDictionary.Add(key, pool);
            }
            return pool;
        }

        private GameObject CreatePooledItem(string key)
        {
            var go = Instantiate(_objectList.GetObject(key));
             
            // This is used to return ParticleSystems to the pool when they have stopped.
            var returnToPool = go.AddComponent<ReturnToPoolObject>();
            returnToPool._pool = GetObjectPool(key);


            return go;
        }

        // Called when an item is returned to the pool using Release
        private void OnReturnedToPool(GameObject system)
        {
            system.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool(GameObject system)
        {
            system.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        private void OnDestroyPoolObject(GameObject system)
        {
            Destroy(system.gameObject);
        }

        public GameObject CreateObject(string key, Vector3 position, Quaternion rotation)
        {
            var obj = CreatePooledItem(key);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        public T CreateObject<T>(string key, Vector3 position, Quaternion rotation)
        {
            var obj = CreatePooledItem(key);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            var target = obj.GetComponent<T>();
            return target;
        }
    }
}