using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UnityCustomExtension
{
    public static class RuntimeCacheCommandQueue
    {
        private struct CacheCommand
        {
            public GameObject Target;
            public string Key;
            public bool ForceUpdate;

            public CacheCommand(GameObject target, string key, bool forceUpdate)
            {
                Target = target;
                Key = key;
                ForceUpdate = forceUpdate;
            }
        }

        private static Queue<CacheCommand> _queue = new Queue<CacheCommand>();
        private static bool _started = false;
        private static bool _ready = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            if (!_started)
            {
                _started = true;
                WaitForObjectFinderReady().Forget();
            }
        }

        /// <summary>
        /// 外部から呼び出すAPI（即実行 or 保留）
        /// </summary>
        public static void Register(GameObject obj, string key, bool forceUpdate = false)
        {
            if (_ready)
            {
                ObjectFinder.Instance.AddObjectRuntimeCache(obj, key, forceUpdate);
            }
            else
            {
                _queue.Enqueue(new CacheCommand(obj, key, forceUpdate));
            }
        }

        /// <summary>
        /// ObjectFinder.Instance が使えるまで待って、キューを流す
        /// </summary>
        private static async UniTaskVoid WaitForObjectFinderReady()
        {
            float timeout = 5f;
            float timer = 0f;

            while (ObjectFinder.Instance == null && timer < timeout)
            {
                await UniTask.Yield();
                timer += Time.unscaledDeltaTime;
            }

            if (ObjectFinder.Instance != null)
            {
                _ready = true;
                Flush();
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("ObjectFinder.Instance が見つかりませんでした（RuntimeCacheCommandQueue）");
            }
#endif
        }

        private static void Flush()
        {
            while (_queue.Count > 0)
            {
                var cmd = _queue.Dequeue();
                ObjectFinder.Instance.AddObjectRuntimeCache(cmd.Target, cmd.Key, cmd.ForceUpdate);
            }
        }
    }
}