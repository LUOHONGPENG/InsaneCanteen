using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelClearUIMgr : MonoBehaviour
{
    public GameObject objPopup;
    public Text codeTitle;
    public Button btnOK;

    public void Init()
    {
        btnOK.onClick.RemoveAllListeners();
        btnOK.onClick.AddListener(delegate ()
        {
            EventCenter.Instance.EventTrigger("NextLevel", null) ;
            objPopup.SetActive(false);
        });

    }
    public void OnEnable()
    {
        EventCenter.Instance.AddEventListener("NextLevelUI", NextLevelUIEvent);
    }



    public void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("NextLevelUI", NextLevelUIEvent);
    }

    private void NextLevelUIEvent(object arg0)
    {
        bool isAllClear = (bool)arg0;
        if (isAllClear)
        {
            codeTitle.text = "tx_ui_levelClear".ToLanguageText();
            btnOK.gameObject.SetActive(false);
        }
        else
        {
            codeTitle.text = "tx_ui_orderFinish".ToLanguageText();
            btnOK.gameObject.SetActive(true);
        }
        objPopup.SetActive(true);

    }
}
