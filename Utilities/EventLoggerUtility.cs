using UnityEngine;
using System;
using System.IO;

public static class EventLoggerUtility
{
    private static string logFilePath;

    static EventLoggerUtility()
    {
        // ログファイルのパスを設定
        logFilePath = Path.Combine(Application.persistentDataPath, "event_log_ano.txt");
        Debug.Log($"Unified log file path: {logFilePath}");
    }

    // イベントをログに記録するメソッド
    public static void Log(string category, string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logMessage = $"{timestamp}\t{category}\t{message}";
        
        try
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(logMessage);
            }
            Debug.Log($"Logged: {logMessage}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error writing to log file: {e.Message}");
        }
    }
}