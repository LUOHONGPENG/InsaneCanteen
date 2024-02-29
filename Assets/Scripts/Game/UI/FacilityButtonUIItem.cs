using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityButtonUIItem : MonoBehaviour
{
    public Image imgFacility;

    private FacilityExcelItem facilityExcelItem;


    public void Init(int facilityID)
    {
        if (PublicTool.GetFacilityItem(facilityID) != null)
        {
            this.facilityExcelItem = PublicTool.GetFacilityItem(facilityID);
            imgFacility.sprite = Resources.Load("Sprite/Facility/" + facilityExcelItem.iconUrl, typeof(Sprite)) as Sprite;
            imgFacility.SetNativeSize();
        }
    }

    //获取该按钮设施的typeID
    public int GetFacilityID()
    {
        if (facilityExcelItem != null)
        {
            return facilityExcelItem.id;
        }
        return -1;
    }

}
