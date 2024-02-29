using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileItem : MonoBehaviour
{
    private Vector2Int thisPosID;

    public void Init(Vector2Int posID)
    {
        thisPosID = posID;
        SetSelfPos();
    }

    public void SetSelfPos()
    {
        this.transform.localPosition = PublicTool.ConvertPosFromID(thisPosID) ;
    }
}
