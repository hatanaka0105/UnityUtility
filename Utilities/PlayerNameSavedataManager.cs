using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameSavedataManager : MonoBehaviour
{
    // KeyList
    /*
        Key1 PlayerName
        Key2 StageClearNo <- 一旦考えないことにする
    */

    [SerializeField]
    public TMPro.TMP_InputField playerNameText;

    public const string PLAYER_NAME_KEY = "PlayerName";

    // Start is called before the first frame update
    void Start()
    {
        if (playerNameText == null)
        {
            return;
        }

        // セーブデータがあるかチェックする
        if (Check())
        {
            // あればテキストにプレイヤーネームを書き込む
            playerNameText.text = Load();
        }
    }

    void Save()
    {
        // キーと値をセットする
        /*
        PlayerPrefs.SetInt("key1", 100);
        PlayerPrefs.SetFloat("key2", 3.333f);
        PlayerPrefs.SetString("key3", "sample message");
        */

        PlayerPrefs.SetString(PLAYER_NAME_KEY, playerNameText.text);
#if UNITY_EDITOR
        Debug.Log(PlayerPrefs.GetString(PLAYER_NAME_KEY) + ":名前をセーブしました");
#endif
        // 保存
        PlayerPrefs.Save();
    }

    string Load()
    {
        // キーを指定して値を読み込む（デフォルト値なし）
        /*
        int a1 = PlayerPrefs.GetInt("key1");      // キーkey1が存在しない場合、a1に0が入る。
        float b1 = PlayerPrefs.GetFloat("key2");  // キーkey2が存在しない場合、b1に0.0が入る。
        string c1 = PlayerPrefs.GetString("key3");// キーkey3が存在しない場合、c1に""（空文字）が入る。
        */

        // キーを指定して値を読み込む（デフォルト値あり）
        /*
        int a2 = PlayerPrefs.GetInt("key1", 99);         // キーkey1が存在しない場合、a2に99が入る。
        float b2 = PlayerPrefs.GetFloat("key2", 1.11f);  // キーkey2が存在しない場合、b2に1.11fが入る。
        string c2 = PlayerPrefs.GetString("key3", "test");      // キーkey3が存在しない場合、c2に"test"が入る。
        */

        // 名前が無かったらJohnが入る
        string s1 = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player");

#if UNITY_EDITOR
        Debug.Log(PlayerPrefs.GetString(PLAYER_NAME_KEY) + ":名前をロードしました");
#endif
        return s1;
    }

    bool Check()
    {
        // キーkey1が存在する場合、existにtrueが入り。存在しない場合falseが入る。
        bool exist = PlayerPrefs.HasKey(PLAYER_NAME_KEY);

        // 値が保存されているときの処理
        if (exist)
        {
            return true;
        }

        // 値が保存されていないときの処理
        return false;
    }

    public void PlayerNameSave()
    {
        Save();
    }
}