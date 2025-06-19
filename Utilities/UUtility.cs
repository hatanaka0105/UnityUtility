using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.IO;
using Cysharp.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityCustomExtension
{
    public class PosRotation
    {
        public Vector3 Pos;
        public Quaternion Rot;
    }

    public class ReferenceWrapper<T>
    {
        public T Value;

        public ReferenceWrapper(T value)
        {
            Value = value;
        }
    }

    [Serializable]
    public enum MoveDirection
    {
        Right,
        Left,
    }

    [Serializable]
    public enum EightDirection
    {
        Right,
        Left,
        Forward,
        Backward,
        RightForward,
        RightBackward,
        LeftForward,
        LeftBackward,
    }

    [Serializable]
    public enum DirectionXYZ
    {
        Forward,
        Backward,
        Right,
        Left,
        Up,
        Down,
    }

    /// <summary>
    /// Unity系拡張メソッドを入れる場所
    /// </summary>
    public static class UUtility
    {
        public static float NormalizeAngle(float angle)
        {
            angle = angle % 360f;
            if (angle > 180f) angle -= 360f;
            return angle;
        }

        public static int FindClosestPointIndex(this List<Vector3> list, Vector3 target)
        {
            int closestIndex = 0;
            float minDistance = float.MaxValue;

            foreach (var point in list)
            {
                float distance = Vector3.Distance(point, target);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = list.IndexOf(point);
                }
            }

            return closestIndex;
        }

        public static void SetXZVelocityZero(this Rigidbody rigidbody)
        {
            var velocity = Vector3.zero;
            velocity.y = rigidbody.velocity.y;
            rigidbody.velocity = velocity;
        }

        public static Vector3 SafeDiv(this Vector3 a, Vector3 b)
        {
            return new Vector3(
                b.x != 0 ? a.x / b.x : 0,
                b.y != 0 ? a.y / b.y : 0,
                b.z != 0 ? a.z / b.z : 0
            );
        }

        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static T GetMaxTransformRight<T>(List<T> list) where T : MonoBehaviour
        {
            // リストが空の場合はnullを返す
            if (list.Count == 0)
            {
                return null;
            }

            // 最初の要素を最大と仮定
            var maxObject = list[0];
            float maxX = maxObject.transform.position.x;

            // リストの残りの要素と比較
            for (int i = 1; i < list.Count; i++)
            {
                float currentX = list[i].transform.position.x;

                // より大きい値が見つかれば更新
                if (currentX > maxX)
                {
                    maxX = currentX;
                    maxObject = list[i];
                }
            }

            return maxObject;
        }

        public static T GetMinTransformRight<T>(List<T> list) where T : MonoBehaviour
        {
            // リストが空の場合はnullを返す
            if (list.Count == 0)
            {
                return null;
            }

            // 最初の要素を最小と仮定
            var minObject = list[0];
            float minX = minObject.transform.position.x;

            // リストの残りの要素と比較
            for (int i = 1; i < list.Count; i++)
            {
                float currentX = list[i].transform.position.x;

                // より小さい値が見つかれば更新
                if (currentX < minX)
                {
                    minX = currentX;
                    minObject = list[i];
                }
            }

            return minObject;
        }

        public static List<T> GetObjectsInBounds<T>(List<T> objects, Bounds bounds) where T : MonoBehaviour
        {
            var list = new List<T>();
            foreach (var obj in objects)
            {
                if(obj == null)
                {
                    continue;
                }
                if (bounds.max.x > obj.transform.position.x && bounds.min.x < obj.transform.position.x)
                {
                    list.Add(obj);
                }
            }
            return list;
        }

        public static Gradient CopyGradient(this Gradient original)
        {
            Gradient copy = new Gradient();
            copy.SetKeys(original.colorKeys, original.alphaKeys);
            return copy;
        }

        public static void PlayEffect(this GameObject effect)
        {
            var particles = effect.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }

        public static void StopEffect(this GameObject effect)
        {
            var particles = effect.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particles)
            {
                particle.Stop();
            }
        }

        public static void Swap<T>(this List<T> list, int indexA, int indexB)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }

        private static bool WaitForFileToExist(string filePath, float timeoutSeconds)
        {
            float startTime = Time.realtimeSinceStartup;

            while (!File.Exists(filePath))
            {
                if (Time.realtimeSinceStartup - startTime > timeoutSeconds)
                {
                    return false;
                }
            }

            return true;
        }

        public static Color GetRainbowColor(int index)
        {
            switch (index)
            {
                case 0:
                    return Color.red; // 赤
                case 1:
                    return new Color(1f, 0.647f, 0f); // 橙
                case 2:
                    return Color.yellow; // 黄色
                case 3:
                    return new Color(0.5f, 1f, 0f); ; // 黄緑
                case 4:
                    return Color.green; // 緑
                case 5:
                    return Color.blue; // 青
                case 6:
                    return new Color(0.29f, 0f, 0.51f); // インディゴ
                case 7:
                    return new Color(0.93f, 0.51f, 0.93f); // 紫
                default:
                    return Color.white; // デフォルト色
            }
        }

        public static async UniTask<T> CreateUIAsync<TFactory, T>(string key, float timeoutSeconds = 5f)
            where TFactory : UIFactory<T>
        {
            float timer = 0f;
            TFactory factory = null;

            // キャッシュに登録されるまで待機（毎フレーム）
            while ((factory = ObjectFinder.Instance.GetObjectRuntimeCacheWithTag<TFactory>(key)) == null)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);

                timer += Time.deltaTime;
                if (timer > timeoutSeconds)
                {
#if UNITY_EDITOR
                    Debug.LogError($"UIFactoryがタイムアウトしました: key={key}");
#endif
                    return default;
                }
            }

            var ui = factory.CreateUI();
            if (ui != null)
            {
                return ui;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"key:{key} のオブジェクトの生成に失敗しました。Factoryのロジックに問題があるかもしれません");
#endif
                return default;
            }
        }

        public static IEnumerator CreateUIAsync<TFactory, T>(string key, System.Action<T> onCompleted)
            where TFactory : UIFactory<T>
        {
            TFactory factory = null;

            // nullである限り待機（毎フレームチェック）
            while ((factory = ObjectFinder.Instance.GetObjectRuntimeCacheWithTag<TFactory>(key)) == null)
            {
                yield return null;
            }

            var ui = factory.CreateUI();
            if (ui != null)
            {
                onCompleted?.Invoke(ui);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"key:{key} のオブジェクトの生成に失敗しました。Factoryのロジックに問題があるかもしれません");
#endif
                onCompleted?.Invoke(default);
            }
        }

        public static T CreateUI<TFactory, T>(string key) where TFactory : UIFactory<T>
        {
            var factory = ObjectFinder.Instance.GetObjectRuntimeCacheWithTag<TFactory>(key);
            if (factory != null)
            {
                var ui = factory.CreateUI();
                if (ui != null)
                {
                    return ui;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError("key:" + key + "のオブジェクトの生成に失敗しました。Factoryのロジックに問題があるかもしれません");
#endif
                    return default(T);
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("key:" + key + "のUIFactoryが見つかりませんでした。登録に失敗している可能性があります");
#endif
                return default(T);
            }
        }

        public static bool IsInvalid(this UnityEvent unityEvent)
        {
            int listenerCount = unityEvent.GetPersistentEventCount();

            if(listenerCount == 0)
            {
                return true;
            }

            for (int i = 0; i < listenerCount; i++)
            {
                // オブジェクトが登録されているか確認
                var target = unityEvent.GetPersistentTarget(i);
                // メソッドが指定されているか確認
                var methodName = unityEvent.GetPersistentMethodName(i);

                if (target == null || string.IsNullOrEmpty(methodName))
                {
                    return true; // 無効なリスナーがある
                }
            }

            return false; // 全てのリスナーが有効
        }

        public static Vector3 CatmullRom(Vector3[] points, int index, float t)
        {
            // Ensure the index is within bounds
            index = Mathf.Clamp(index, 0, points.Length - 2);

            Vector3 p0 = points[Mathf.Clamp(index - 1, 0, points.Length - 1)];
            Vector3 p1 = points[index];
            Vector3 p2 = points[Mathf.Clamp(index + 1, 0, points.Length - 1)];
            Vector3 p3 = points[Mathf.Clamp(index + 2, 0, points.Length - 1)];

            // Catmull-Rom spline formula
            return 0.5f * ((2f * p1) + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t + (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
        }

#if UNITY_EDITOR
        public const float ALPHA_MESH_PREVIEW_DEFAULT = 0.7f;
        /// <summary>
        /// シーン上でメッシュをプレビューできる
        /// 実体がないFactoryとか、移動する床系に使う
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="offset"></param>
        /// <param name="prefab"></param>
        public static void DrawSceneViewMeshPreview(this Transform transform
            , MeshFilter[] meshes
            , Vector3 offset
            , Color color
            , float alpha = ALPHA_MESH_PREVIEW_DEFAULT
            , GameObject targetPrefab = null)
        {
            Gizmos.color = new Color(color.r, color.g, color.b, alpha);
            MeshFilter[] objs;
            if (targetPrefab != null)
            {
                objs = targetPrefab.GetComponentsInChildren<MeshFilter>();
            }
            foreach (var obj in meshes)
            {
                if (obj == null)
                {
                    continue;
                }
                Mesh mesh = obj.sharedMesh;
                Vector3 parentPosOffset = Vector3.zero;
                if (obj.transform.parent != null)
                {
                    parentPosOffset = obj.transform.parent.position;
                }
                Gizmos.DrawMesh(mesh
                    , transform.position + obj.transform.position - parentPosOffset + offset
                    , obj.transform.rotation
                    , obj.transform.lossyScale);
            }
        }

        /// <summary>
        /// シーン上でメッシュをプレビューできる
        /// 実体がないFactoryとか、移動する床系に使う
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="offset"></param>
        /// <param name="prefab"></param>
        public static void DrawSceneViewMeshPreview(
            Vector3 position
            , MeshFilter[] meshes
            , Color color
            , float alpha = ALPHA_MESH_PREVIEW_DEFAULT
            , GameObject targetPrefab = null)
        {
            Gizmos.color = new Color(color.r, color.g, color.b, alpha);
            if (targetPrefab != null)
            {
                meshes = targetPrefab.GetComponentsInChildren<MeshFilter>();
            }
            foreach (var obj in meshes)
            {
                if (obj == null)
                {
                    continue;
                }
                Mesh mesh = obj.sharedMesh;
                Vector3 parentPosOffset = Vector3.zero;
                if (obj.transform.parent != null)
                {
                    parentPosOffset = obj.transform.parent.position;
                }
                Gizmos.DrawMesh(mesh
                    , position + obj.transform.position - parentPosOffset
                    , obj.transform.rotation
                    , obj.transform.lossyScale);
            }
        }

        /// <summary>
        /// シーン上でメッシュをプレビューできる
        /// 実体がないFactoryとか、移動する床系に使う
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="offset"></param>
        /// <param name="prefab"></param>
        public static void DrawSceneViewMeshPreview(this Transform transform
            , Quaternion rotateOffset
            , float alpha = ALPHA_MESH_PREVIEW_DEFAULT
            , GameObject targetPrefab = null)
        {
            Gizmos.color = new Color(1, 1, 1, alpha);
            if (transform.GetComponentInChildren<MeshFilter>())
            {
                MeshFilter[] objs;
                if (targetPrefab != null)
                {

                    objs = targetPrefab.GetComponentsInChildren<MeshFilter>();
                }
                else
                {
                    objs = transform.GetComponentsInChildren<MeshFilter>();
                }
                foreach (var obj in objs)
                {
                    MeshFilter filter;
                    if (targetPrefab != null)
                    {
                        filter = targetPrefab.GetComponentInChildren<MeshFilter>();
                    }
                    else
                    {
                        filter = transform.GetComponentInChildren<MeshFilter>();
                    }
                    Mesh mesh = filter.sharedMesh;
                    Vector3 parentPosOffset = Vector3.zero;
                    if (obj.transform.parent != null)
                    {
                        parentPosOffset = obj.transform.parent.position;
                    }
                    Gizmos.DrawMesh(mesh
                        , transform.position + Quaternion.Euler(transform.rotation.eulerAngles + rotateOffset.eulerAngles) * filter.transform.localPosition
                        , Quaternion.Euler(filter.transform.rotation.eulerAngles + rotateOffset.eulerAngles)
                        , filter.transform.lossyScale);
                }
            }
        }

        public static void PreviewSceneMesh<T>(Color color) where T : MonoBehaviour
        {
            foreach (var previewObject in GameObject.FindObjectsOfType<T>())
            {
                if (Selection.activeObject != previewObject.gameObject)
                {
                    Handles.BeginGUI();
                    Vector3 screenPosition = HandleUtility.WorldToGUIPoint(previewObject.transform.position);

                    // Adjust the button position to be centered on the object
                    Rect buttonRect = new Rect(screenPosition.x - 50, screenPosition.y, 80, 20);

                    Color originalColor = GUI.color;

                    // Set the GUI color to be semi-transparent
                    GUI.color = color; // 半透明の白色 (アルファ値0.5)

                    if (GUI.Button(buttonRect, previewObject.gameObject.name))
                    {
                        Selection.activeObject = previewObject.gameObject;
                        Event.current.Use(); // Prevent further event propagation
                    }

                    Handles.EndGUI();
                }
            }
        }
#endif

        public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
        {
            float currentTime = 0;
            audioMixer.GetFloat(exposedParam, out float currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }
            yield break;
        }

        public static float CalculateTimeToReachPath(Vector3[] path, float speed)
        {
            return CalculatePathDistance(path) / speed;
        }

        public static float CalculatePathDistance(Vector3[] path)
        {
            float totalDistance = 0f;

            for (int i = 0; i < path.Length - 1; i++)
            {
                float distance = Vector3.Distance(path[i], path[i + 1]);
                totalDistance += distance;
            }

            return totalDistance;
        }

        public static List<T> FindObjectsImplementingInterface<T>()
        {
            List<T> resultList = new List<T>();

            // 全てのアクティブなGameObjectを取得
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject go in gameObjects)
            {
                // GameObjectが無効であればスキップ
                if (!go.activeInHierarchy)
                    continue;

                // GameObjectにアタッチされているコンポーネントを取得
                MonoBehaviour[] components = go.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour component in components)
                {
                    // コンポーネントが指定したインターフェースを実装しているかチェック
                    if (component is T interfaceObject)
                    {
                        resultList.Add(interfaceObject);
                    }
                }
            }

            return resultList;
        }

#if UNITY_EDITOR
        public static void DrawString(string text, Vector3 worldPos, Color? colour = null)
        {
            Handles.BeginGUI();
            Color defaultColor = GUI.color;
            if (colour.HasValue) GUI.color = colour.Value;
            var view = SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height - size.y * 2, size.x, size.y), text);
            GUI.color = defaultColor;
            Handles.EndGUI();
        }
#endif

        public static void RemoveSuffixInstantiated(this GameObject target)
        {
            target.name = target.name.Replace("(Clone)", "");
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public static List<T> ShuffleList<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        /// <summary>
        /// 与えられた総時間から、ランダムに切って最小インターバル以上のランダムな時間空けて繰り返し実行する
        /// </summary>
        /// <param name="action"></param>
        /// <param name="totalDuration">使える総時間</param>
        /// <param name="minDuration">毎回少なくともこれ以上のインターバルを空ける</param>
        public static void ScheduleRandomly(Action action, float totalDuration, float minDuration)
        {
            if (totalDuration <= 0 || minDuration <= 0 || minDuration > totalDuration)
            {
                throw new ArgumentException("Invalid durations provided.");
            }

            //最大回数＝総時間/最低インターバル秒
            int maxInvokeNum = (int)(totalDuration / minDuration);
            //与えられた総時間をバジェットとする
            float remainTotalTime = totalDuration;

            var intervals = CalculateRandomIntervals(totalDuration, minDuration);

            //決まった回数とインターバルで順番に実行をキューする
            Tween previousAction = null;
            for (int i = 0; i < intervals.Count; i++)
            {
                int currentIndex = i;  // to avoid closure problem
                previousAction = DOVirtual.DelayedCall(intervals[currentIndex], action.Invoke);

                if (currentIndex < intervals.Count - 1)
                {
                    previousAction.OnComplete(() =>
                    {
                        DOVirtual.DelayedCall(intervals[currentIndex + 1], action.Invoke);
                    });
                }
            }
        }

        /// <summary>
        /// 与えられた総時間から、ランダムに切り詰めてintervalのリストを作る
        /// </summary>
        /// <param name="totalDuration"></param>
        /// <param name="minDuration"></param>
        /// <returns></returns>
        private static List<float> CalculateRandomIntervals(float totalDuration, float minDuration)
        {
            List<float> intervals = new List<float>();

            float remainTotalTime = totalDuration; 
            while (remainTotalTime >= minDuration)
            {
                float currentInterval = UnityEngine.Random.Range(minDuration, remainTotalTime);
                intervals.Add(currentInterval);

                remainTotalTime -= currentInterval;
            }

            return intervals;
        }

        public static Vector3 RandomPowerVector(float minPower, float maxPower)
        {
            float xVec = UnityEngine.Random.Range(-1, 2) * UnityEngine.Random.Range(minPower, maxPower);
            float yVec = UnityEngine.Random.Range(-1, 2) * UnityEngine.Random.Range(minPower, maxPower);
            float zVec = UnityEngine.Random.Range(-1, 2) * UnityEngine.Random.Range(minPower, maxPower);
            return new Vector3(xVec, yVec, zVec);
        }

        public static Vector3 RandomVector()
        {
            int xVec = UnityEngine.Random.Range(-1, 2);
            int yVec = UnityEngine.Random.Range(-1, 2);
            int zVec = UnityEngine.Random.Range(-1, 2);
            return new Vector3(xVec, yVec, zVec);
        }
        
        public static Vector3 RandomVector(float min, float max)
        {
            var xVec = UnityEngine.Random.Range(min, max);
            var yVec = UnityEngine.Random.Range(min, max);
            var zVec = UnityEngine.Random.Range(min, max);
            return new Vector3(xVec, yVec, zVec);
        }

        public static Vector3 RandomVector8()
        {
            var dir = UnityEngine.Random.Range(0, 8);
            switch(dir)
            {
                case 0:
                    return Vector3.right;
                case 1:
                    return Vector3.left;
                case 2:
                    return Vector3.forward;
                case 3:
                    return Vector3.back;
                case 4:
                    return new Vector3(1f, 0, 1f);
                case 5:
                    return new Vector3(1f, 0, -1f);
                case 6:
                    return new Vector3(-1f, 0, 1f);
                case 7:
                    return new Vector3(-1f, 0, -1f);
                default:
                    return Vector3.zero;
            }
        }

        public static float GetCubicBezier(float time, float start, float controlPoint1, float controlPoint2, float end)
        {
            return (float)(Math.Pow(1 - time, 3) * start 
                + 3 * Math.Pow(1 - time, 2) * time * controlPoint1 
                + 3 * (1 - time) * Math.Pow(time, 2) * controlPoint2 
                + Math.Pow(time, 3) * end);
        }

        /// <summary>
        /// Vector2XYをVector3XZに変換する
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ConvertToVector3XZ(Vector2 vector)
        {
            return new Vector3(vector.x, 0, vector.y);
        }

#if UNITY_EDITOR
        /// <summary>
        /// シーン上の指定したオブジェクトを全部検索する
        /// 重いのでEditor専用
        /// </summary>
        /// <returns>指定したTで見つかったオブジェクトを返す</returns>
        public static List<T> SearchObjectInScene<T>() where T : MonoBehaviour
        {
            var list = new List<T>();

            //GameObjectを全て取得
            var gameObjectList = Resources.FindObjectsOfTypeAll<T>();

            //取得したGameObjectの中からTargetを探す
            foreach (var gameObjectInHierarchy in gameObjectList)
            {
                //Hierarchy上のものでなければスルー
                if (!AssetDatabase.GetAssetOrScenePath(gameObjectInHierarchy).Contains(".unity"))
                {
                    continue;
                }

                list.Add(gameObjectInHierarchy);
            }

            return list;
        }
#endif

        public static void WaitOneFrameAction(this MonoBehaviour behaviour, Action action)
        {
            behaviour.StartCoroutine(WaitOneFrame(action));
        }

        private static IEnumerator WaitOneFrame(Action action, int waitFrame = 1)
        {
            for (int i = 0; i < waitFrame; i++)
            {
                yield return null;
            }

            action?.Invoke();
        }

        public static void WaitFrameAction(this MonoBehaviour behaviour, int waitFrame, Action action)
        {
            behaviour.StartCoroutine(WaitOneFrame(action, waitFrame));
        }

        public static void WaitTimeAction(this MonoBehaviour behaviour, Action action, float time)
        {
            behaviour.StartCoroutine(Wait(action, time));
        }

        private static IEnumerator Wait(Action action, float time)
        {
            yield return new WaitForSeconds(time);

            action?.Invoke();
        }

        /// <summary>
        /// リストの中で最も数値が近いものを探す
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float Nearest(this IEnumerable<float> self, float target)
        {
            var min = self.Min(c => Math.Abs(c - target));
            return self.First(c => Math.Abs(c - target) == min);
        }

        /// <summary>
        /// リストの中で最も数値が近いものを探す
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int Nearest(this IEnumerable<int> self, int target)
        {
            var min = self.Min(c => Math.Abs(c - target));
            return self.First(c => Math.Abs(c - target) == min);
        }

        /// <summary>
        /// リストの中で最も距離が近いものを探す
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Transform Nearest(this List<Transform> self, Transform target)
        {
            var min = self.Min(c => Math.Abs(c.position.magnitude - target.position.magnitude));
            return self.First(c => Math.Abs(c.position.magnitude - target.position.magnitude) == min);
        }

        public static bool IsNullOrCountZero<T>(this List<T> list)
        {
            return list == null || list.Count <= 0;
        }

        /// <summary>
        /// 対象との間に障害物が無いかを調べる 高さが関係無いバージョン
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNoObstacleBetweenTargetXZ(this Transform self, GameObject target, float height = 0f, Vector3? targetOffset = null, Vector3? selfOffset = null)
        {
            if (targetOffset == null)
            {
                targetOffset = Vector3.zero;
            }
            if(selfOffset == null)
            {
                selfOffset = Vector3.zero;
            }
            Vector3 posDelta = (target.transform.position + targetOffset.Value) - (self.position + selfOffset.Value);
            if (Physics.Raycast(self.position, new Vector3(posDelta.x, height, posDelta.z), out RaycastHit hit))
            {
                //障害物無し
                if (hit.collider.gameObject == target)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 対象との間に障害物が無いかを調べる
        /// 判定甘め版
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNoObstacleBetweenTargetXZBoxCast
            (this Transform self
            , GameObject target
            , LayerMask layerMask
            , float heightTarget = 0f
            , Vector3? targetOffset = null
            , Vector3? selfOffset = null)
        {
            if(targetOffset == null)
            {
                targetOffset = Vector3.zero;
            }
            if (selfOffset == null)
            {
                selfOffset = Vector3.zero;
            }
            Vector3 posDelta 
                = (target.transform.position + targetOffset.Value) - (self.position + selfOffset.Value);
            if (Physics.BoxCast(self.position
                , Vector3.one * 0.1f
                , new Vector3(posDelta.x, heightTarget, posDelta.z)
                , out RaycastHit hit
                , Quaternion.identity
                , Mathf.Infinity
                , layerMask))
            {
                //障害物無し
                if (hit.collider.gameObject == target)
                {
                    return true;
                }
            }
#if UNITY_EDITOR
            if (hit.collider != null && hit.collider.gameObject != null)
            {
                //Debug.Log(hit.collider.gameObject.name + "が間にある");
            }
            else
            {
                //Debug.Log("当たらなかった");
            }
#endif
            return false;
        }

        /// <summary>
        /// 対象との間に障害物が無いかを調べる
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNoObstacleBetweenTarget
            (this Transform self
            , GameObject target
            , Vector3 offset
            , LayerMask layer)
        {
            Vector3 posDelta = (target.transform.position + offset) - self.position;
            if (Physics.Raycast(
                self.position
                , posDelta
                , out RaycastHit hit
                , posDelta.magnitude
                , layer
                , QueryTriggerInteraction.Ignore))
            {
                //障害物無し
                if (hit.collider.gameObject == target)
                {
#if UNITY_EDITOR
                    //Debug.Log(target.name + "Marker:障害物なし");
#endif
                    return true;
                }
#if UNITY_EDITOR
                //Debug.Log(target.name + "Marker:" + hit.collider.name);
                return false;
#endif
            }
#if UNITY_EDITOR
            //Debug.Log(target.name + "Marker:何にもヒットしなかった");
#endif
            //targetにすらヒットしなかった場合は一旦trueとする
            return true;
        }

        /// <summary>
        /// Buttonのコリジョンを描画する
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="raycastPadding"></param>
        public static void DrawButtonCollision(Button button)
        {
            var trans = button.transform as RectTransform;
            var raycastPadding = button.image.raycastPadding;
            var pos = new Vector3(trans.position.x + (raycastPadding.x / 2) - (raycastPadding.z / 2),
                        trans.position.y + (raycastPadding.y / 2) - (raycastPadding.w / 2), 0);
            var size = new Vector3((trans.rect.width - (raycastPadding.x + raycastPadding.z)) * trans.lossyScale.x,
                                    (trans.rect.height - (raycastPadding.y + raycastPadding.w)) * trans.lossyScale.y, 0.1f);
            Gizmos.DrawCube(pos, size);
        }

        public static Tweener TweenColorAlpha(this Image image, float from, float to, float duration, TweenCallback onComplete)
        {
            image.ChangeColorAlpha(from);
            Tweener tween = DOVirtual.Float(from, to, duration, (alfa) =>
            {
                image.ChangeColorAlpha(alfa);
            }).OnComplete(onComplete);
            return tween;
        }

        public static void ChangeColorAlpha(this Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
        
        /// <summary>
        /// 自分の親Canvasをサーチして返す
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform SearchTopCanvas(Transform transform)
        {
            Transform parent = transform;
            while (true)
            {
                if (parent.GetComponent<Canvas>() != null)
                {
                    return parent;
                }
                parent = parent.parent;
            }
        }

        /// <summary>
        /// Transformの孫以下の特定名のノードを探索する
        /// Transform.Find()は1個下の階層しか見れない
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindChildRecursive(this Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;

                Transform result = FindChildRecursive(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// AudioMixer用
        /// floatの音量をデシベルに変換する
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static float ConvertVolumeToDb(float volume)
        {
            var clamped = Mathf.Clamp(volume, 0.0001f, 10.0f);
            var logged = Mathf.Log10(clamped);
            var db = Mathf.Clamp(logged * 20f, -80f, 20f);
            return db;
        }

        /// <summary>
        /// AudioMixer用
        /// デシベルをfloatの音量に変換する
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static float ConvertDbToVolume(float db)
        {
            var clampedDb = Mathf.Clamp(db, -80f, 20f);
            var volume = Mathf.Pow(10f, clampedDb / 20f);
            return volume;
        }

        public static AudioMixerGroup GetMixerGroup(this AudioMixer mixer, string path)
        {
            AudioMixerGroup[] groups = mixer.FindMatchingGroups(path);

            if (groups.Length > 0)
            {
#if UNITY_EDITOR
                Debug.Log("取得したグループ: " + groups[0].name);
#endif
                return groups[0];
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("指定したグループが見つかりませんでした。");
            }
#endif
            return null;
        }

        public static float GetRightOrLeftVector(MoveDirection dir)
        {
            float vec = 0.0f;
            switch (dir)
            {
                case MoveDirection.Left:
                    {
                        vec = 1.0f;
                        break;
                    }
                case MoveDirection.Right:
                    {
                        vec = -1.0f;
                        break;
                    }
            }
            return vec;
        }

        /// <summary>
        /// 指定したResourcesを生成してコンポーネントの形で返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T InstantiateFromResources<T>(string path) where T : MonoBehaviour
        {
            var res = Resources.Load(path);
            var obj = GameObject.Instantiate(res) as GameObject;
            var comp = obj.GetComponent<T>();
            if(comp == null)
            {
                comp = obj.GetComponentInChildren<T>();
            }
            if (comp == null)
            {
                return default(T);
            }
            return comp;
        }

        /// <summary>
        /// 子供として指定したResourcesのプレハブを作る
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static T InstantiateFromResourcesAsChild<T>(string path, Transform parent, string objName)
        {
            var res = Resources.Load(path);
            var obj = GameObject.Instantiate(res, parent) as GameObject;
            obj.name = objName;
            var button = obj.GetComponent<T>();
            return button;
        }

        ///// <summary>
        ///// 指定したResourcesを生成してコンポーネントの形で返す
        ///// Instantiateを1F待つので低負荷(だと思う)
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public static T InstantiateFromResourcesDeffered<T>(string path) where T : MonoBehaviour
        //{
        //    StartCoroutine(LoadResourcesWaitOneFrame());
        //    var targetObj = 
        //    if (comp != null)
        //    {
        //        return default(T);
        //    }
        //    return comp;
        //}

        ///// <summary>
        ///// 1フレーム待ち生成。負荷軽減
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public static IEnumerator LoadResourcesWaitOneFrame(string path)
        //{
        //    var res = Resources.Load(path);
        //    yield return null; //1フレーム待つ
        //    var targetObj = GameObject.Instantiate(res) as GameObject;
        //    yield return targetObj;
        //}

        /// <summary>
        /// 指定したResourcesを生成してコンポーネントの形で返す
        /// 非同期でロードし、1フレーム待ってからInstantiateするのできっと低負荷
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T InstantiateFromResourcesAsyncDeffered<T>(string path) where T : MonoBehaviour
        {
            var res = Resources.Load(path);
            var debugWindow = GameObject.Instantiate(res) as GameObject;
            var comp = debugWindow.GetComponentInChildren<T>();
            if (comp != null)
            {
                return default(T);
            }
            return comp;
        }

        /// <summary>
        /// 非同期ロード、1フレーム待ち生成
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerator LoadResourcesAsyncWaitOneFrame(string path)
        {
            var request = Resources.LoadAsync(path);
            while (!request.isDone) // ロード完了待ち
            {
                yield return null;
            }
            yield return null; //ロード完了したら1フレーム待って生成
            var targetObj = GameObject.Instantiate(request.asset) as GameObject;
            yield return targetObj;
        }

        /// <summary>
        /// エラーを吐いてくれるFindGameObjectWithTag
        /// </summary>
        /// <param name="tag">検索するtag</param>
        /// <param name="outputNullRef">見つからなかった際に独自の出力を出したい場合</param>
        /// <returns></returns>
        public static GameObject FindGameObjectWithTagError(string tag, string outputNullRef = "")
        {
            if (string.IsNullOrEmpty(tag))
            {
#if UNITY_EDITOR
                Debug.LogError("ERROR:検索しようとしたtagが空かnullになっています");
#endif
                return null;
            }
            var obj = GameObject.FindGameObjectWithTag(tag);
            if (obj == null)
            {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(outputNullRef))
                {
                    Debug.LogError("ERROR:tag:" + tag + "のオブジェクトは見つかりませんでした");
                }
                else
                {
                    Debug.LogError(outputNullRef);
                }
#endif
                return null;
            }
            return obj;
        }

        /// <summary>
        /// エラーを吐いてくれるFindGameObjectWithTagを指定した型に変換してくれるもの
        /// </summary>
        /// <param name="tag">検索するtag</param>
        /// <param name="outputNullRef">nullだったときに出す出力</param>
        /// <returns></returns>
        public static T FindGameObjectError<T>(string outputNullRef = "") where T : MonoBehaviour
        {
            var obj = GameObject.FindObjectOfType<T>();
            if (obj == null)
            {
#if UNITY_EDITOR
                Debug.LogError("ERROR:" + typeof(T).Name + "のオブジェクトが見つかりませんでした。");
#endif
                return null;
            }
            return obj;
        }

        /// <summary>
        /// エラーを吐いてくれるFindGameObjectWithTagを指定した型に変換してくれるもの
        /// </summary>
        /// <param name="tag">検索するtag</param>
        /// <param name="outputNullRef">nullだったときに出す出力</param>
        /// <returns></returns>
        public static T FindGameObjectWithTagError<T>(string tag, string outputNullRef = "") where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(tag))
            {
#if UNITY_EDITOR
                Debug.LogError("ERROR:検索しようとしたtagが空かnullになっています");
#endif
                return null;
            }
            var obj = GameObject.FindGameObjectWithTag(tag);
            if (obj == null)
            {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(outputNullRef))
                {
                    Debug.LogError("ERROR:tag:" + tag + "のオブジェクトは見つかりませんでした");
                }
                else
                {
                    Debug.LogError(outputNullRef);
                }
#endif
                return null;
            }
            T typeObj = obj.GetComponent<T>();
            if (typeObj == null)
            {
#if UNITY_EDITOR
                Debug.LogError("ERROR:tag:" + tag + "のオブジェクトは見つかりましたが、" + typeof(T).Name + "に変換できません。");
#endif
                return null;
            }
            return typeObj;
        }

        /// <summary>
        /// tagのついたオブジェクトを探して、登録したコールバックを再生する
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="action">見つかったあとに行うアクション</param>
        /// <param name="outputNullRef"></param>
        public static void ActionTaggedGameObject(string tag, Action<GameObject> action, string outputNullRef = "")
        {
            if(action == null)
            {
                return;
            }
            var target = FindGameObjectWithTagError(tag, outputNullRef);
            if(target != null)
            {
                action?.Invoke(target);
            }
        }

        /// <summary>
        /// tagのついた指定したコンポーネントを探して、登録したコールバックを再生する
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="action">見つかったあとに行うアクション</param>
        /// <param name="outputNullRef"></param>
        public static void ActionTaggedGameObject<T>(string tag, Action<T> action, string outputNullRef = "") where T : MonoBehaviour
        {
            if (action == null)
            {
                return;
            }
            var target = FindGameObjectWithTagError<T>(tag, outputNullRef);
            if (target != null)
            {
                action?.Invoke(target);
            }
        }

        /// <summary>
        /// tagのついた指定したコンポーネントを探して、登録したコールバックを再生する
        /// 見つからなかった時のコールバック付き
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="action">見つかったあとに行うアクション</param>
        /// <param name="outputNullRef"></param>
        public static void ActionTaggedGameObject<T>(string tag, Action<T> action, Action ifNullCallback, string outputNullRef = "") where T : MonoBehaviour
        {
            if (action == null)
            {
                return;
            }
            var target = FindGameObjectWithTagError<T>(tag, outputNullRef);
            if (target != null)
            {
                action?.Invoke(target);
            }
            else
            {
                ifNullCallback?.Invoke();
            }
        }

#if UNITY_EDITOR
        public static List<T> GetAllObject<T>() where T : MonoBehaviour
        {
            List<T> list = new List<T>();
            var target = Resources.FindObjectsOfTypeAll<T>();
            foreach (var obj in target)
            {
                //Hierarchy上のものでなければスルー
                if (!AssetDatabase.GetAssetOrScenePath(obj).Contains(".unity"))
                {
                    continue;
                }
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// Missingになっている項目を削除しつつオブジェクトの名前を整理した上で新しく子オブジェクトを追加する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="serializedList"></param>
        /// <param name="componentList"></param>
        /// <param name="baseNameChildren"></param>
        /// <param name="onCreate"></param>
        public static List<T> AddChildSorted<T>(Transform parent, List<T> serializedList, string baseNameChildren, bool isGUI = false, Action<GameObject> onCreate = null) where T : UnityEngine.Object
        {
            var newList = new List<T>();
            int index = 0;
            foreach (var target in serializedList)
            {
                if (target == null)
                {
                    continue;
                }
                index++;
                target.name = baseNameChildren + index;
                newList.Add(target);
            }
            if(newList.Count == 0)
            {
                serializedList.Clear();
            }
            serializedList = newList;
            int numOfIndex = serializedList.Count + 1;
            var child = new GameObject(baseNameChildren + numOfIndex);
            child.transform.parent = parent;
            if (isGUI)
            {
                var rect = child.AddComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
            }
            else
            {
                child.transform.position = Vector3.zero;
            }
            onCreate?.Invoke(child);

            var component = child.GetComponent<T>();
            if(component != null)
            {
                serializedList.Add(component);
            }
            else
            {
                Debug.LogError("対象アイテム" + child.name + "に目的のComponent:" + typeof(T) + "がつけられませんでした");
            }
            return serializedList;
        }

        /// <summary>
        /// Missingになっている項目を削除しつつオブジェクトの名前を整理した上で新しく子オブジェクトを追加する
        /// Resources.Load版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="serializedList"></param>
        /// <param name="componentList"></param>
        /// <param name="baseNameChildren"></param>
        /// <param name="onCreate"></param>
        public static List<T> AddChildSorted<T>(string path, Transform parent, List<T> serializedList, string baseNameChildren, bool isGUI = false, Action<T> onCreate = null) where T : UnityEngine.Object
        {
            var newList = new List<T>();
            int index = 0;
            foreach (var target in serializedList)
            {
                if (target == null)
                {
                    continue;
                }
                index++;
                target.name = baseNameChildren + index;
                newList.Add(target);
            }
            if (newList.Count == 0)
            {
                serializedList.Clear();
            }
            serializedList = newList;
            int numOfIndex = serializedList.Count + 1;
            var res = Resources.Load(path);
            GameObject child = GameObject.Instantiate(res, parent) as GameObject;
            child.name = baseNameChildren + numOfIndex;
            if (isGUI)
            {
                var rect = child.AddComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
            }
            else
            {
                child.transform.position = Vector3.zero;
            }
            var component = child.GetComponent<T>();
            if (component != null)
            {
                onCreate?.Invoke(component);
                serializedList.Add(component);
            }
            else
            {
                Debug.LogError("対象アイテム" + child.name + "に目的のComponent:" + typeof(T) + "がつけられませんでした");
            }
            return serializedList;
        }
#endif
    }
}