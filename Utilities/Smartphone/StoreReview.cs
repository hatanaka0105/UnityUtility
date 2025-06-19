using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
using Google.Play;
#endif

/// <summary>
/// レビュー機能
/// ネイティブの評価ウィンドウをポップアップする
/// </summary>
public class StoreReview : MonoBehaviour
{
    public void SendReview()
    {
#if UNITY_IOS
        UnityEngine.iOS.Device.RequestStoreReview();
#elif UNITY_ANDROID
        StartCoroutine(RequestReviewAndroid());
#endif
    }

#if UNITY_ANDROID
    private IEnumerator RequestReviewAndroid()
    {
        var reviewManager = new Google.Play.Review.ReviewManager();
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        var playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;
        playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
    }
#endif
}
