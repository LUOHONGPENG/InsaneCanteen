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

        FacilityExcelItem excelItem = PublicTool.GetFacilityItem(thisData.typeID);
        spFacility.sprite = Resources.Load("Sprite/Facility/" + excelItem.iconUrl, typeof(Sprite)) as Sprite;
        SetSelfPos();
    }

    public void SetSelfPos()
    {
        this.transform.localPosition = PublicTool.ConvertPosFromID(thisData.posID);
    }

    public FacilitySetData GetData()
    {
        return thisData;
    }
}
