using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllor : MonoBehaviour
{
    [SerializeField]
    private Button battle_button;
    [SerializeField]
    private TMP_Dropdown battle_scene_dropdown;

    // Start is called before the first frame update
    void Start()
    {
        // battle_scene_dropdown.onValueChanged.AddListener(delegate{
        //     DropdownValueChange(battle_scene_dropdown);
        // });
        battle_button.onClick.AddListener(ClickBattleBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ClickBattleBtn(){
        if(battle_scene_dropdown.value == 0)
            SceneManager.LoadScene("Battle Scene");
        else
         Debug.LogWarning(battle_scene_dropdown.value + "Scene not finish yet");
    }

    // private void DropdownValueChange(TMP_Dropdown dd){
    //     Debug.Log(dd.value);
    // }
}
