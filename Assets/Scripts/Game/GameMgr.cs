using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    public override void Init()
    {
        StartCoroutine(IE_InitGame());
    }

    public IEnumerator IE_InitGame()
    {
        yield return StartCoroutine(ExcelDataMgr.Instance.IE_Init());

        //LoadScene(SceneName.Menu);

        yield break;


    }
}
