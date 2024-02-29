using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public FacilityButtonUIMgr facilityButtonUIMgr;
    public FacilityPreviewUIMgr facilityPreviewUIMgr;

    private bool isInit = false;
    public void Init()
    {
        facilityButtonUIMgr.Init();
        facilityPreviewUIMgr.Init();
        isInit = true;
    }

}
