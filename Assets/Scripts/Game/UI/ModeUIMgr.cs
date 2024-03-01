using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeUIMgr : MonoBehaviour
{
    public Button btnBuild;
    public Button btnCook;

    public void Init()
    {
        btnBuild.onClick.RemoveAllListeners();
        btnBuild.onClick.AddListener(delegate ()
        {
            InputMgr.Instance.SetState(InputState.Build);
        });

        btnCook.onClick.RemoveAllListeners();
        btnCook.onClick.AddListener(delegate ()
        {
            InputMgr.Instance.SetState(InputState.Cook);
        });
    }


}
