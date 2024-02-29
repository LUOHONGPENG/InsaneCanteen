using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLineItem : MonoBehaviour
{
    public LineRenderer line;
    private MapSlotItem outSlot;
    private MapSlotItem inSlot;

    public void Init(MapSlotItem outSlot, MapSlotItem inSlot)
    {
        this.outSlot = outSlot;
        this.inSlot = inSlot;
    }

    private void CreateLine()
    {

    }

    private void UpdateLine()
    {

    }

}
