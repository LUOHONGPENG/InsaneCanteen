using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameGlobal
{
    //����
    public static LanguageType languageType = LanguageType.CN;
    #region Map
    //��ͼ��С
    public static int mapSizeX = 18;
    public static int mapSizeY = 18;
    public static float mapTileSize = 0.5f;
    public static Vector3 mapDelta = new Vector3(-2.5f, -0.25f, 0);
    //��һ����ʩ�� �׶��ļ��
    public static float mapSlotSpacingX = 0.3f;
    public static float mapSlotSpacingY = 0.3f;
    #endregion

    public static int[] testFacilityID = { 1001,2001 };
}
