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

    public FacilitySetData GetData()
    {
        return thisData;
    }

    public void Init(FacilitySetData facilityData)
    {
        thisData = facilityData;
        //SetSprite
        //设置图片
        spFacility.sprite = Resources.Load("Sprite/Facility/" + thisData.GetExcelItem().iconUrl, typeof(Sprite)) as Sprite;
        //SetCollider
        //设置碰撞体
        if (polyCol != null)
        {
            Destroy(polyCol);
        }
        polyCol = spFacility.gameObject.AddComponent<PolygonCollider2D>();
        //初始化位置
        SetSelfPos();
        //初始化孔洞
        InitSlot();
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    public void SetSelfPos()
    {
        //根据PosID直译的位置
        Vector3 basicPos = PublicTool.ConvertPosFromID(thisData.posID);
        //根据占地面积，中心位置应当有些偏差
        Vector3 delta = PublicTool.CalculateFacilityModelDelta(thisData.sizeX, thisData.sizeY);
        
        //使用BoxCol自适应的话
        //boxCol.size = new Vector2(thisData.sizeX * GameGlobal.mapTileSize, thisData.sizeY * GameGlobal.mapTileSize);

        this.transform.localPosition = basicPos + delta;
    }

    public void InitSlot()
    {
        //表现层-创造出去的孔
        PublicTool.ClearChildItem(tfSlotOut);
        if (thisData.listSlotOut.Count > 0)
        {
            int maxSlotOut = thisData.listSlotOut.Count;
            for(int i = 0; i < maxSlotOut; i++)
            {
                GameObject objSlot = GameObject.Instantiate(pfSlotOut, tfSlotOut);
                MapSlotItem itemSlot = objSlot.GetComponent<MapSlotItem>();
                itemSlot.Init(SlotType.Out, i, maxSlotOut, thisData.keyID);
            }
        }

        //表现层-创造进入的孔
        PublicTool.ClearChildItem(tfSlotIn);
        if(thisData.listSlotIn.Count > 0)
        {
            int maxSlotIn = thisData.listSlotIn.Count;
            for(int i = 0; i < maxSlotIn; i++)
            {
                GameObject objSlot = GameObject.Instantiate(pfSlotIn, tfSlotIn);
                MapSlotItem itemSlot = objSlot.GetComponent<MapSlotItem>();
                itemSlot.Init(SlotType.In, i, maxSlotIn, thisData.keyID);
            }
        }

        //如果出入孔都有的话 把他们错开
        if (thisData.listSlotOut.Count > 0 && thisData.listSlotIn.Count > 0)
        {
            tfSlotOut.transform.localPosition = new Vector3(tfSlotOut.transform.localPosition.x, GameGlobal.mapSlotSpacingY, tfSlotOut.transform.localPosition.z);
            tfSlotIn.transform.localPosition = new Vector3(tfSlotIn.transform.localPosition.x, -GameGlobal.mapSlotSpacingY, tfSlotIn.transform.localPosition.z);
        }

    }

}
