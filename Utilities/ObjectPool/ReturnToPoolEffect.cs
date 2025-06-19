using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UnityCustomExtension
{
    /// <summary>
    /// 終了時にプールに帰るパーティクルシステム
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class ReturnToPoolEffect : MonoBehaviour
    {
        public ParticleSystem system;
        public IObjectPool<ParticleSystem> pool;

        void Awake()
        {
            system = GetComponent<ParticleSystem>();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        void OnParticleSystemStopped()
        {
            try
            {
                pool.Release(system);
            }
            catch (System.InvalidOperationException ex)
            {
                //Debug.LogError("EffectPool Log:" + gameObject.name + " has no pool");
            }
        }
    }
}