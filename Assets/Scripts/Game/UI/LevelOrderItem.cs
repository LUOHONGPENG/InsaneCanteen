using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOrderItem : MonoBehaviour
{
    public Image imgFood;
    public Text txNum;


    public void Init(int typeID, int Num)
    {
        FoodExcelItem foodItem = PublicTool.GetFoodItem(typeID);
        imgFood.sprite = Resources.Load("Sprite/Food/" + foodItem.iconUrl, typeof(Sprite)) as Sprite;

        txNum.text = Num.ToString();
    }


}
