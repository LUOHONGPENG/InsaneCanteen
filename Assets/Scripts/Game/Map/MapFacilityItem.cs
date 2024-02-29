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
        Vector3 sizeDelta = new Vector3(0.5f * GameGlobal.mapTileSize * (thisData.sizeX-1),
            0.5f * GameGlobal.mapTileSize * (thisData.sizeY-1), 0);

        this.transform.localPosition = basicPos + sizeDelta;
    }

    public FacilitySetData GetData()
    {
        return thisData;
    }
}
