using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameMgr : MonoBehaviour
{
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
        uiMgr.Init();

        isInit = true;

        yield break;
    }
}
