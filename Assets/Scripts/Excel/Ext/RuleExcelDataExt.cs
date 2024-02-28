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
        //����
        RuleSortHighestPriority(ruleItems);

        //Import rule
        //�������
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

    //����������ȼ�����
    public void RuleSortHighestPriority(List<RuleExcelItem> list)
    {
        list.Sort((y, x) => { return x.priority.CompareTo(y.priority); });
    }

    #region Rule Container
    //The container for the rules of the conbination of food
    //��ʼ��֮���ھ��ڷ������Ƿ������䷽����������
    public class RuleContainer
    {
        public Dictionary<int, int> dicNeedFood = new Dictionary<int, int>();

        //Import the array of needed foodID and the number
        //������Ҫ�����ʳ��ID������������/�б�
        public RuleContainer(List<int> need_foodID,List<int> need_foodNum)
        {
            dicNeedFood.Clear();
            //Check whether the lists are valid again
            //�ٴμ�����������Ƿ����
            if (need_foodID.Count > 0 && need_foodNum.Count > 0 && need_foodID.Count == need_foodNum.Count)
            {
                for(int i = 0; i < need_foodID.Count; i++)
                {
                    int foodID = need_foodID[i];
                    int foodNum = need_foodNum[i];
                    //Check whether the data is bigger than 0 and whether the foodID exist in this rule
                    //��������Ƿ�������Ҹ�ʳ���Ѵ�������������
                    if (foodID > 0 && foodNum > 0 && !dicNeedFood.ContainsKey(foodID))
                    {
                        dicNeedFood.Add(foodID, foodNum);
                    }
                }
            }
        }

        //��鵱ǰ��״���Ƿ���ϸ�������
        public bool CheckMatchThisRule(List<Vector2Int> existFood)
        {
            //��ǰ���ϵ�ʳ����������
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
                        //��һ��ʳ�Ĳ���������ֱ��Pass
                        return false;
                    }
                }
            }

            //���������һ�������ʳ������
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

