using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneGameData
{
    //��ǰ�ؿ�ID
    public int curLevelID = -1;
    //��ǰ����
    public Vector2Int curOrder = new Vector2Int(-1, -1);
    //��ǰ��Ʒ
    public Dictionary<int, int> dicCurFood = new Dictionary<int, int>();

    public void Init()
    {
        InitFacility();
    }

    #region ��Ʒ�Ͷ���
    /// <summary>
    /// �¹ؿ����ö���
    /// </summary>
    /// <param name="order"></param>
    public void NewLevelOrder(Vector2Int order)
    {
        curOrder = order;
        ClearFood();
    }

    /// <summary>
    /// �¹ؿ�����ͣ���ʱ��Ҫ����
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
    /// ��鵱ǰ��Ʒ�Ƿ������������
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
