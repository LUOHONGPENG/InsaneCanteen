using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFacilityItem : MonoBehaviour
{
    public SpriteRenderer spFacility;
    //public BoxCollider2D boxCol;
    public PolygonCollider2D polyCol;

    private FacilitySetData thisData;


    public void Init(FacilitySetData facilityData)
    {
        thisData = facilityData;
        //SetSprite
        spFacility.sprite = Resources.Load("Sprite/Facility/" + thisData.GetExcelItem().iconUrl, typeof(Sprite)) as Sprite;
        //SetCollider
        if (polyCol != null)
        {
            Destroy(polyCol);
        }
        polyCol = spFacility.gameObject.AddComponent<PolygonCollider2D>();
        SetSelfPos();
    }

    public void SetSelfPos()
    {
        Vector3 basicPos = PublicTool.ConvertPosFromID(thisData.posID);
        Vector3 delta = PublicTool.CalculateFacilityModelDelta(thisData.sizeX, thisData.sizeY);
        
        //使用BoxCol自适应的话
        //boxCol.size = new Vector2(thisData.sizeX * GameGlobal.mapTileSize, thisData.sizeY * GameGlobal.mapTileSize);

        this.transform.localPosition = basicPos + delta;
    }


    public FacilitySetData GetData()
    {
        return thisData;
    }
}
