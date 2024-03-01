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
    //����ֵID�������½ǵ�ש��Ϊ׼
    //Position ID
    public Vector2Int posID = new Vector2Int(-1, -1);
    //����װSlot���ݵ�List,Vector2Int = (keyID,slotID)
    //(-1,-1)���ʾΪ��
    public List<Vector2Int> listSlotOut = new List<Vector2Int>();
    public List<Vector2Int> listSlotIn = new List<Vector2Int>();
    //������Cook
    //���Ź��������ʳ��
    public List<Queue<int>> listSlotStore = new List<Queue<int>>();
    public FacilityDeliverRequest deliverRequest = null;

    /// <summary>
    /// ��ȡ��ʩExcel����
    /// </summary>
    /// <returns></returns>
    public FacilityExcelItem GetExcelItem()
    {
        return PublicTool.GetFacilityItem(typeID);
    }

    public FacilitySetData(int keyID, int typeID, Vector2Int posID)
    {
        //������Ϣ��ʼ��
        this.keyID = keyID;
        this.typeID = typeID;
        this.posID = posID;
        FacilityExcelItem excelItem = GetExcelItem();
        if (excelItem != null)
        {
            facilityType = excelItem.type;
            //Slot��Ϣ��ʼ��
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

            //˳�㽫Cook��ش�����Ϣ��ʼ��
            //������л�Ͳ��������һ��
            listSlotStore.Clear();
            for (int i = 0; i < inSlotNum; i++)
            {
                Queue<int> queueFood = new Queue<int>();
                listSlotStore.Add(queueFood);
            }
        }
        deliverRequest = null;
    }

    #region Occupyռ�ÿռ�
    /// <summary>
    /// ����ռ�ݸ���
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
    /// ����ռ�ݸ���
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

    //������Facilityռ�ݵĸ���PosID������2X2��᷵���ĸ�����
    public List<Vector2Int> GetOccupyPosID()
    {
        return PublicTool.CalculateFacilityOccupy(posID, sizeX, sizeY);
    }
    #endregion

    #region Slot���ӿ�
    //���ӣ���Facility����Ŀ�
    public void JoinSlotOut(int thisSlotID, int otherKeyID, int otherSlotID)
    {
        if (thisSlotID < listSlotOut.Count)
        {
            Vector2Int otherInfo = listSlotOut[thisSlotID];
            //��������Ѿ��й���
            if (otherInfo.x >= 0)
            {
                FacilitySetData otherData = PublicTool.GetSceneGameData().GetFacility(otherInfo.x);
                otherData.DisjoinSlotIn(otherInfo.y);
            }
            listSlotOut[thisSlotID] = new Vector2Int(otherKeyID, otherSlotID);
        }
    }

    //��󣬸�Facility����Ŀ�
    public void DisjoinSlotOut(int thisSlotID)
    {
        if (thisSlotID < listSlotOut.Count)
        {
            listSlotOut[thisSlotID] = new Vector2Int(-1, -1);
        }
    }

    //���ӣ���Facility����Ŀ�
    public void JoinSlotIn(int thisSlotID, int otherKeyID, int otherSlotID)
    {
        if (thisSlotID < listSlotIn.Count)
        {
            //��������Ѿ��й���
            Vector2Int otherInfo = listSlotIn[thisSlotID];
            if (otherInfo.x >= 0)
            {
                FacilitySetData otherData = PublicTool.GetSceneGameData().GetFacility(otherInfo.x);
                otherData.DisjoinSlotOut(otherInfo.y);
            }
            listSlotIn[thisSlotID] = new Vector2Int(otherKeyID, otherSlotID);
        }
    }

    //��󣬸�Facility����Ŀ�
    public void DisjoinSlotIn(int thisSlotID)
    {
        if (thisSlotID < listSlotIn.Count)
        {
            listSlotIn[thisSlotID] = new Vector2Int(-1, -1);
        }
    }
    #endregion


    #region Cook������

    /// <summary>
    /// ��������Ƿ������ӣ������Ӳſ��Խ���ʳ��
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
    /// ���ܲ�����ʳ��
    /// </summary>
    /// <param name="typeID"></param>
    /// <param name="SlotID"></param>
    public void ReceiveFood(int typeID, int slotID)
    {
        //�������Ƿ�������
        if (CheckWhetherInSlotHaveLink(slotID) && slotID<listSlotStore.Count)
        {
            //����ʳ��
            listSlotStore[slotID].Enqueue(typeID);

            Debug.Log("AddFood " + slotID + " " + typeID);
        }
    }

    /// <summary>
    /// �������ʳ��
    /// </summary>
    public void ClearFood()
    {
        for(int i = 0; i < listSlotStore.Count; i++)
        {
            listSlotStore[i].Clear();
        }
    }


    /// <summary>
    /// ��������Ҫ��
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
    /// ��������Ҫ��
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

