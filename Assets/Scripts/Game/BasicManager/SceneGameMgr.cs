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
        //绑定当前幕的管理器，不过这次游戏应该只有一幕所以还好
        GameMgr.Instance.curSceneGameMgr = this;

        //Initialization
        //初始化各模块
        sceneGameData = new SceneGameData();
        sceneGameData.Init();
        uiMgr.Init();

        isInit = true;

        yield break;
    }
}
