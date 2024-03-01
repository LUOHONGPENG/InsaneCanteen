using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This part is about Facility
/// ��һ���ֻ������ʩ����
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
    /// �¹ؿ�����
    /// </summary>
    public void NewLevelFacility()
    {
        InitFacility();
        AddFacility(3001, new Vector2Int(15, 15));
    }

    /// <summary>
    /// ֹͣ���ʱ�����ã�����Ҫɾ���ߺ���ʩ������Ҫ���ʳ�����������
    /// </summary>
    public void StopCookFacility()
    {
        for(int i = 0; i < listFacility.Count; i++)
        {
            listFacility[i].ClearFood();
            listFacility[i].EndDeliverRequest();
        }
    }

    #region Basic Facility Function ��ɾ��
    /// <summary>
    /// ������ʩ������
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
    /// ж����ʩ��ɾ��
    /// </summary>
    /// <param name="keyID"></param>
    public void RemoveFacility(int keyID)
    {
        if (dicFacility.ContainsKey(keyID))
        {
            FacilitySetData targetFacility = dicFacility[keyID];
            //�ǵ��Ȱѹ�����ж��(֮��д)

            //ɾ�����ɾ����
            listFacility.Remove(targetFacility);
            dicFacility.Remove(keyID);
        }
    }


    /// <summary>
    /// ��ȡ���飩
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


    #region Faclity��ظ�������

    public bool CheckSetFaclityValid(int typeID,Vector2Int posID)
    {
        FacilityExcelItem excelItem = PublicTool.GetFacilityItem(typeID);
        List<Vector2Int> listToOccupy = PublicTool.CalculateFacilityOccupy(posID, excelItem.sizeX, excelItem.sizeY);
        List<Vector2Int> listNowOccupy = GetAllFacilityOccupy();

        //����Ƿ����
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

        //����Ƿ�ռ��
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
    /// ��ȡȫ��ʩռ�ݸ�
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

