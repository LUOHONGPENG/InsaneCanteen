using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSlotItem : MonoBehaviour
{
    [HideInInspector]
    public SlotType slotType;
    [HideInInspector]
    public int FacilityKeyID = -1;
    [HideInInspector]
    public int slotID = -1;

    public virtual void Init(SlotType slotType,int slotID,int maxID,int keyID)
    {
        this.slotType = slotType;
        this.slotID = slotID;
        this.FacilityKeyID = keyID;
        SetPosition(maxID);
    }

    public void SetPosition(int maxID)
    {
        //也可以用Layout排版，不过这个比较稳妥
        float midID = (maxID - 1) * 0.5f;
        this.transform.localPosition = new Vector3((slotID-midID) * GameGlobal.mapSlotSpacingX, 0, 1F);
    }
}
