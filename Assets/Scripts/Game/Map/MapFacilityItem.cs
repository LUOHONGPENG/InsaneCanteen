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
        //����ͼƬ
        spFacility.sprite = Resources.Load("Sprite/Facility/" + thisData.GetExcelItem().iconUrl, typeof(Sprite)) as Sprite;
        //SetCollider
        //������ײ��
        if (polyCol != null)
        {
            Destroy(polyCol);
        }
        polyCol = spFacility.gameObject.AddComponent<PolygonCollider2D>();
        //��ʼ��λ��
        SetSelfPos();
        //��ʼ���׶�
        InitSlot();
    }

    /// <summary>
    /// ����λ��
    /// </summary>
    public void SetSelfPos()
    {
        //����PosIDֱ���λ��
        Vector3 basicPos = PublicTool.ConvertPosFromID(thisData.posID);
        //����ռ�����������λ��Ӧ����Щƫ��
        Vector3 delta = PublicTool.CalculateFacilityModelDelta(thisData.sizeX, thisData.sizeY);
        
        //ʹ��BoxCol����Ӧ�Ļ�
        //boxCol.size = new Vector2(thisData.sizeX * GameGlobal.mapTileSize, thisData.sizeY * GameGlobal.mapTileSize);

        this.transform.localPosition = basicPos + delta;
    }

    public void InitSlot()
    {
        //���ֲ�-�����ȥ�Ŀ�
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

        //���ֲ�-�������Ŀ�
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

        //�������׶��еĻ� �����Ǵ�
        if (thisData.listSlotOut.Count > 0 && thisData.listSlotIn.Count > 0)
        {
            tfSlotOut.transform.localPosition = new Vector3(tfSlotOut.transform.localPosition.x, GameGlobal.mapSlotSpacingY, tfSlotOut.transform.localPosition.z);
            tfSlotIn.transform.localPosition = new Vector3(tfSlotIn.transform.localPosition.x, -GameGlobal.mapSlotSpacingY, tfSlotIn.transform.localPosition.z);
        }

    }

}
