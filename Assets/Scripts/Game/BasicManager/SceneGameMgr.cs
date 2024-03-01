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
        //�󶨵�ǰĻ�Ĺ����������������ϷӦ��ֻ��һĻ���Ի���
        GameMgr.Instance.curSceneGameMgr = this;

        //Initialization
        //��ʼ����ģ��
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
    /// ����Facility���ź�
    /// </summary>
    /// <param name="arg0"></param>
    private void SetFacilityEvent(object arg0)
    {
        SetFacilityInfo info = (SetFacilityInfo)arg0;
        //���Ŀ��λ���������
        if(sceneGameData.CheckSetFaclityValid(info.typeID, info.posID))
        {
            //�����ݲ����Facility
            FacilitySetData newFacilityData = sceneGameData.AddFacility(info.typeID, info.posID);
            //�ڱ��ֲ����Facility
            mapMgr.AddFacility(newFacilityData);
        }
        else
        {
            //֮�󲹷���
            Debug.Log("Not Enough Space");
        }
    }

    private void DeleteFacilityEvent(object arg0)
    {
        int keyID = (int)arg0;
        FacilitySetData tarData = sceneGameData.GetFacility(keyID);

        //����ɾ����
        if (tarData.typeID == 3001)
        {
            Debug.Log("Invalid Delete Target");
        }
        else
        {
            //����һ�°���ص���ɾ��
            for(int i = 0; i < tarData.listSlotOut.Count; i++)
            {
                //��ɾ�Է� ��ɾ�Լ�
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
                //��ɾ�Է� ��ɾ�Լ�
                Vector2Int otherInfo = tarData.listSlotIn[i];
                if (otherInfo.x >= 0)
                {
                    FacilitySetData otherData = sceneGameData.GetFacility(otherInfo.x);
                    otherData.DisjoinSlotOut(otherInfo.y);
                }
                tarData.DisjoinSlotIn(i);
            }
            //���ֲ�ɾ��
            mapMgr.UpdateLine();

            //��ɾ���ֲ��Facility
            mapMgr.RemoveFacility(keyID);
            //��ɾ���ݲ��
            sceneGameData.RemoveFacility(keyID);
        }
    }

    private void SetLinkEvent(object arg0)
    {
        SetLinkInfo info = (SetLinkInfo)arg0;
        //���ݲ�-���׷�����
        FacilitySetData tarDataOut = sceneGameData.GetFacility(info.outKeyID);
        tarDataOut.JoinSlotOut(info.outSlotID, info.inKeyID, info.inSlotID);
        //���ݲ�-��׷�����
        FacilitySetData tarDataIn = sceneGameData.GetFacility(info.inKeyID);
        tarDataIn.JoinSlotIn(info.inSlotID, info.outKeyID, info.outSlotID);
        //���ֲ�
        mapMgr.UpdateLine();
    }

    private void DeleteLinkEvent(object arg0)
    {
        DeleteLinkInfo info = (DeleteLinkInfo)arg0;
        FacilitySetData tarDataOut = sceneGameData.GetFacility(info.outKeyID);
        tarDataOut.DisjoinSlotOut(info.outSlotID);
        FacilitySetData tarDataIn = sceneGameData.GetFacility(info.inKeyID);
        tarDataOut.DisjoinSlotIn(info.inSlotID);
        //���ֲ�
        mapMgr.UpdateLine();
    }
    #endregion
}
