using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public FacilityButtonUIMgr facilityButtonUIMgr;



    private bool isInit = false;
    public void Init()
    {
        facilityButtonUIMgr.Init();
        isInit = true;
    }

}
