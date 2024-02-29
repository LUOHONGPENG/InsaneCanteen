using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameGlobal
{
    #region Map

    public static int mapSizeX = 18;
    public static int mapSizeY = 18;
    public static float mapTileSize = 0.5f;
    public static Vector3 mapDelta = new Vector3(-3, -0.25f, 0);

    #endregion

    public static int[] testFacilityID = { 1001,2001 };
}
