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