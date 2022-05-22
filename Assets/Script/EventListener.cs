using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    public delegate void OnBoolChangeDelegate(bool newVal);
    public event OnBoolChangeDelegate OnVariableChange;

    private bool m_bool;
    public bool Boolean{
        get{
            return m_bool;
        }

        set{
            if(m_bool = value) return;
            if(OnVariableChange != null)
                OnVariableChange(!m_bool);
            m_bool = value;
        }
    }

    private EventListener menu = new EventListener();
    // Start is called before the first frame update
    void Start()
    {
        menu.OnVariableChange += Test;
    }

    // Update is called once per frame
    void Update()
    {
        menu.Boolean = Input.GetKeyDown(KeyCode.W);
    }

    public void Test(bool value){
        Debug.Log(value);
    }
}
