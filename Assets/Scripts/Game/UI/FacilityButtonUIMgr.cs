using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityButtonUIMgr : MonoBehaviour
{
    public Transform tfButton;
    public GameObject pfButton;

    public void Init()
    {
        InitButton();
    }

    public void InitButton()
    {
        PublicTool.ClearChildItem(tfButton);

        for(int i = 0; i < GameGlobal.testFacilityID.Length; i++)
        {
            int tempID = GameGlobal.testFacilityID[i];
            GameObject objButton = GameObject.Instantiate(pfButton, tfButton);
            FacilityButtonUIItem itemButton = objButton.GetComponent<FacilityButtonUIItem>();
            itemButton.Init(tempID);
        }
    }
}
