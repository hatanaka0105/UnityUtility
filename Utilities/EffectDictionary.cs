using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UnityCustomExtension.Effect
{
    public class EffectDictionary : DontDestroyOnLoadSingletonMonoBehaviour<EffectDictionary>
    {
        [SerializeField]
        private EffectList _effectList;

        // エフェクトキーごとに個別のプールを管理
        private Dictionary<string, IObjectPool<ParticleSystem>> _effectPools = new Dictionary<string, IObjectPool<ParticleSystem>>();

        /// <summary>
        /// 指定されたキーのエフェクト用プールを取得（存在しない場合は作成）
        /// </summary>
        private IObjectPool<ParticleSystem> GetEffectPool(string key)
        {
            if (!_effectPools.ContainsKey(key))
            {
                _effectPools[key] = new ObjectPool<ParticleSystem>(
                    () => CreateNewEffectForPool(key),  // 新規作成
                    OnTakeFromPool,                     // プールから取得時
                    OnReturnedToPool,                   // プールに戻す時
                    OnDestroyPoolObject,                // 破棄時
                    true,                               // コレクションチェック
                    10,                                 // デフォルトキャパシティ
                    20                                  // 最大キャパシティ
                );
            }
            return _effectPools[key];
        }

        /// <summary>
        /// プール用の新しいエフェクトを作成（プライベート）
        /// </summary>
        private ParticleSystem CreateNewEffectForPool(string key)
        {
            var prefab = _effectList.GetEffect(key);
            if (prefab != null)
            {
                var go = Instantiate(prefab);
                var ps = go.GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                    // プールに戻すためのコンポーネント追加
                    var returnToPool = go.AddComponent<ReturnToPoolEffect>();
                    returnToPool.pool = GetEffectPool(key);

                    return ps;
                }
            }
            return null;
        }

        /// <summary>
        /// プールを使用してエフェクトを生成（修正版）
        /// </summary>
        public GameObject CreateEffect_Pool(string key, Transform parent = null, Vector3? position = null, Vector3? rotation = null)
        {
            var pool = GetEffectPool(key);
            var ps = pool.Get(); // プールから取得

            if (ps != null)
            {
                var go = ps.gameObject;

                // 位置・回転・親の設定
                if (parent != null)
                {
                    go.transform.SetParent(parent);
                    go.transform.localPosition = position ?? Vector3.zero;
                    go.transform.localRotation = Quaternion.Euler(rotation ?? Vector3.zero);
                }
                else
                {
                    go.transform.position = position ?? Vector3.zero;
                    go.transform.rotation = Quaternion.Euler(rotation ?? Vector3.zero);
                }

                // パーティクル再生
                ps.Play();

                return go;
            }

            return null;
        }

        /// <summary>
        /// プールを使用してエフェクトを生成（オーバーロード）
        /// </summary>
        public GameObject CreateEffect_Pool(string key, Transform parent, Vector3 pos, Vector3 rotation)
        {
            return CreateEffect_Pool(key, parent, (Vector3?)pos, (Vector3?)rotation);
        }

        /// <summary>
        /// プールを使用してエフェクトを生成（位置指定）
        /// </summary>
        public GameObject CreateEffect_Pool(string key, Vector3 position, Quaternion rotation)
        {
            return CreateEffect_Pool(key, null, position, rotation.eulerAngles);
        }

        // プールから取得時に呼ばれる
        private void OnTakeFromPool(ParticleSystem system)
        {
            if (system != null && system.gameObject != null)
            {
                system.gameObject.SetActive(true);
            }
        }

        // プールに戻す時に呼ばれる
        private void OnReturnedToPool(ParticleSystem system)
        {
            system.gameObject.SetActive(false);
            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // プール容量超過時の破棄処理
        private void OnDestroyPoolObject(ParticleSystem system)
        {
            Destroy(system.gameObject);
        }

        // --- 既存のCreateEffectメソッド群（プールを使わない通常版） ---

        /// <summary>
        /// エフェクトを取得する
        /// </summary>
        public GameObject GetEffect(string key)
        {
            return _effectList.GetEffect(key);
        }

        /// <summary>
        /// エフェクト生成（通常版）
        /// </summary>
        public GameObject CreateEffect(string key)
        {
            var prefab = _effectList.GetEffect(key);
            return Instantiate(prefab);
        }

        /// <summary>
        /// エフェクト生成（通常版）
        /// </summary>
        public GameObject CreateEffect(string key, Transform parent, Vector3 position, Quaternion rotation)
        {
            var prefab = _effectList.GetEffect(key);
            var obj = Instantiate(prefab, parent);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        /// <summary>
        /// エフェクト生成（通常版）
        /// </summary>
        public GameObject CreateEffect(string key, Transform parent)
        {
            var prefab = _effectList.GetEffect(key);
            var obj = Instantiate(prefab, parent);
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            return obj;
        }

        /// <summary>
        /// エフェクト生成（通常版）
        /// </summary>
        public GameObject CreateEffect(string key, Transform parent, Vector3 pos, Vector3 rotation)
        {
            var prefab = _effectList.GetEffect(key);
            var obj = Instantiate(prefab, parent);
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localPosition = pos;
            obj.transform.localRotation = Quaternion.Euler(rotation);
            return obj;
        }

        /// <summary>
        /// エフェクト生成（通常版）
        /// </summary>
        public GameObject CreateEffect(string key, Vector3 position, Quaternion rotation)
        {
            var prefab = _effectList.GetEffect(key);
            var obj = Instantiate(prefab);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }
    }
}