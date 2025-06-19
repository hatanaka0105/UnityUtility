using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Globalization;

namespace UnityCustomExtension
{
    public class AutoRoomCheckTextInput : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject _successFindUI;

        private TMP_InputField _input;
        private List<RoomInfo> _roomList;

        private void Awake()
        {
            _input = GetComponent<TMP_InputField>();
        }

        public void OnValueChanged()
        {
            if (_input == null || _roomList == null || _roomList.Count == 0)
            {
                return;
            }

            bool isFind = false;
            string roomName = "";
            string input = _input.text;
            // ルームリストを展開して目的の部屋を探す
            foreach (RoomInfo roomInfo in _roomList)
            {
                if (roomInfo == null)
                {
                    continue;
                }
                roomName = roomInfo.Name;
                if (string.Compare(input, roomName, new CultureInfo("ja-JP"), CompareOptions.IgnoreWidth) == 0)
                {
                    if (_successFindUI != null)
                    {
                        _successFindUI.SetActive(true);
                    }
                    isFind = true;
                }
            }
            if (!isFind && _successFindUI != null)
            {
                _successFindUI.SetActive(false);
            }
        }

        // ルームリストに更新があった時
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            // 既存の部屋リストをクリア
            if (_roomList != null)
            {
                _roomList.Clear();
            }

            // 新しいルームリストに更新
            _roomList = roomList;
        }
    }
}