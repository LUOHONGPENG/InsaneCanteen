using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameGlobal
{
    //语言
    public static LanguageType languageType = LanguageType.CN;
    #region Map
    //地图大小
    public static int mapSizeX = 18;
    public static int mapSizeY = 18;
    public static float mapTileSize = 0.5f;
    public static Vector3 mapDelta = new Vector3(-2.5f, -0.25f, 0);
    //在一个设施内 孔洞的间距
    public static float mapSlotSpacingX = 0.3f;
    public static float mapSlotSpacingY = 0.3f;
    #endregion

    public static int[] testFacilityID = { 1001,2001 };
}
