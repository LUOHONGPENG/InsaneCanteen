using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameMgr : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(IE_Init());
    }

    private bool isInit = false;

    public IEnumerator IE_Init()
    {
        yield return new WaitUntil(() => GameMgr.Instance.isInit);

        //绑定当前幕的管理器，不过这次游戏应该只有一幕所以还好
        GameMgr.Instance.curSceneGameMgr = this;

        yield break;

        isInit = true;

    }
}
