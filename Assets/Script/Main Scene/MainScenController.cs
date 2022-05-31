using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainScenController : MonoBehaviour
{
    //start meanu
    [SerializeField]
    private GameObject m_charater_show_point;

    // charater menu
    [SerializeField]
    private Button charater_menu_btn;
    [SerializeField]
    private GameObject m_charater_panel;
    [SerializeField]
    private GameObject[] m_character_prefabs;
    [SerializeField]
    private GameObject m_character_icon;
    [SerializeField]
    private GameObject m_charater_menu;
    private ArrayList m_character_list = new ArrayList();

    
    // setting
    [SerializeField]
    private Button setting_menu_btn;
    [SerializeField]
    private GameObject m_setting_panel;
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

        if(charater_menu_btn != null)
            charater_menu_btn.onClick.AddListener(OnClickCharacterMenu);
        
        if(setting_menu_btn != null)
            setting_menu_btn.onClick.AddListener(OnClickSettingMenu);

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

        if(GameConst.Characater_name != null){
            OnChooseCharater(GameConst.Characater_name);
        }

        m_setting_panel.SetActive(false);
        m_charater_panel.SetActive(false);
    }

    private void OnPlayerNameChanged(string name){
        if(string.IsNullOrEmpty(name))
            return;
        name = player_name_input_field.text;
            // 設定遊戲玩家的名稱
        PhotonNetwork.NickName = name;
        PlayerPrefs.SetString(player_name_pref_key, name);
    }

    private void OnClickCharacterMenu(){
        m_setting_panel.SetActive(false);
        m_charater_panel.SetActive(true);
        if(m_character_list.Count != 0)
            return;
        foreach(GameObject cp in m_character_prefabs){
            GameObject temp = Instantiate(m_character_icon,m_charater_menu.transform);
            temp.transform.localScale = new Vector3(1f, 1f , 1f);
            temp.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => {OnChooseCharater(temp.transform.GetChild(2).name);});
            GameObject temp_c = Instantiate(cp,cp.transform.position , Quaternion.Euler(0f, 180f, 0f) ,temp.transform);
            temp_c.transform.localScale = new Vector3(100f, 100f , 100f);
            temp_c.transform.localPosition = new Vector3(0f, -70f, 0f);
            m_character_list.Add(temp);
        }
    }

    private void OnClickSettingMenu(){
        m_setting_panel.SetActive(true);
        m_charater_panel.SetActive(false);
    }

    private void OnChooseCharater(string name)
    {
        name = name.Split("(")[0];
        if(m_charater_show_point.transform.childCount != 0)
            Destroy(m_charater_show_point.transform.GetChild(0).gameObject);
        GameObject obj = Resources.Load(name) as GameObject;
        GameObject temp_c = Instantiate(obj, Vector3.zero/*obj.transform.position*/ , Quaternion.Euler(0f, 180f, 0f) , m_charater_show_point.transform);
        temp_c.transform.localScale =  new Vector3(10, 10 , 10);
        GameConst.Characater_name = name;
    }

}
