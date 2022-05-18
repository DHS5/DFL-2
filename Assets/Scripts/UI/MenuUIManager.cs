using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    
    
    [SerializeField] private GameObject infoButtons;


    // ### Properties ###

    public bool InfoButtonsOn
    {
        set
        {
            infoButtons.SetActive(value);
        }
    }


    private void Start()
    {
        settingsManager = SettingsManager.InstanceSettingsManager;
        settingsManager.GetManagers();
    }


    // ### Functions ###

    public void SetAsLastSibling(GameObject go)
    {
        go.transform.SetAsLastSibling();
    }
}
