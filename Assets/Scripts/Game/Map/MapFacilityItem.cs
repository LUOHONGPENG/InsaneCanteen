using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFacilityItem : MonoBehaviour
{
    public SpriteRenderer spFacility;

    private FacilitySetData thisData;

    public void Init(FacilitySetData facilityData)
    {
        thisData = facilityData;

        spFacility.sprite = Resources.Load("Sprite/Facility/" + thisData.GetExcelItem().iconUrl, typeof(Sprite)) as Sprite;
        SetSelfPos();
    }

    public void SetSelfPos()
    {
        Vector3 basicPos = PublicTool.ConvertPosFromID(thisData.posID);
        Vector3 delta = PublicTool.CalculateFacilityModelDelta(thisData.sizeX, thisData.sizeY);

        this.transform.localPosition = basicPos + delta;
    }


    public FacilitySetData GetData()
    {
        return thisData;
    }
}
