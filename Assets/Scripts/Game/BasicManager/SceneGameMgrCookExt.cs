using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneGameMgr
{

    #region Level�ؿ����
    /// <summary>
    /// ��һ��
    /// </summary>
    private void NextLevelEvent(object arg0)
    {
        if (sceneGameData.curLevelID < PublicTool.GetMaxLevel())
        {
            sceneGameData.curLevelID++;
            NewLevel();
        }
    }

    /// <summary>
    /// ��һ������
    /// </summary>
    private void NewLevel()
    {
        LevelExcelItem levelItem = PublicTool.GetLevelItem(sceneGameData.curLevelID);
        //���ý���
        InputMgr.Instance.SetState(InputState.Build);
        //��������
        sceneGameData.NewLevelFacility();
        sceneGameData.NewLevelOrder(new Vector2Int(levelItem.needFoodType, levelItem.needFoodNum));
        //���õ�ͼ
        mapMgr.InitFacility();
        mapMgr.UpdateLine();
        mapMgr.ClearFood();
        //����UI,��Ҫ���ṩ����ʩ���
        uiMgr.NewLevelSetFacility(levelItem.readyFacility);
        uiMgr.NewLevelUpdateOrder();

        //Cook
        isCooking = false;
    }

    #endregion

    #region Cook
    public bool isCooking = false;
    public float timerCook = GameGlobal.cookCheckTime;

    public void StartCook(object arg0)
    {
        isCooking = true;
        timerCook = GameGlobal.cookCheckTime;
    }

    //ֹͣ���ʱ����Ҫ���ò��ٶ���
    public void StopCook(object arg0)
    {
        isCooking = false;
        //��������
        sceneGameData.StopCookFacility();
        //���õ�ͼ����Ҫ��Ҫ��ʳ���������
        mapMgr.ClearFood();
        //����UI����Ҫ�����ʳ��
        uiMgr.StopCookUpdateFood();
    }

    public void FixedUpdate()
    {
        if (isCooking)
        {
            timerCook -= Time.deltaTime;
            if (timerCook < 0)
            {
                CookCheckFacility();
                timerCook = GameGlobal.cookCheckTime;
            }
        }
    }



    private void CookCheckFacility()
    {
        for(int i = 0;i < sceneGameData.listFacility.Count; i++)
        {
            FacilitySetData facility = sceneGameData.listFacility[i];
            switch (facility.facilityType)
            {
                case FacilityType.Source:
                    //Դͷ��Ҫ����������Request,����Ҫ��������棨Ҳû�У�
                    CookCheckSourceFacility(facility);
                    break;
                case FacilityType.Mixer:
                    CookCheckMixerFacility(facility);
                    break;
                case FacilityType.End:
                    CookCheckEndFacility(facility);
                    break;
            }


        }
    }

    /// <summary>
    /// ���Դͷ����ʩ
    /// </summary>
    /// <param name="facility"></param>
    private void CookCheckSourceFacility(FacilitySetData facility)
    {
        if (facility.deliverRequest == null)
        {
            //����Ƿ���������
            if (facility.listSlotOut.Count > 0 && facility.listSlotOut[0].x >= 0)
            {
                //����������Request
                Vector2Int slotLink = facility.listSlotOut[0];
                facility.CreateDeliverRequest(facility.GetExcelItem().sourceFoodID, slotLink.x, slotLink.y);
                //���ֲ�-�����һ������line�ɹ�ȥĿ��Slot��Dotween������ά��1�뼴��
                mapMgr.CreateFood(facility.GetExcelItem().sourceFoodID, new Vector2Int(facility.keyID, 0), slotLink);
            }
        }
        else
        {
            if (facility.deliverRequest.CheckDeliver())
            {
                //�ͳ�ʳ��
                SendFoodToFacility(facility.deliverRequest);
                //����Request
                facility.EndDeliverRequest();
            }
            else
            {
                //Requestʱ������ 0.2��
                facility.deliverRequest.DeliverTimeGo();
            }
        }
    }

    /// <summary>
    /// �������ʩ
    /// </summary>
    /// <param name="facility"></param>
    private void CookCheckMixerFacility(FacilitySetData facility)
    {
        if (facility.deliverRequest == null)
        {
            //���ȼ����û���¼������û�оͲ��ù�����Ĳ�����
            if (facility.listSlotOut.Count > 0 && facility.listSlotOut[0].x >= 0)
            {
                //�������������
                int linkNum = 0;
                //��ʳ��� �������������
                int linkHaveFoodNum = 0;
                //�ȼ�����Ƿ������������
                for (int i = 0; i < facility.listSlotStore.Count; i++)
                {
                    //ȷ���������ӵ� �������������
                    if (facility.CheckWhetherInSlotHaveLink(i))
                    {
                        linkNum++;
                        if (facility.listSlotStore[i].Count > 0)
                        {
                            linkHaveFoodNum++;
                        }
                    }
                }

                //�����������
                if (linkHaveFoodNum >= linkNum)
                {
                    //�Ƚ����ֵ��һ��
                    Dictionary<int, int> dicFoodNum = new Dictionary<int, int>();
                    for (int i = 0; i < facility.listSlotStore.Count; i++)
                    {
                        if (facility.CheckWhetherInSlotHaveLink(i))
                        {
                            int foodID = facility.listSlotStore[i].Dequeue();
                            if (dicFoodNum.ContainsKey(foodID))
                            {
                                dicFoodNum[foodID]++;
                            }
                            else
                            {
                                dicFoodNum.Add(foodID, 1);
                            }
                        }
                    }

                    //�ֵ�ת��
                    List<Vector2Int> listForBlend = new List<Vector2Int>();
                    foreach (var Pair in dicFoodNum)
                    {
                        listForBlend.Add(new Vector2Int(Pair.Key, Pair.Value));
                    }

                    //������Ͻ��
                    int resultID = PublicTool.SearchBlendResult(listForBlend, facility.typeID);
                    if (resultID < 0)
                    {
                        //�ϳɽ�̿
                        resultID = 9999;
                    }

                    //��������
                    Vector2Int slotLink = facility.listSlotOut[0];
                    facility.CreateDeliverRequest(resultID, slotLink.x, slotLink.y);
                    //���ֲ�-�����һ������line�ɹ�ȥĿ��Slot��Dotween������ά��1�뼴��
                    mapMgr.CreateFood(resultID, new Vector2Int(facility.keyID, 0), slotLink);
                }
            }
        }
        else
        {
            if (facility.deliverRequest.CheckDeliver())
            {
                //�ͳ�ʳ��
                SendFoodToFacility(facility.deliverRequest);
                //����Request
                facility.EndDeliverRequest();
            }
            else
            {
                //Requestʱ������ 0.2��
                facility.deliverRequest.DeliverTimeGo();
            }
        }
    }

    private void CookCheckEndFacility(FacilitySetData facility)
    {
        //ֱ�Ӽ����
        for (int i = 0; i < facility.listSlotStore.Count; i++)
        {
            //ȷ���������ӵ� �������������
            if (facility.CheckWhetherInSlotHaveLink(i))
            {
                //�п��
                if (facility.listSlotStore[i].Count > 0)
                {
                    //ֱ�ӳԵ�
                    int foodID = facility.listSlotStore[i].Dequeue();
                    sceneGameData.EatFood(foodID);
                    EventCenter.Instance.EventTrigger("UpdateFood", foodID);

                    Debug.Log("EatFood " + foodID);
                    //ˢ��ʳ��
                    if (sceneGameData.CheckWhetherMeetOrder())
                    {
                        //ֹͣ���
                        isCooking = false;
                        if(sceneGameData.curLevelID < PublicTool.GetMaxLevel())
                        {
                            EventCenter.Instance.EventTrigger("NextLevelUI", false);
                        }
                        else
                        {
                            EventCenter.Instance.EventTrigger("NextLevelUI", true);
                        }
                    }
                }
            }
        }
    }





    /// <summary>
    /// Request��ɺ� Ŀ��������ʳ��
    /// </summary>
    /// <param name="request"></param>
    public void SendFoodToFacility(FacilityDeliverRequest request)
    {
        if (sceneGameData.dicFacility.ContainsKey(request.tarFacilityKeyID))
        {
            sceneGameData.dicFacility[request.tarFacilityKeyID].ReceiveFood(request.foodTypeID, request.tarFacilitySlotID);
        }
    }
    #endregion
}
