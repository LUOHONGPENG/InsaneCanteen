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

    private SceneGameData sceneGameData;

    public void Init()
    {
        sceneGameData = PublicTool.GetSceneGameData();

        InitMapBG();
        InitFacility();

    }

    /// <summary>
    /// ��ʼ����ʱ������ͼ��
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

    /// <summary>
    /// ��ʼ����ʩ��������������Ĭ�ϵ�����
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
}
