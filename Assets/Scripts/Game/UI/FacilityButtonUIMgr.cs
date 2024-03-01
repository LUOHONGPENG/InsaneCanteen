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

    }

    public void SetFacility(List<int> listFacility)
    {
        PublicTool.ClearChildItem(tfButton);

        for (int i = 0; i < listFacility.Count; i++)
        {
            int tempID = listFacility[i];
            GameObject objButton = GameObject.Instantiate(pfButton, tfButton);
            FacilityButtonUIItem itemButton = objButton.GetComponent<FacilityButtonUIItem>();
            itemButton.Init(tempID);
        }
    }
}
