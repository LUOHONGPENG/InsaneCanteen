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

        AddFacility(3001, new Vector2Int(10, 10));
    }

    /// <summary>
    /// ������ʩ������
    /// </summary>
    public void AddFacility(int typeID, Vector2Int posID)
    {
        //����Ƿ�λ�ñ�ռ�˻����Ƿ�ﵽ��ʩ����

        //�ȼ���λ�ú���
        facilityKeyID++;
        FacilitySetData newFacilityData = new FacilitySetData(facilityKeyID,typeID,posID);
        listFacility.Add(newFacilityData);
        dicFacility.Add(facilityKeyID, newFacilityData);
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
}


#region FacilitySet

public class FacilitySetData
{
    //Every facility will have different keyID
    public int keyID = -1;
    //The type of facility will be determined by typeID
    public int typeID = -1;
    //Position
    public Vector2Int posID = new Vector2Int(-1, -1);

    public FacilitySetData(int keyID, int typeID, Vector2Int posID)
    {
        this.keyID = keyID;
        this.typeID = typeID;
        this.posID = posID;
    }
}

#endregion