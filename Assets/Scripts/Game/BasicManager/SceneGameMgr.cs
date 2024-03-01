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
        EventCenter.Instance.AddEventListener("SetLink", SetLinkEvent);
        EventCenter.Instance.AddEventListener("DeleteLink", DeleteLinkEvent);
    }


    public void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("SetFacility", SetFacilityEvent);
        EventCenter.Instance.RemoveEventListener("DeleteFacility", DeleteFacilityEvent);
        EventCenter.Instance.RemoveEventListener("SetLink", SetLinkEvent);
        EventCenter.Instance.RemoveEventListener("DeleteLink", DeleteLinkEvent);

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
            //补充一下把相关的线删除
            for(int i = 0; i < tarData.listSlotOut.Count; i++)
            {
                //先删对方 再删自己
                Vector2Int otherInfo = tarData.listSlotOut[i];
                if (otherInfo.x >= 0)
                {
                    FacilitySetData otherData = sceneGameData.GetFacility(otherInfo.x);
                    otherData.DisjoinSlotIn(otherInfo.y);
                }
                tarData.DisjoinSlotOut(i);
            }
            for (int i = 0; i < tarData.listSlotIn.Count; i++)
            {
                //先删对方 再删自己
                Vector2Int otherInfo = tarData.listSlotIn[i];
                if (otherInfo.x >= 0)
                {
                    FacilitySetData otherData = sceneGameData.GetFacility(otherInfo.x);
                    otherData.DisjoinSlotOut(otherInfo.y);
                }
                tarData.DisjoinSlotIn(i);
            }
            //表现层删线
            mapMgr.UpdateLine();

            //先删表现层的Facility
            mapMgr.RemoveFacility(keyID);
            //再删数据层的
            sceneGameData.RemoveFacility(keyID);
        }
    }

    private void SetLinkEvent(object arg0)
    {
        SetLinkInfo info = (SetLinkInfo)arg0;
        //数据层-出孔方设置
        FacilitySetData tarDataOut = sceneGameData.GetFacility(info.outKeyID);
        tarDataOut.JoinSlotOut(info.outSlotID, info.inKeyID, info.inSlotID);
        //数据层-入孔方设置
        FacilitySetData tarDataIn = sceneGameData.GetFacility(info.inKeyID);
        tarDataIn.JoinSlotIn(info.inSlotID, info.outKeyID, info.outSlotID);
        //表现层
        mapMgr.UpdateLine();
    }

    private void DeleteLinkEvent(object arg0)
    {
        DeleteLinkInfo info = (DeleteLinkInfo)arg0;
        FacilitySetData tarDataOut = sceneGameData.GetFacility(info.outKeyID);
        tarDataOut.DisjoinSlotOut(info.outSlotID);
        FacilitySetData tarDataIn = sceneGameData.GetFacility(info.inKeyID);
        tarDataOut.DisjoinSlotIn(info.inSlotID);
        //表现层
        mapMgr.UpdateLine();
    }
    #endregion
}
