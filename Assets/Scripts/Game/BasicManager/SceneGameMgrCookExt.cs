using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneGameMgr
{

    #region Level�ؿ����
    /// <summary>
    /// ��һ��
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
    /// ��һ������
    /// </summary>
    private void NewLevel()
    {
        LevelExcelItem levelItem = PublicTool.GetLevelItem(sceneGameData.curLevelID);
        //���ý���
        InputMgr.Instance.SetState(InputState.Build);
        //��������
        sceneGameData.NewLevelFacility();
        sceneGameData.NewLevelOrder(new Vector2Int(levelItem.needFoodType, levelItem.needFoodNum));
        //���õ�ͼ
        mapMgr.InitFacility();
        mapMgr.UpdateLine();
        //����UI,��Ҫ���ṩ����ʩ���
        uiMgr.NewLevelSetFacility(levelItem.readyFacility);
    }

    #endregion

    #region Cook
    public bool isCooking = false;

    public void StartCook()
    {

    }

    //ֹͣ���ʱ����Ҫ���ò��ٶ���
    public void StopCook()
    {
        //��������
        sceneGameData.StopCookFacility();
        //���õ�ͼ
    }

    #endregion
}
