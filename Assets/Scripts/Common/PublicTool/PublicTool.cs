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


        return new Vector3(posX, posY,1f) + new Vector3(-3, -0.25f, 0);
    }

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
    #endregion

}
