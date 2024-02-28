using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class RuleExcelData
{
    public List<RuleContainer> listRule = new List<RuleContainer>();

    public void Init()
    {
        listRule.Clear();

        List<RuleExcelItem> ruleItems = new List<RuleExcelItem>(items);
        //排序
        RuleSortHighestPriority(ruleItems);

        //Import rule
        //导入规则
        for (int i = 0; i < ruleItems.Count; i++)
        {
            RuleExcelItem tempItem = ruleItems[i];
            if(tempItem.array_foodID.Count > 0 && tempItem.array_foodNum.Count > 0 && tempItem.array_foodID.Count == tempItem.array_foodNum.Count)
            {
                RuleContainer rule = new RuleContainer(tempItem.array_foodID, tempItem.array_foodNum);
                listRule.Add(rule);
                Debug.Log(ruleItems[i].id);
            }
        }
    }

    //规则根据优先级排序
    public void RuleSortHighestPriority(List<RuleExcelItem> list)
    {
        list.Sort((y, x) => { return x.priority.CompareTo(y.priority); });
    }

    #region Rule Container
    //The container for the rules of the conbination of food
    //初始化之后在局内方便检测是否满足配方条件的容器
    public class RuleContainer
    {
        public Dictionary<int, int> dicNeedFood = new Dictionary<int, int>();

        //Import the array of needed foodID and the number
        //导入需要满足的食材ID及数量的数组/列表
        public RuleContainer(List<int> need_foodID,List<int> need_foodNum)
        {
            dicNeedFood.Clear();
            //Check whether the lists are valid again
            //再次检查代入的数组是否合理
            if (need_foodID.Count > 0 && need_foodNum.Count > 0 && need_foodID.Count == need_foodNum.Count)
            {
                for(int i = 0; i < need_foodID.Count; i++)
                {
                    int foodID = need_foodID[i];
                    int foodNum = need_foodNum[i];
                    //Check whether the data is bigger than 0 and whether the foodID exist in this rule
                    //检查数据是否大于零且该食材已存在于这条规则
                    if (foodID > 0 && foodNum > 0 && !dicNeedFood.ContainsKey(foodID))
                    {
                        dicNeedFood.Add(foodID, foodNum);
                    }
                }
            }
        }

        //检查当前的状况是否符合该条规则
        public bool CheckMatchThisRule(List<Vector2Int> existFood)
        {
            //当前符合的食物种类数量
            int matchTypeNum = 0;

            for(int i = 0; i < existFood.Count; i++)
            {
                Vector2Int tempFood = existFood[i];
                if (dicNeedFood.ContainsKey(tempFood.x))
                {
                    if (tempFood.y >= dicNeedFood[tempFood.x])
                    {
                        matchTypeNum++;
                    }
                    else
                    {
                        //有一种食材不满足数量直接Pass
                        return false;
                    }
                }
            }

            //检查满足这一条规则的食材种类
            if(matchTypeNum >= dicNeedFood.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

}

