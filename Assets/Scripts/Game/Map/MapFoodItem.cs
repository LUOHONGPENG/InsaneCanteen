using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        tfThis.position = start;
        tfThis.localScale = Vector2.zero;
        StartCoroutine(IE_Ani(end));
        StartCoroutine(IE_Destory());

    }

    public IEnumerator IE_Ani(Vector2 end)
    {
        tfThis.DOMove(end, 1.1f);
        tfThis.DOScale(1.5f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        tfThis.DOScale(1f, 0.1f);
        yield return new WaitForSeconds(0.9f);
        tfThis.DOScale(0, 0.1f);
    }

    public IEnumerator IE_Destory()
    {
        yield return new WaitForSeconds(GameGlobal.deliverTime + 0.2f);
        Destroy(this.gameObject);
    }
}
