using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFacilityItem : MonoBehaviour
{
    [Header("Basic")]
    public SpriteRenderer spFacility;
    //public BoxCollider2D boxCol;
    public PolygonCollider2D polyCol;

    [Header("Slot")]
    public Transform tfSlotOut;
    public GameObject pfSlotOut;
    public Transform tfSlotIn;
    public GameObject pfSlotIn;


    private FacilitySetData thisData;

    public void Init(FacilitySetData facilityData)
    {
        thisData = facilityData;
        //SetSprite
        //����ͼƬ
        spFacility.sprite = Resources.Load("Sprite/Facility/" + thisData.GetExcelItem().iconUrl, typeof(Sprite)) as Sprite;
        //SetCollider
        //������ײ��
        if (polyCol != null)
        {
            Destroy(polyCol);
        }
        polyCol = spFacility.gameObject.AddComponent<PolygonCollider2D>();
        //����λ��
        SetSelfPos();
    }

    public void SetSelfPos()
    {
        Vector3 basicPos = PublicTool.ConvertPosFromID(thisData.posID);
        Vector3 delta = PublicTool.CalculateFacilityModelDelta(thisData.sizeX, thisData.sizeY);
        
        //ʹ��BoxCol����Ӧ�Ļ�
        //boxCol.size = new Vector2(thisData.sizeX * GameGlobal.mapTileSize, thisData.sizeY * GameGlobal.mapTileSize);

        this.transform.localPosition = basicPos + delta;
    }


    public FacilitySetData GetData()
    {
        return thisData;
    }
}
