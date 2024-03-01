using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RuleExcelData;

public partial class PublicTool
{
    /// <summary>
    /// Useful function for clearing the child objects
    /// 用了一辈子的清除子物体方法，常用于UI
    /// </summary>
    /// <param name="tf"></param>
    public static void ClearChildItem(UnityEngine.Transform tf)
    {
        foreach (UnityEngine.Transform item in tf)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }
    }

    public static Vector3 ConvertPosFromID(Vector2Int posID)
    {
        int centerX = (GameGlobal.mapSizeX - 1) / 2;
        int centerY = (GameGlobal.mapSizeY - 1) / 2;

        float posX = (posID.x - centerX) * GameGlobal.mapTileSize;
        float posY = (posID.y - centerY) * GameGlobal.mapTileSize;


        return new Vector3(posX, posY,1f) + GameGlobal.mapDelta;
    }

    public static Vector2Int ConvertPosToID(Vector3 pos)
    {
        int centerX = (GameGlobal.mapSizeX - 1) / 2;
        int centerY = (GameGlobal.mapSizeY - 1) / 2;

        Vector3 tempPos = pos - GameGlobal.mapDelta;

        int posIDX = Mathf.RoundToInt(tempPos.x / GameGlobal.mapTileSize) + centerX;
        int posIDY = Mathf.RoundToInt(tempPos.y / GameGlobal.mapTileSize) + centerY;
        return new Vector2Int(posIDX, posIDY);
    }

    #region Convenient

    public static SceneGameMgr GetSceneGameMgr()
    {
        return GameMgr.Instance.curSceneGameMgr;
    }

    public static SceneGameData GetSceneGameData()
    {
        return GetSceneGameMgr().sceneGameData;
    }

    public static Vector2 GetMousePos()
    {
        return InputMgr.Instance.GetMousePos();
    }
    #endregion

    #region ForThisProject
    /// <summary>
    /// 计算设施的占据范围
    /// </summary>
    /// <param name="posID"></param>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <returns></returns>
    public static List<Vector2Int> CalculateFacilityOccupy(Vector2Int posID, int sizeX,int sizeY)
    {
        List<Vector2Int> tempPos = new List<Vector2Int>();
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                tempPos.Add(new Vector2Int(posID.x + i, posID.y + j));
            }
        }
        return tempPos;
    }

    /// <summary>
    /// 物体占据格最左下角以及物体中心的偏差距离（拖拽时鼠标中心会和物体中心一致）
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <returns></returns>
    public static Vector3 CalculateFacilityModelDelta(int sizeX,int sizeY)
    {
        return new Vector3(0.5f * GameGlobal.mapTileSize * (sizeX-1), 
            0.5f * GameGlobal.mapTileSize * (sizeY - 1), 0);
    }


    #endregion

    #region Excel
    //快速获取表格数据
    public static FacilityExcelItem GetFacilityItem(int facilityID)
    {
        return ExcelDataMgr.Instance.facilityExcelData.GetExcelItem(facilityID);
    }

    public static FoodExcelItem GetFoodItem(int foodID)
    {
        return ExcelDataMgr.Instance.foodExcelData.GetExcelItem(foodID);
    }

    public static string GetLanguageText(string key)
    {
        return ExcelDataMgr.Instance.languageExcelData.GetText(key);
    }
    #endregion

}
