using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此处用于装载Event使用的Info结构体
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