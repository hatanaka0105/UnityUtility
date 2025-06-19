using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using System;

/// <summary>
/// 順番に非同期処理を処理していくクラス Dotween限定
/// </summary>
public class ObservableQueueWorker : IDisposable
{
    private bool running = false;
    private List<Func<IObservable<Tween>>> eventQueue = new List<Func<IObservable<Tween>>>();
    private bool isDisposed = false;

    /// <summary>
    /// 非同期処理を追加
    /// </summary>
    public void Enqueue(Func<IObservable<Tween>> asyncAction)
    {
        eventQueue.Add(asyncAction);
        if (!running)
        {
            running = true;
            SubscribeNext();
        }
    }

    /// <summary>
    /// 非同期処理を追加
    /// </summary>
    public void Clear()
    {
        eventQueue.Clear();
        running = false;
    }

    /// <summary>
    /// 先頭に割り込み
    /// </summary>
    public void InsertFirst(Func<IObservable<Tween>> asyncAction)
    {
        if (running)
        {
            eventQueue.Insert(0, asyncAction);
        }
        else
        {
            Enqueue(asyncAction);
        }
    }

    /// <summary>
    /// 現在進行しているイベントが終わったらDisposeする
    /// </summary>
    public void EndLoop()
    {
#if UNITY_EDITOR
        Debug.Log("End Loop");
#endif
        InsertFirst(() => 
        {
            Observable.Start(Dispose); 
            return null; 
        });
    }

    // 次のObservableを発火させる
    void SubscribeNext()
    {
        if (eventQueue.Count == 0 || isDisposed)
        {
#if UNITY_EDITOR
            Debug.Log("Stop Running");
#endif
            running = false;
            return;
        }

        var e = eventQueue[0];
        eventQueue.RemoveAt(0);

        e().Finally(() => SubscribeNext()).Subscribe();
    }

    public void Dispose()
    {
        isDisposed = true;
    }
}