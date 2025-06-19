using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オンライン時にプレイヤーが揃ったかどうかを管理するためのユーティリティ
/// OnAllPlayersReadyを購読することで、プレイヤーが揃った時の処理
/// OnTimeoutを購読することで、プレイヤーが揃わなかった時の処理を
/// それぞれ制御できる
/// NOTE:まだテストできていないが最初からシーン上になくても、Instantiateで後から生成しても使用可能、のはず
/// </summary>
namespace UnityCustomExtension
{
    public class PlayerReadyManager : MonoBehaviour
    {
        [SerializeField]
        private bool _autoStartEnabled = true;
        [SerializeField]
        private float _timeoutSeconds = 30f;

        private PhotonView _photonView;
        private List<int> _readyPlayers = new List<int>();
        private List<int> _sceneLoadedPlayers = new List<int>(); // シーンロード完了プレイヤーリスト
        private int _requiredPlayerCount;
        public int RequiredPlayerCount => _requiredPlayerCount;
        private bool _isWaiting;

        public event Action WaitPlayers;
        public event Action AllPlayersReady;
        public event Action AllSceneLoaded;
        public event Action Timeout;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            _photonView = GetComponent<PhotonView>();
            if (_photonView == null)
            {
#if UNITY_EDITOR                    
                Debug.LogError($"PhotonViewが設定されていません。:{name}");
#endif
            }
        }

        void Start()
        {
            if (!_autoStartEnabled) return;

            if (!PhotonNetwork.OfflineMode)
            {
                // HACK:とりあえず
                StartWaitingForPlayers(PhotonNetwork.PlayerList.Length);
            }
            else
            {
                StartWaitingForPlayers(1);
            }
        }

        public void SetAutoStartEnabled(bool autoStartEnabled)
        {
            _autoStartEnabled = autoStartEnabled;
        }


        /// <summary>
        /// プレイヤーの準備待ち開始
        /// </summary>
        /// <param name="playerCount">必要なプレイヤー数</param>
        public void StartWaitingForPlayers(int playerCount)
        {
            Debug.Log($"StartWaitingForPlayers: {playerCount}, {_isWaiting}, {PhotonNetwork.OfflineMode}, {PhotonNetwork.IsMasterClient}");


            if (_isWaiting) return;

            _requiredPlayerCount = playerCount;
            _readyPlayers.Clear();
            _isWaiting = true;

            if (PhotonNetwork.IsMasterClient)
            {
                // マスタークライアントは自動的に準備完了
                _readyPlayers.Add(PhotonNetwork.LocalPlayer.ActorNumber - 1);
                StartCoroutine(WaitForPlayersCoroutine());
            }
            else
            {
                // 他のクライアントは準備完了信号を送信
                StartCoroutine(SendReadySignalCoroutine());
            }

            WaitPlayers?.Invoke();
        }

        private IEnumerator WaitForPlayersCoroutine()
        {
            Debug.Log("WaitForPlayersCoroutine started");
            float elapsedTime = 0f;

            while (elapsedTime < _timeoutSeconds)
            {
                if (_readyPlayers.Count >= _requiredPlayerCount)
                {
                    // NotifyAllPlayersReady();
                    _photonView.RPC(nameof(NotifyAllPlayersReady), RpcTarget.All);
                    yield break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            NotifyTimeout();
        }

        private IEnumerator SendReadySignalCoroutine()
        {
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            
            // 準備完了まで定期的に信号を送信
            while (_isWaiting)
            {
                _photonView.RPC(nameof(ReceiveReadySignalRPC), RpcTarget.MasterClient, playerIndex);
                yield return new WaitForSeconds(1f);
            }
        }

        [PunRPC]
        private void ReceiveReadySignalRPC(int playerIndex)
        {
            if (!_readyPlayers.Contains(playerIndex))
            {
                _readyPlayers.Add(playerIndex);
            }
        }

        [PunRPC]
        private void NotifyAllPlayersReady()
        {
            _isWaiting = false;
            AllPlayersReady?.Invoke();
        }

        [PunRPC]
        private void NotifyTimeout()
        {
            Debug.Log("そろわなかった");
            _isWaiting = false;
            Timeout?.Invoke();
        }

        public void StopWaiting()
        {
            _isWaiting = false;
            StopAllCoroutines();
        }

        public bool IsPlayerReady(int playerIndex)
        {
            return _readyPlayers.Contains(playerIndex);
        }

        public int GetReadyPlayerCount()
        {
            return _readyPlayers.Count;
        }


        /// <summary>
        /// 各プレイヤーがシーンロードを完了した時に呼び出す
        /// </summary>
        public void NotifySceneLoaded()
        {
            if (PhotonNetwork.OfflineMode)
            {
                OnAllScenesLoaded();
                return;
            }

            _photonView.RPC(nameof(ReceiveSceneLoadedSignalRPC), RpcTarget.MasterClient, 
                PhotonNetwork.LocalPlayer.ActorNumber - 1);
        }

        [PunRPC]
        private void ReceiveSceneLoadedSignalRPC(int playerIndex)
        {
            if (!_sceneLoadedPlayers.Contains(playerIndex))
            {
                _sceneLoadedPlayers.Add(playerIndex);
                Debug.Log($"Player {playerIndex} loaded scene. Current count: {_sceneLoadedPlayers.Count}/{_requiredPlayerCount}");

                // 全プレイヤーのロード完了を確認
                if (_sceneLoadedPlayers.Count >= _requiredPlayerCount)
                {
                    _photonView.RPC(nameof(OnAllScenesLoaded), RpcTarget.All);
                }
            }
        }

        [PunRPC]
        private void OnAllScenesLoaded()
        {
            Debug.Log("All players loaded scenes");
            AllSceneLoaded?.Invoke();
            _sceneLoadedPlayers.Clear(); // リセット
        }

        // シーン切り替え時などにリセットするためのメソッド
        public void ResetSceneLoadedStatus()
        {
            _sceneLoadedPlayers.Clear();
        }

        public bool IsAllScenesLoaded()
        {
            return _sceneLoadedPlayers.Count >= _requiredPlayerCount;
        }

        public int GetSceneLoadedPlayerCount()
        {
            return _sceneLoadedPlayers.Count;
        }
    }
}