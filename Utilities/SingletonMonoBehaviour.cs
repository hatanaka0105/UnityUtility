using UnityEngine;


namespace LittleCheeseWorks
{
    /// <summary>
    /// NOTE:DontDestroyOnLoadSingletonMonoBehaviourとは違い、シーン遷移時に破棄されてほしいシングルトン
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
#if UNITY_EDITOR
                        //Debug.LogWarning(typeof(T) + " is not instantiated.");
#endif
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}