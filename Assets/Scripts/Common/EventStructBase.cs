using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �˴�����װ��Eventʹ�õ�Info�ṹ��
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