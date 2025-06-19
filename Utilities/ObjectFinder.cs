using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// シーン上の依存関係にないオブジェクトを取得したいときGameObject.Findするのはパフォーマンス上良くないので
    /// 最初から持っておいて渡す用のもの
    /// 追加したいオブジェクトはObjectCacheBaseクラスを定義しインスペクタ上で登録しておく
    /// 登録してないオブジェクトでも、tagやタイプが決まっていて該当するオブジェクトが一つなら初回だけのFindでキャッシュ
    /// </summary>
    public class ObjectFinder : DontDestroyOnLoadSingletonMonoBehaviour<ObjectFinder>
    {
        private Dictionary<string, GameObject> _runtimeCacheList = new Dictionary<string, GameObject>();

        /// <summary>
        /// オブジェクトキーで検索するが、
        /// 見つからない場合はFindしてキャッシュする
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public GameObject FindTargetAndCache(string tag = null)
        {
            GameObject targetObject = null;

            if (!string.IsNullOrEmpty(tag))
            {
                var tempTarget = GetObjectWithTag(tag);
                if (tempTarget != null)
                {
                    _runtimeCacheList.Add(tag, targetObject);
                    return tempTarget;
                }
            }

            return targetObject;
        }

        /// <summary>
        /// オブジェクトキーで検索
        /// 指定のTypeで返す
        /// 見つからない場合はtagで検索&キャッシュも可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public T GetObjectAsType<T>(string tag = null) where T : MonoBehaviour
        {
            GameObject targetObject = null;
            if (!string.IsNullOrEmpty(tag))
            {
                var tempTarget = GetObjectRuntimeCacheWithTag<T>(tag);
                if (tempTarget != null)
                {
                    _runtimeCacheList.Add(tag, targetObject);
                    return tempTarget;
                }
#if UNITY_EDITOR
                Debug.LogError(typeof(T) + "型のキャッシュが見つかりませんでした。設定を見直してください");
#endif
                return default(T);
            }

            //指定の型で取得
            var target = targetObject.GetComponent<T>();
            if (target == null)
            {
#if UNITY_EDITOR
                Debug.LogError(typeof(T) + "型のキャッシュは見つかりましたが、指定した型ではありません。設定を見直してください");
#endif
                return default(T);
            }

            return target;
        }

        /// <summary>
        /// 指定tagで検索し、見つからなかった場合はFind＆キャッシュ
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public GameObject GetObjectWithTag(string tag, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                if (_runtimeCacheList.ContainsKey(tag))
                {
                    return _runtimeCacheList[tag];
                }
                var obj = GameObject.FindGameObjectWithTag(tag);
                _runtimeCacheList.Add(tag, obj);
                return obj;
            }
            else
            {
                if (_runtimeCacheList.ContainsKey(key))
                {
                    return _runtimeCacheList[key];
                }
#if UNITY_EDITOR
                Debug.Log("key:" + key + "のオブジェクトがキャッシュにないためtagとして検索します");
#endif
                var obj = GameObject.FindGameObjectWithTag(tag);
                _runtimeCacheList.Add(key, obj);
                return obj;
            }
        }

        /// <summary>
        /// 指定tagで検索し、見つからなかった場合はFind&キャッシュ
        /// 型指定版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetObjectWithTag<T>(string tag, string key = "") where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(key))
            {
                if (_runtimeCacheList.ContainsKey(tag))
                {
                    return _runtimeCacheList[tag].GetComponent<T>();
                }
                var obj = GameObject.FindGameObjectWithTag(tag);
                _runtimeCacheList.Add(tag, obj);
                return obj.GetComponent<T>();
            }
            else
            {
                if (_runtimeCacheList.ContainsKey(key))
                {
                    return _runtimeCacheList[key].GetComponent<T>();
                }
                var obj = GameObject.FindGameObjectWithTag(tag);
                _runtimeCacheList.Add(key, obj);
                return obj.GetComponent<T>();
            }
        }

        /// <summary>
        /// 指定keyで検索
        /// 外部から登録、キャッシュ等自分でkeyを作った場合はこちら
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <returns></returns>
        public T GetObjectRuntimeCacheWithTag<T>(string tag) where T : MonoBehaviour
        {
            GameObject target = null;
            if (_runtimeCacheList.ContainsKey(tag) && _runtimeCacheList.TryGetValue(tag, out target))
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log("ObjectFinder Log RuntimeCacheListから取得:" + tag 
                    + " object:" + _runtimeCacheList[tag].name);
#endif
                if (target != null)
                {
                    return _runtimeCacheList[tag].GetComponent<T>();
                }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                else
                {
                    Debug.Log("ObjectFinder Log RuntimeCacheListには存在するが型名が違います 型名:" + typeof(T) + " tag:" + tag);
                }
#endif
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("ObjectFinder Log RuntimeCacheListに未登録:" + tag);
#endif
            return null;
        }

        /// <summary>
        /// 外部からAwakeなどの任意タイミングで登録する場合
        /// 他オブジェクトからAwakeで参照するようなタイミングが無い場合は、ここから後付することでGameObject.FindGameObjectsWithTagを防げる
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        public void AddObjectRuntimeCache(GameObject target, string key, bool forceUpdate = false)
        {
            if(forceUpdate && _runtimeCacheList.ContainsKey(key))
            {
                _runtimeCacheList.Remove(key);
            }
            _runtimeCacheList.Add(key, target);
        }

        public void RemoveFromRuntimeCache(string key)
        {
            _runtimeCacheList.Remove(key);
        }
    }
}