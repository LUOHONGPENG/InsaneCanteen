using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneGameData
{
    //当前关卡ID
    public int curLevelID = -1;
    //当前订单
    public Vector2Int curOrder = new Vector2Int(-1, -1);
    //当前菜品
    public Dictionary<int, int> dicCurFood = new Dictionary<int, int>();

    public void Init()
    {
        InitFacility();
    }

    #region 菜品和订单
    /// <summary>
    /// 新关卡设置订单
    /// </summary>
    /// <param name="order"></param>
    public void NewLevelOrder(Vector2Int order)
    {
        curOrder = order;
        ClearFood();
    }

    /// <summary>
    /// 新关卡和暂停烹饪时需要调用
    /// </summary>
    public void ClearFood()
    {
        dicCurFood.Clear();
    }

    public void EatFood(int foodID)
    {
        if (dicCurFood.ContainsKey(foodID))
        {
            dicCurFood[foodID]++;
        }
        else
        {
            dicCurFood.Add(foodID, 1);
        }
    }


    /// <summary>
    /// 检查当前菜品是否满足过关条件
    /// </summary>
    /// <returns></returns>
    public bool CheckWhetherMeetOrder()
    {
        if (dicCurFood.ContainsKey(curOrder.x))
        {
            if (dicCurFood[curOrder.x] >= curOrder.y)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
