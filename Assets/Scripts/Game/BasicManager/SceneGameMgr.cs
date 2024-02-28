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

        //�󶨵�ǰĻ�Ĺ����������������ϷӦ��ֻ��һĻ���Ի���
        GameMgr.Instance.curSceneGameMgr = this;

        yield break;

        isInit = true;

    }
}
