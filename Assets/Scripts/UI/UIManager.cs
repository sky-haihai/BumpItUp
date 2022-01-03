using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject selectButton;
    public GameObject spaceButton;
    public GameObject RestartPop;
    public GameObject QuitPop;
    
    private void SelectOrSpace(object m_object)
    {
        if(selectButton.activeSelf)
        {
            selectButton.SetActive(false);
            spaceButton.SetActive(true);
        }
        else
        {
            selectButton.SetActive(true);
            spaceButton.SetActive(false);
        }
    }
    void Start()
    {
        EventManager.Instance.Register("OnMusicSelected", SelectOrSpace);
    }

    public void OpenRestartPop()
    {
        RestartPop.SetActive(true);
    }
    public void CloseRestartPop()
    {
        RestartPop.SetActive(false);
    }
    public void OpenQuitPop()
    {
        QuitPop.SetActive(true);
    }
    public void CloseQuitPop()
    {
        QuitPop.SetActive(false);
    }
}
