using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    [Header("Tile")]
    public Transform tfTile;
    public GameObject pfTile;
    [Header("Facility")]
    public Transform tfFacility;
    public GameObject pfFacility;
    public Dictionary<int, MapFacilityItem> dicFacility = new Dictionary<int, MapFacilityItem>();
    [Header("Line")]
    public Transform tfLine;
    public GameObject pfLine;

    private SceneGameData sceneGameData;

    public void Init()
    {
        sceneGameData = PublicTool.GetSceneGameData();

        InitMapBG();
        InitFacility();
        UpdateLine();
    }

    /// <summary>
    /// 初始化的时候建立地图版
    /// </summary>
    public void InitMapBG()
    {
        PublicTool.ClearChildItem(tfTile);
        for (int i = 0;i < GameGlobal.mapSizeX; i++)
        {
            for (int j = 0; j < GameGlobal.mapSizeY; j++)
            {
                Vector2Int thisPos = new Vector2Int(i, j);
                GameObject objTile = GameObject.Instantiate(pfTile, tfTile);
                MapTileItem itemTile = objTile.GetComponent<MapTileItem>();
                itemTile.Init(thisPos);
            }
        }
    }

    #region Facility 设施相关
    /// <summary>
    /// 初始化设施，比如在里面塞默认的盘子
    /// </summary>
    public void InitFacility()
    {
        PublicTool.ClearChildItem(tfFacility);
        dicFacility.Clear();

        for (int i = 0; i < sceneGameData.listFacility.Count; i++)
        {
            FacilitySetData facilityData = sceneGameData.listFacility[i];

            AddFacility(facilityData);
        }
    }

    public void AddFacility(FacilitySetData facilityData)
    {
        GameObject objFacility = GameObject.Instantiate(pfFacility, tfFacility);
        MapFacilityItem itemFacility = objFacility.GetComponent<MapFacilityItem>();
        itemFacility.Init(facilityData);
        dicFacility.Add(facilityData.keyID, itemFacility);
    }

    public void RemoveFacility(int keyID)
    {
        if (dicFacility.ContainsKey(keyID))
        {
            MapFacilityItem itemFacility = dicFacility[keyID];
            dicFacility.Remove(keyID);
            Destroy(itemFacility.gameObject);
        }
    }
    #endregion

    #region Line 连线相关

    public void UpdateLine()
    {
        PublicTool.ClearChildItem(tfLine);

        for(int i = 0; i < sceneGameData.listFacility.Count; i++)
        {
            FacilitySetData thisFacility = sceneGameData.listFacility[i];
            for(int j = 0; j < thisFacility.listSlotOut.Count; j++)
            {
                //检测是否有连接
                if (thisFacility.listSlotOut[j].x >= 0)
                {
                    Vector2Int linkInfo = thisFacility.listSlotOut[j];
                    GameObject objLine = GameObject.Instantiate(pfLine, tfLine);
                    MapLineItem itemLine = objLine.GetComponent<MapLineItem>();
                    if(dicFacility.ContainsKey(thisFacility.keyID) && dicFacility.ContainsKey(linkInfo.x))
                    {
                        MapSlotItem outSlot = dicFacility[thisFacility.keyID].listSlotOut[j];
                        MapSlotItem inSlot = dicFacility[linkInfo.x].listSlotIn[linkInfo.y];
                        itemLine.Init(outSlot, inSlot);
                    }
                }
            }
        }

    }
    #endregion
}
