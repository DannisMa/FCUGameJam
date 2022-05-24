using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameControllor : MonoBehaviour
{
    [SerializeField]
    private InputField player_name_input_field;
    private const string player_name_pref_key = "Player Name";

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (player_name_input_field != null)
            player_name_input_field.onValueChanged.AddListener(name => OnPlayerNameChanged(name));

        string default_name = string.Empty;

        if (player_name_input_field != null)
            player_name_input_field.text = default_name;
        if (PlayerPrefs.HasKey(player_name_pref_key))
        {
            default_name =  PlayerPrefs.
            GetString(player_name_pref_key);
            player_name_input_field.text = default_name;
        }
            // 設定遊戲玩家的名稱
        PhotonNetwork.NickName = default_name;
    }

    private void OnPlayerNameChanged(string name){
        if(string.IsNullOrEmpty(name))
            return;
        name = player_name_input_field.text;
            // 設定遊戲玩家的名稱
        PhotonNetwork.NickName = name;
        PlayerPrefs.SetString(player_name_pref_key, name);
    }
}
