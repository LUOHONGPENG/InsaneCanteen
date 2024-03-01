using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class FacilitySetData
{
    //Every facility will have different keyID
    public int keyID = -1;
    //The type of facility will be determined by typeID
    public int typeID = -1;
    public FacilityType facilityType;
    //坐标值ID，以左下角的砖块为准
    //Position ID
    public Vector2Int posID = new Vector2Int(-1, -1);
    //用来装Slot数据的List,Vector2Int = (keyID,slotID)
    //(-1,-1)则表示为空
    public List<Vector2Int> listSlotOut = new List<Vector2Int>();
    public List<Vector2Int> listSlotIn = new List<Vector2Int>();
    //烹饪相关Cook
    //被放过来储存的食物
    public List<Queue<int>> listSlotStore = new List<Queue<int>>();
    public FacilityDeliverRequest deliverRequest = null;

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
        FacilityExcelItem excelItem = GetExcelItem();
        if (excelItem != null)
        {
            facilityType = excelItem.type;
            //Slot信息初始化
            int outSlotNum = 0;
            int inSlotNum = 0;
            switch (excelItem.type)
            {
                case FacilityType.Source:
                    outSlotNum = 1;
                    break;
                case FacilityType.Mixer:
                    outSlotNum = 1;
                    inSlotNum = excelItem.inSlot;
                    break;
                case FacilityType.End:
                    inSlotNum = 1;
                    break;
            }

            listSlotOut.Clear();
            for (int i = 0; i < outSlotNum; i++)
            {
                listSlotOut.Add(new Vector2Int(-1, -1));
            }
            listSlotIn.Clear();
            for (int i = 0; i < inSlotNum; i++)
            {
                listSlotIn.Add(new Vector2Int(-1, -1));
            }

            //顺便将Cook相关储存信息初始化
            //储存队列会和插入孔数量一致
            listSlotStore.Clear();
            for (int i = 0; i < inSlotNum; i++)
            {
                Queue<int> queueFood = new Queue<int>();
                listSlotStore.Add(queueFood);
            }
        }
        deliverRequest = null;
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
        return PublicTool.CalculateFacilityOccupy(posID, sizeX, sizeY);
    }
    #endregion

    #region Slot连接孔
    //连接，该Facility向外的孔
    public void JoinSlotOut(int thisSlotID, int otherKeyID, int otherSlotID)
    {
        if (thisSlotID < listSlotOut.Count)
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


    #region Cook烹饪相关

    /// <summary>
    /// 检查该入孔是否有连接，有连接才可以接收食物
    /// </summary>
    /// <returns></returns>
    public bool CheckWhetherInSlotHaveLink(int slotID)
    {
        if (slotID < listSlotIn.Count && listSlotIn[slotID].x >= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 接受并储存食物
    /// </summary>
    /// <param name="typeID"></param>
    /// <param name="SlotID"></param>
    public void ReceiveFood(int typeID, int slotID)
    {
        //检测入孔是否有连接
        if (CheckWhetherInSlotHaveLink(slotID) && slotID<listSlotStore.Count)
        {
            //塞入食物
            listSlotStore[slotID].Enqueue(typeID);

            Debug.Log("AddFood " + slotID + " " + typeID);
        }
    }

    /// <summary>
    /// 清空所有食物
    /// </summary>
    public void ClearFood()
    {
        for(int i = 0; i < listSlotStore.Count; i++)
        {
            listSlotStore[i].Clear();
        }
    }


    /// <summary>
    /// 创建运输要求
    /// </summary>
    /// <param name="foodTypeID"></param>
    /// <param name="tarFacilityKeyID"></param>
    /// <param name="tarFacilitySlotID"></param>
    public void CreateDeliverRequest(int foodTypeID,int tarFacilityKeyID,int tarFacilitySlotID)
    {
        if(deliverRequest == null)
        {
            deliverRequest = new FacilityDeliverRequest(foodTypeID, tarFacilityKeyID, tarFacilitySlotID);
        }
    }

    /// <summary>
    /// 结束运输要求
    /// </summary>
    public void EndDeliverRequest()
    {
        if(deliverRequest != null)
        {
            deliverRequest = null;
        }
    }
    #endregion
}

