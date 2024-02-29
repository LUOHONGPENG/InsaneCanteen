using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public Transform tfTile;
    public GameObject pfTile;

    public void Init()
    {
        InitMapBG();
    }

    public void InitMapBG()
    {
        for(int i = 0;i < GameGlobal.mapSizeX; i++)
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
}
