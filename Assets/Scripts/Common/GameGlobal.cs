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
    public static Vector3 mapDelta = new Vector3(-3f, -0.25f, 0);
    //��һ����ʩ�� �׶��ļ��
    public static float mapSlotSpacingX = 0.3f;
    public static float mapSlotSpacingY = 0.3f;
    //Cook
    public static float deliverTime = 1f;
    public static float cookCheckTime = 0.2f;
    #endregion

}
