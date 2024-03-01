using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneGameMgr
{

    #region Level关卡相关
    /// <summary>
    /// 下一关
    /// </summary>
    private void NextLevel()
    {
        if (sceneGameData.curLevelID < PublicTool.GetMaxLevel())
        {
            sceneGameData.curLevelID++;
            NewLevel();
        }
        else
        {
            //GameEnd
        }
    }

    /// <summary>
    /// 新一关重置
    /// </summary>
    private void NewLevel()
    {
        LevelExcelItem levelItem = PublicTool.GetLevelItem(sceneGameData.curLevelID);
        //重置交互
        InputMgr.Instance.SetState(InputState.Build);
        //重置数据
        sceneGameData.NewLevelFacility();
        sceneGameData.NewLevelOrder(new Vector2Int(levelItem.needFoodType, levelItem.needFoodNum));
        //重置地图
        mapMgr.InitFacility();
        mapMgr.UpdateLine();
        //重置UI,主要是提供的设施会变
        uiMgr.NewLevelSetFacility(levelItem.readyFacility);
    }

    #endregion

    #region Cook
    public bool isCooking = false;

    public void StartCook()
    {

    }

    //停止烹饪时候需要重置不少东西
    public void StopCook()
    {
        //重置数据
        sceneGameData.StopCookFacility();
        //重置地图
    }

    #endregion
}
