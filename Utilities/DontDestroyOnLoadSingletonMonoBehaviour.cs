using UnityEngine;

public class DontDestroyOnLoadSingletonMonoBehaviour<T> : MonoBehaviour where T : DontDestroyOnLoadSingletonMonoBehaviour<T>
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            // 最も頻繁なケース：インスタンスが既に存在
            if (instance != null)
                return instance;

            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"{typeof(T)} is nothing");
#endif
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        CheckInstance();
        DontDestroyOnLoad(this);
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
#if UNITY_EDITOR
            Debug.Log("DontDestroyOnLoad Log:" + gameObject.name + "はnullなので入替");
#endif
            instance = (T)this;
            return true;
        }
        else if (Instance == this)
        {
#if UNITY_EDITOR
            Debug.Log("DontDestroyOnLoad Log:" + gameObject.name + "はnullなので入替");
#endif
            return true;
        }

        Destroy(this.gameObject);
        return false;
    }
}