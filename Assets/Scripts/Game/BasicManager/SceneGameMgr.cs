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

    }

    public void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("SetFacility", SetFacilityEvent);
        EventCenter.Instance.RemoveEventListener("DeleteFacility", DeleteFacilityEvent);

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
            //��ɾ���ֲ��Facility
            mapMgr.RemoveFacility(keyID);
            //��ɾ���ݲ��
            sceneGameData.RemoveFacility(keyID);
        }
    }


    #endregion
}
