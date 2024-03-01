using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class RuleExcelData
{
    public Dictionary<int, List<RuleContainer>> dicMixerRule = new Dictionary<int, List<RuleContainer>>();

    public void Init()
    {
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
                RuleContainer rule = new RuleContainer(tempItem.array_foodID, tempItem.array_foodNum,tempItem.resultID);

                if (dicMixerRule.ContainsKey(tempItem.mixerID))
                {
                    dicMixerRule[tempItem.mixerID].Add(rule);
                }
                else
                {
                    List<RuleContainer> listRule = new List<RuleContainer>();
                    listRule.Add(rule);
                    dicMixerRule.Add(tempItem.mixerID, listRule);
                }
            }
        }
    }

    //����������ȼ�����
    public void RuleSortHighestPriority(List<RuleExcelItem> list)
    {
        list.Sort((y, x) => { return x.priority.CompareTo(y.priority); });
    }

    //����������Ͻ��
    public int SearchBlendResult(List<Vector2Int> existFood,int mixerID)
    {
        if (dicMixerRule.ContainsKey(mixerID))
        {
            List<RuleContainer> listRule = dicMixerRule[mixerID];
            for (int i = 0; i < listRule.Count; i++)
            {
                RuleContainer tempRule = listRule[i];
                if (tempRule.CheckMatchThisRule(existFood))
                {
                    return tempRule.resultID;
                }
            }
        }
        return -1;
    }

    #region Rule Container
    //The container for the rules of the conbination of food
    //��ʼ��֮���ھ��ڷ������Ƿ������䷽����������
    public class RuleContainer
    {
        public Dictionary<int, int> dicNeedFood = new Dictionary<int, int>();
        public int resultID = -1;

        //Import the array of needed foodID and the number
        //������Ҫ�����ʳ��ID������������/�б�
        public RuleContainer(List<int> need_foodID,List<int> need_foodNum,int resultID)
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
                //¼����ID
                this.resultID = resultID;
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

