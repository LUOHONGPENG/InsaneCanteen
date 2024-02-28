using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoSingleton<GameMgr>
{
    public SceneGameMgr curSceneGameMgr = null;

    public bool isInit = false;
    public override void Init()
    {
        StartCoroutine(IE_InitGame());
    }

    public IEnumerator IE_InitGame()
    {
        yield return StartCoroutine(ExcelDataMgr.Instance.IE_Init());

        LoadScene(SceneName.Game);

        isInit = true;

        yield break;
    }

    public void LoadScene(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
