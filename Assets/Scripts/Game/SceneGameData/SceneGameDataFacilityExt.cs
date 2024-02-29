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

    /// <summary>
    /// 放置设施（增）
    /// </summary>
    public void AddFacility(int typeID, Vector2Int posID)
    {
        //检查是否位置被占了或者是否达到设施上限

        //先假设位置合理
        facilityKeyID++;
        FacilitySetData newFacilityData = new FacilitySetData(facilityKeyID,typeID,posID);
        listFacility.Add(newFacilityData);
        dicFacility.Add(facilityKeyID, newFacilityData);
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