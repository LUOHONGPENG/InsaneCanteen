using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameMgr : MonoBehaviour
{
    public Camera mapCamera;
    public Camera uiCamera;
    public MapMgr mapMgr;
    public UIMgr uiMgr;
    public SceneGameData sceneGameData;
    private bool isInit = false;

    public void Start()
    {
        StartCoroutine(IE_Init());
    }

    public IEnumerator IE_Init()
    {
        yield return new WaitUntil(() => GameMgr.Instance.isInit);

        //Bind current scene's game manager
        //绑定当前幕的管理器，不过这次游戏应该只有一幕所以还好
        GameMgr.Instance.curSceneGameMgr = this;

        //Initialization
        //初始化各模块
        sceneGameData = new SceneGameData();
        sceneGameData.Init();
        mapMgr.Init();
        uiMgr.Init();

        isInit = true;

        yield break;
    }

    #region EventDeal


    public void OnEnable()
    {
        EventCenter.Instance.AddEventListener("SetFacility", SetFacilityEvent);
        EventCenter.Instance.AddEventListener("DeleteFacility", DeleteFacilityEvent);

    }

    public void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("SetFacility", SetFacilityEvent);
        EventCenter.Instance.RemoveEventListener("DeleteFacility", DeleteFacilityEvent);

    }

    /// <summary>
    /// 设置Facility的信号
    /// </summary>
    /// <param name="arg0"></param>
    private void SetFacilityEvent(object arg0)
    {
        SetFacilityInfo info = (SetFacilityInfo)arg0;
        //检查目标位置这合理吗
        if(sceneGameData.CheckSetFaclityValid(info.typeID, info.posID))
        {
            //在数据层放置Facility
            FacilitySetData newFacilityData = sceneGameData.AddFacility(info.typeID, info.posID);
            //在表现层放置Facility
            mapMgr.AddFacility(newFacilityData);
        }
        else
        {
            //之后补反馈
            Debug.Log("Not Enough Space");
        }
    }

    private void DeleteFacilityEvent(object arg0)
    {
        int keyID = (int)arg0;
        FacilitySetData tarData = sceneGameData.GetFacility(keyID);

        //不许删盘子
        if (tarData.typeID == 3001)
        {
            Debug.Log("Invalid Delete Target");
        }
        else
        {
            //先删表现层的Facility
            mapMgr.RemoveFacility(keyID);
            //再删数据层的
            sceneGameData.RemoveFacility(keyID);
        }
    }


    #endregion
}
