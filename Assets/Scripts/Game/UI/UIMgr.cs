using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public LevelUIMgr levelUIMgr;
    public ModeUIMgr modeUIMgr;
    public FacilityButtonUIMgr facilityButtonUIMgr;
    public FacilityPreviewUIMgr facilityPreviewUIMgr;
    public LinePreviewUIMgr linePreviewUIMgr;
    public LevelClearUIMgr levelClearUIMgr;

    private bool isInit = false;
    public void Init()
    {
        levelUIMgr.Init();
        modeUIMgr.Init();
        facilityButtonUIMgr.Init();
        facilityPreviewUIMgr.Init();
        linePreviewUIMgr.Init();
        levelClearUIMgr.Init();
        isInit = true;
    }

    /// <summary>
    /// 新关卡重置设施
    /// </summary>
    public void NewLevelSetFacility(List<int> listFacility)
    {
        facilityButtonUIMgr.SetFacility(listFacility);
    }

    /// <summary>
    /// 新关卡刷新订单
    /// </summary>
    public void NewLevelUpdateOrder()
    {
        levelUIMgr.NewLevel();
    }

    /// <summary>
    /// 停止烹饪清除食物
    /// </summary>
    public void StopCookUpdateFood()
    {
        levelUIMgr.UpdateFood();
    }
}
