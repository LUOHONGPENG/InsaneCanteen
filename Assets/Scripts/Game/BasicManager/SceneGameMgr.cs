using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneGameMgr : MonoBehaviour
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

        StartGame();

        yield break;
    }

    private void StartGame()
    {
        sceneGameData.curLevelID = 1;
        NewLevel();
    }


    #region EventDeal


    public void OnEnable()
    {
        //�������
        EventCenter.Instance.AddEventListener("SetFacility", SetFacilityEvent);
        EventCenter.Instance.AddEventListener("DeleteFacility", DeleteFacilityEvent);
        EventCenter.Instance.AddEventListener("SetLink", SetLinkEvent);
        EventCenter.Instance.AddEventListener("DeleteLink", DeleteLinkEvent);

        //ģʽ���
        EventCenter.Instance.AddEventListener("BuildState", StopCook);
        EventCenter.Instance.AddEventListener("CookState", StartCook);
    }



    public void OnDestroy()
    {
        //�������
        EventCenter.Instance.RemoveEventListener("SetFacility", SetFacilityEvent);
        EventCenter.Instance.RemoveEventListener("DeleteFacility", DeleteFacilityEvent);
        EventCenter.Instance.RemoveEventListener("SetLink", SetLinkEvent);
        EventCenter.Instance.RemoveEventListener("DeleteLink", DeleteLinkEvent);
        //ģʽ���
        EventCenter.Instance.RemoveEventListener("BuildState", StopCook);
        EventCenter.Instance.RemoveEventListener("CookState", StartCook);
    }


    #endregion
}
