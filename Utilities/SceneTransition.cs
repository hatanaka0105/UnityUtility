using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCustomExtension.Scene
{
    public static class SceneTransition
    {
        private static AsyncOperation _asyncLoad = null;

        public static void SceneChange(string sceneName)
        {
            SceneManager.LoadScene(sceneName);

            Resources.UnloadUnusedAssets();
        }

        public static void SceneChangeDependencePlatform(string defaultSceneName)
        {
#if UNITY_ANDROID
            SceneManager.LoadScene(_androidScene);
#elif UNITY_IOS
            SceneManager.LoadScene(_iosScene);
#else
            SceneManager.LoadScene(defaultSceneName);
#endif
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// IDFAによるトラッキング許可状況次第で飛ばす先のシーンを変える
        /// </summary>
        /// <param name="defaultSceneName"></param>
        public static void SceneChangeByATT(string defaultSceneName)
        {
#if UNITY_ANDROID
        SceneManager.LoadScene(_androidScene);
#elif UNITY_IOS
        int status = AttObjC.GetTrackingAuthorizationStatus();
        if (status == 0)
        {
            SceneManager.LoadScene(_iosScene);
        }
        else
        {
            SceneManager.LoadScene(defaultSceneName);
        }
#else
            SceneManager.LoadScene(defaultSceneName);
#endif
        }

        /// <summary>
        /// 非同期ロード
        /// 遷移を任意のタイミングで行う場合
        /// </summary>
        private static void StartLoadSceneAsync()
        {
            // 次のシーンの非同期ロード開始
#if UNITY_ANDROID
        _asyncLoad = SceneManager.LoadSceneAsync(_androidScene);
#elif UNITY_IOS
        _asyncLoad = SceneManager.LoadSceneAsync(_iosScene);
#else
            //_asyncLoad = SceneManager.LoadSceneAsync(defaultSceneName);
#endif
            _asyncLoad.allowSceneActivation = false;         // シーン遷移無効化
        }

        /// <summary>
        /// IDFAによるトラッキング許可状況次第で飛ばす先のシーンを変える
        /// トラッキングシーンは大抵軽いので通常通り読み込む
        /// </summary>
        public static void StartLoadSceneAsyncByATT(string defaultSceneName)
        {
#if UNITY_ANDROID
        _asyncLoad = SceneManager.LoadSceneAsync(_androidScene);
#elif UNITY_IOS
        int status = AttObjC.GetTrackingAuthorizationStatus();
        if (status == 0)
        {
            _asyncLoad = SceneManager.LoadSceneAsync(_iosScene);
        }
        else
        {
            _asyncLoad = SceneManager.LoadSceneAsync(defaultSceneName);
        }
#else
            _asyncLoad = SceneManager.LoadSceneAsync(defaultSceneName);
#endif
            if (_asyncLoad != null)
            {
                _asyncLoad.allowSceneActivation = false; //シーン遷移無効化
            }
        }

        /// <summary>
        /// シーンプリロード
        /// </summary>
        public static void StartLoadSceneAsync(string defaultSceneName)
        {
            _asyncLoad = SceneManager.LoadSceneAsync(defaultSceneName);
            if (_asyncLoad != null)
            {
                _asyncLoad.allowSceneActivation = false; //シーン遷移無効化
            }
        }

        /// <summary>
        /// 非同期ロード
        /// ロード済みだったら即遷移、済んでいなかったら済み次第遷移
        /// </summary>
        public static void SceneChangeAsyncLoaded()
        {
            if (_asyncLoad != null)
            {
                _asyncLoad.allowSceneActivation = true;
            }

            //メモリ解放
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 非同期ロード
        /// </summary>
        /// <returns></returns>
        public static IEnumerator StartLoadSceneAsyncCoroutine(string sceneName)
        {
            // 次のシーンの非同期ロード開始
            _asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            _asyncLoad.allowSceneActivation = false; //シーン遷移無効化

            // ロードが完了するまで待つ
            yield return _asyncLoad;
        }
    }
}