using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此处主要用于装载Event使用的Info结构体
/// </summary>

public struct SetFacilityInfo
{
    public int typeID;
    public Vector2Int posID;

    public SetFacilityInfo(int typeID, Vector2Int posID)
    {
        this.typeID = typeID;
        this.posID = posID;
    }
}

public struct SetLinkInfo
{
    public int outKeyID;
    public int outSlotID;
    public int inKeyID;
    public int inSlotID;

    public SetLinkInfo(int outKeyID,int outSlotID,int inKeyID,int inSlotID)
    {
        this.outKeyID = outKeyID;
        this.outSlotID = outSlotID;
        this.inKeyID = inKeyID;
        this.inSlotID = inSlotID;
    }
}

public struct DeleteLinkInfo
{
    public int outKeyID;
    public int outSlotID;
    public int inKeyID;
    public int inSlotID;

    public DeleteLinkInfo(int outKeyID, int outSlotID, int inKeyID, int inSlotID)
    {
        this.outKeyID = outKeyID;
        this.outSlotID = outSlotID;
        this.inKeyID = inKeyID;
        this.inSlotID = inSlotID;
    }
}

public class FacilityDeliverRequest
{
    public int foodTypeID;
    public int tarFacilityKeyID;
    public int tarFacilitySlotID;

    public FacilityDeliverRequest(int foodTypeID,int tarFacilityKeyID,int tarFacilitySlotID)
    {
        this.foodTypeID = foodTypeID;
        this.tarFacilityKeyID = tarFacilityKeyID;
        this.tarFacilitySlotID = tarFacilitySlotID;
    }

}