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

        AddFacility(3001, new Vector2Int(10, 10));
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


#region FacilitySet

public class FacilitySetData
{
    //Every facility will have different keyID
    public int keyID = -1;
    //The type of facility will be determined by typeID
    public int typeID = -1;
    //坐标值ID，以左下角的砖块为准
    //Position ID
    public Vector2Int posID = new Vector2Int(-1, -1);
    //用来装Slot数据的List,Vector2Int = (keyID,slotID)
    //(-1,-1)则表示为空
    public List<Vector2Int> listSlotOut = new List<Vector2Int>();
    public List<Vector2Int> listSlotIn = new List<Vector2Int>();

    /// <summary>
    /// 获取设施Excel数据
    /// </summary>
    /// <returns></returns>
    public FacilityExcelItem GetExcelItem()
    {
        return PublicTool.GetFacilityItem(typeID);
    }

    public FacilitySetData(int keyID, int typeID, Vector2Int posID)
    {
        //基本信息初始化
        this.keyID = keyID;
        this.typeID = typeID;
        this.posID = posID;
        //Slot信息初始化
        FacilityExcelItem excelItem = GetExcelItem();
        if (excelItem != null)
        {
            listSlotOut.Clear();
            for(int i = 0; i < excelItem.outSlot; i++)
            {
                listSlotOut.Add(new Vector2Int(-1, -1));
            }
            listSlotIn.Clear();
            for (int i = 0; i < excelItem.inSlot; i++)
            {
                listSlotIn.Add(new Vector2Int(-1, -1));
            }
        }
    }

    #region Occupy占用空间
    /// <summary>
    /// 横向占据格数
    /// </summary>
    public int sizeX
    {
        get
        {
            if (GetExcelItem() != null)
            {
                return GetExcelItem().sizeX;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// 纵向占据格数
    /// </summary>
    public int sizeY
    {
        get
        {
            if (GetExcelItem() != null)
            {
                return GetExcelItem().sizeY;
            }
            else
            {
                return 0;
            }
        }
    }

    //获得这个Facility占据的格子PosID，比如2X2则会返回四个坐标
    public List<Vector2Int> GetOccupyPosID()
    {
        return PublicTool.CalculateFacilityOccupy(posID,sizeX,sizeY);
    }
    #endregion

    #region Slot连接孔
    //连接，该Facility向外的孔
    public void JoinSlotOut(int thisSlotID, int otherKeyID, int otherSlotID)
    {
        if(thisSlotID < listSlotOut.Count)
        {
            Vector2Int otherInfo = listSlotOut[thisSlotID];
            //如果本身已经有关联
            if (otherInfo.x >= 0)
            {
                FacilitySetData otherData = PublicTool.GetSceneGameData().GetFacility(otherInfo.x);
                otherData.DisjoinSlotIn(otherInfo.y);
            }
            listSlotOut[thisSlotID] = new Vector2Int(otherKeyID, otherSlotID);
        }
    }

    //解绑，该Facility向外的孔
    public void DisjoinSlotOut(int thisSlotID)
    {
        if (thisSlotID < listSlotOut.Count)
        {
            listSlotOut[thisSlotID] = new Vector2Int(-1, -1);
        }
    }

    //连接，该Facility进入的孔
    public void JoinSlotIn(int thisSlotID, int otherKeyID, int otherSlotID)
    {
        if (thisSlotID < listSlotIn.Count)
        {
            //如果本身已经有关联
            Vector2Int otherInfo = listSlotIn[thisSlotID];
            if (otherInfo.x >= 0)
            {
                FacilitySetData otherData = PublicTool.GetSceneGameData().GetFacility(otherInfo.x);
                otherData.DisjoinSlotOut(otherInfo.y);
            }
            listSlotIn[thisSlotID] = new Vector2Int(otherKeyID, otherSlotID);
        }
    }

    //解绑，该Facility进入的孔
    public void DisjoinSlotIn(int thisSlotID)
    {
        if (thisSlotID < listSlotIn.Count)
        {
            listSlotIn[thisSlotID] = new Vector2Int(-1, -1);
        }
    }
    #endregion
}

#endregion