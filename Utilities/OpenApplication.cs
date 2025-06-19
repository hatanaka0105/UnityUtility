using UnityEngine;

/// <summary>
/// 外部アプリ連携関連
/// </summary>
public static class ExternalApplicationUtility
{
    /// <summary>
    /// LINEホーム開く
    /// </summary>
    public static void OpenApplication()
    {
        Application.OpenURL("http://line.naver.jp/");
    }

    /// <summary>
    /// 指定URLのアプリを開く
    /// </summary>
    public static void OpenApplication(string URL)
    {
        Application.OpenURL(URL);
    }

    public static void OpenTwitterAccount(string userName)
    {
        Application.OpenURL("https://twitter.com/" + userName);
    }
}