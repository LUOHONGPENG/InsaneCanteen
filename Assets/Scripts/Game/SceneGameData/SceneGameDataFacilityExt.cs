using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This part is about Facility
/// 这一部分会关于设施数据
/// </summary>
public partial class SceneGameData
{
    public List<FacilitySetData> listFacility = new List<FacilitySetData>();
    public Dictionary<int, FacilitySetData> dicFacility = new Dictionary<int, FacilitySetData>();
    public int facilityKeyID = -1;

    private void InitFacility()
    {
        listFacility.Clear();
        dicFacility.Clear();
        facilityKeyID = -1;
    }

    /// <summary>
    /// 新关卡重置
    /// </summary>
    public void NewLevelFacility()
    {
        InitFacility();
        AddFacility(3001, new Vector2Int(15, 15));
    }

    /// <summary>
    /// 停止烹饪时候重置，不需要删除线和设施，但是要清空食物和运输需求
    /// </summary>
    public void StopCookFacility()
    {
        for(int i = 0; i < listFacility.Count; i++)
        {
            listFacility[i].ClearFood();
            listFacility[i].EndDeliverRequest();
        }
    }

    #region Basic Facility Function 增删查
    /// <summary>
    /// 放置设施（增）
    /// </summary>
    public FacilitySetData AddFacility(int typeID, Vector2Int posID)
    {
        facilityKeyID++;
        FacilitySetData newFacilityData = new FacilitySetData(facilityKeyID,typeID,posID);
        listFacility.Add(newFacilityData);
        dicFacility.Add(facilityKeyID, newFacilityData);

        return newFacilityData;
    }

    /// <summary>
    /// 卸除设施（删）
    /// </summary>
    /// <param name="keyID"></param>
    public void RemoveFacility(int keyID)
    {
        if (dicFacility.ContainsKey(keyID))
        {
            FacilitySetData targetFacility = dicFacility[keyID];
            //记得先把关联项卸了(之后写)

            //删完关联删数据
            listFacility.Remove(targetFacility);
            dicFacility.Remove(keyID);
        }
    }


    /// <summary>
    /// 获取（查）
    /// </summary>
    /// <param name="keyID"></param>
    /// <returns></returns>
    public FacilitySetData GetFacility(int keyID)
    {
        if (dicFacility.ContainsKey(keyID))
        {
            return dicFacility[keyID];
        }
        else
        {
            return null;
        }
    }
    #endregion


    #region Faclity相关辅助方法

    public bool CheckSetFaclityValid(int typeID,Vector2Int posID)
    {
        FacilityExcelItem excelItem = PublicTool.GetFacilityItem(typeID);
        List<Vector2Int> listToOccupy = PublicTool.CalculateFacilityOccupy(posID, excelItem.sizeX, excelItem.sizeY);
        List<Vector2Int> listNowOccupy = GetAllFacilityOccupy();

        //检查是否出界
        if (posID.x < 0)
        {
            return false;
        }
        if (posID.x + excelItem.sizeX - 1 >= GameGlobal.mapSizeX)
        {
            return false;
        }
        if (posID.y < 0)
        {
            return false;
        }
        if (posID.y + excelItem.sizeY - 1 >= GameGlobal.mapSizeY)
        {
            return false;
        }

        //检查是否被占用
        for (int i = 0;i < listToOccupy.Count; i++)
        {
            if (listNowOccupy.Contains(listToOccupy[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 获取全设施占据格
    /// </summary>
    /// <returns></returns>
    public List<Vector2Int> GetAllFacilityOccupy()
    {
        List<Vector2Int> tempPos = new List<Vector2Int>();
        for (int i = 0; i < listFacility.Count; i++)
        {
            FacilitySetData tarFacility = listFacility[i];
            List<Vector2Int> tarOccupy = tarFacility.GetOccupyPosID();
            for(int j = 0; j < tarOccupy.Count; j++)
            {
                if (!tempPos.Contains(tarOccupy[j]))
                {
                    tempPos.Add(tarOccupy[j]);
                }
            }
        }
        return tempPos;
    }

    #endregion
}

