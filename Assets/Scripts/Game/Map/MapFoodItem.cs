using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFoodItem : MonoBehaviour
{
    public Transform tfThis;

    public SpriteRenderer spFood;

    public void Init(int typeID)
    {
        FoodExcelItem item = PublicTool.GetFoodItem(typeID);
        spFood.sprite = Resources.Load("Sprite/Food/" + item.iconUrl, typeof(Sprite)) as Sprite;
        
    }

    public void InitAni(Vector2 start,Vector2 end)
    {

        StartCoroutine(IE_Ani());

    }

    public IEnumerator IE_Ani()
    {
        yield return new WaitForSeconds(GameGlobal.deliverTime+0.1f);
        Destroy(this.gameObject);
    }
}
