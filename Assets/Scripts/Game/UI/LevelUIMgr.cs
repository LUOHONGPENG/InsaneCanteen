using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIMgr : MonoBehaviour
{
    public Text txLevel;

    public Transform tfLevelOrder;
    public GameObject pfLevelOrder;

    public Transform tfLevelFood;
    public GameObject pfLevelFood;

    private SceneGameData sceneGameData;

    public void Init()
    {
        PublicTool.ClearChildItem(tfLevelOrder);
        PublicTool.ClearChildItem(tfLevelFood);

        sceneGameData = PublicTool.GetSceneGameData();
    }

    public void NewLevel()
    {
        txLevel.text = string.Format("{0} {1}", "tx_ui_order".ToLanguageText(), sceneGameData.curLevelID);

        PublicTool.ClearChildItem(tfLevelOrder);

        GameObject objOrder = GameObject.Instantiate(pfLevelOrder, tfLevelOrder);
        LevelOrderItem itemOrder = objOrder.GetComponent<LevelOrderItem>();
        itemOrder.Init(sceneGameData.curOrder.x, sceneGameData.curOrder.y);

        UpdateFood();
    }

    public void OnEnable()
    {
        EventCenter.Instance.AddEventListener("UpdateFood", UpdateFoodEvent);
    }

    private void UpdateFoodEvent(object arg0)
    {
        UpdateFood();
    }

    public void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("UpdateFood", UpdateFoodEvent);
    }

    public void UpdateFood()
    {
        //œ»‡≈±È¿˙
        PublicTool.ClearChildItem(tfLevelFood);
        foreach(var Pair in sceneGameData.dicCurFood)
        {
            GameObject objFood = GameObject.Instantiate(pfLevelFood, tfLevelFood);
            LevelOrderItem itemFood = objFood.GetComponent<LevelOrderItem>();
            itemFood.Init(Pair.Key, Pair.Value);
        }
    }

}
