using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeUIMgr : MonoBehaviour
{
    public Text txState;
    public Text txTip;
    public Button btnBuild;
    public Button btnCook;

    public void Init()
    {
        btnBuild.onClick.RemoveAllListeners();
        btnBuild.onClick.AddListener(delegate ()
        {
            InputMgr.Instance.SetState(InputState.Build);
            UpdateStateText();
        });

        btnCook.onClick.RemoveAllListeners();
        btnCook.onClick.AddListener(delegate ()
        {
            InputMgr.Instance.SetState(InputState.Cook);
            UpdateStateText();
        });

        UpdateStateText();
    }

    public void UpdateStateText()
    {
        switch (InputMgr.Instance.GetState())
        {
            case InputState.Build:
                txState.text = "tx_state_build".ToLanguageText();
                txTip.text = "tx_state_buildTip".ToLanguageText();
                break;
            case InputState.Cook:
                txState.text = "tx_state_cook".ToLanguageText();
                txTip.text = "tx_state_cookTip".ToLanguageText();
                break;
        }
    }

}
