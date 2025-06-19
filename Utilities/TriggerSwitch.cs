using System;
using UnityEngine;
using Photon.Pun;
using UnityCustomExtension.Audio;

public class TriggerSwitch : MonoBehaviourPunCallbacks, IPhotonTagSetter
{
    private PhotonView _view;

    private bool _isCollect = false;

    private Action _onPushedEvent;

    private Action _onReleaseEvent;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //一致する組み合わせのものだったらOKとする
        if (other.gameObject.tag == gameObject.tag)
        {
            SetCollect(true);
            if (_view.IsMine)
            {
                _view.RPC(nameof(SetCollect), RpcTarget.Others, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == gameObject.tag)
        {
            SetCollect(false);
            if (_view.IsMine)
            {
                _view.RPC(nameof(SetCollect), RpcTarget.Others, false);
            }
        }
    }

    [PunRPC]
    private void SetCollect(bool isCollect)
    {
#if UNITY_EDITOR
        Debug.Log(gameObject.name + " : "+ isCollect);
#endif
        _isCollect = isCollect;
        if(isCollect)
        {
#if UNITY_EDITOR
            Debug.Log(gameObject.name + "正解");
#endif
            _onPushedEvent?.Invoke();
        }

        AudioClipDictionary.Instance.PlayInstant("PushSwitch");
    }

    public bool IsCollect()
    {
        return _isCollect;
    }

    public void SetPushEvent(Action switchOnEvent, Action switchOffEvent)
    {
        _onPushedEvent += switchOnEvent.Invoke;
        _onReleaseEvent += switchOffEvent.Invoke;
    }

    public void SetTag(string tag)
    {
        if(_view.IsMine)
        {
            _view.RPC(nameof(RPCTag), RpcTarget.Others, tag);
        }
    }

    [PunRPC]
    private void RPCTag(string tag)
    {
        gameObject.tag = tag;
    }
}
