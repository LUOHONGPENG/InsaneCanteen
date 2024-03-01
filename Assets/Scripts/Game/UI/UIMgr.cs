using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public ModeUIMgr modeUIMgr;
    public FacilityButtonUIMgr facilityButtonUIMgr;
    public FacilityPreviewUIMgr facilityPreviewUIMgr;
    public LinePreviewUIMgr linePreviewUIMgr;

    private bool isInit = false;
    public void Init()
    {
        modeUIMgr.Init();
        facilityButtonUIMgr.Init();
        facilityPreviewUIMgr.Init();
        linePreviewUIMgr.Init();
        isInit = true;
    }

}
