using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneGameMgr
{

    #region Level关卡相关
    /// <summary>
    /// 下一关
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
    /// 新一关重置
    /// </summary>
    private void NewLevel()
    {
        LevelExcelItem levelItem = PublicTool.GetLevelItem(sceneGameData.curLevelID);
        //重置交互
        InputMgr.Instance.SetState(InputState.Build);
        //重置数据
        sceneGameData.NewLevelFacility();
        sceneGameData.NewLevelOrder(new Vector2Int(levelItem.needFoodType, levelItem.needFoodNum));
        //重置地图
        mapMgr.InitFacility();
        mapMgr.UpdateLine();
        mapMgr.ClearFood();
        //重置UI,主要是提供的设施会变
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

    //停止烹饪时候需要重置不少东西
    public void StopCook(object arg0)
    {
        isCooking = false;
        //重置数据
        sceneGameData.StopCookFacility();
        //重置地图，主要是要把食物清除掉吧
        mapMgr.ClearFood();
        //重置UI，主要是清除食物
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
                    //源头主要是用来产生Request,不需要检查自身库存（也没有）
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
    /// 检查源头型设施
    /// </summary>
    /// <param name="facility"></param>
    private void CookCheckSourceFacility(FacilitySetData facility)
    {
        if (facility.deliverRequest == null)
        {
            //检查是否有连接物
            if (facility.listSlotOut.Count > 0 && facility.listSlotOut[0].x >= 0)
            {
                //有连接则创造Request
                Vector2Int slotLink = facility.listSlotOut[0];
                facility.CreateDeliverRequest(facility.GetExcelItem().sourceFoodID, slotLink.x, slotLink.y);
                //表现层-创造出一个沿着line飞过去目标Slot的Dotween动画，维持1秒即可
                mapMgr.CreateFood(facility.GetExcelItem().sourceFoodID, new Vector2Int(facility.keyID, 0), slotLink);
            }
        }
        else
        {
            if (facility.deliverRequest.CheckDeliver())
            {
                //送出食物
                SendFoodToFacility(facility.deliverRequest);
                //结束Request
                facility.EndDeliverRequest();
            }
            else
            {
                //Request时间流逝 0.2秒
                facility.deliverRequest.DeliverTimeGo();
            }
        }
    }

    /// <summary>
    /// 检查混合设施
    /// </summary>
    /// <param name="facility"></param>
    private void CookCheckMixerFacility(FacilitySetData facility)
    {
        if (facility.deliverRequest == null)
        {
            //首先检查有没有下级连接物，没有就不用管下面的部分了
            if (facility.listSlotOut.Count > 0 && facility.listSlotOut[0].x >= 0)
            {
                //进入的连接数量
                int linkNum = 0;
                //有食物的 进入的连接数量
                int linkHaveFoodNum = 0;
                //先检查库存是否满足出货条件
                for (int i = 0; i < facility.listSlotStore.Count; i++)
                {
                    //确保是有连接的 否则不用理这格库存
                    if (facility.CheckWhetherInSlotHaveLink(i))
                    {
                        linkNum++;
                        if (facility.listSlotStore[i].Count > 0)
                        {
                            linkHaveFoodNum++;
                        }
                    }
                }

                //满足出货数量
                if (linkHaveFoodNum >= linkNum)
                {
                    //先建个字典存一下
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

                    //字典转化
                    List<Vector2Int> listForBlend = new List<Vector2Int>();
                    foreach (var Pair in dicFoodNum)
                    {
                        listForBlend.Add(new Vector2Int(Pair.Key, Pair.Value));
                    }

                    //搜索混合结果
                    int resultID = PublicTool.SearchBlendResult(listForBlend, facility.typeID);
                    if (resultID < 0)
                    {
                        //合成焦炭
                        resultID = 9999;
                    }

                    //创建订单
                    Vector2Int slotLink = facility.listSlotOut[0];
                    facility.CreateDeliverRequest(resultID, slotLink.x, slotLink.y);
                    //表现层-创造出一个沿着line飞过去目标Slot的Dotween动画，维持1秒即可
                    mapMgr.CreateFood(resultID, new Vector2Int(facility.keyID, 0), slotLink);
                }
            }
        }
        else
        {
            if (facility.deliverRequest.CheckDeliver())
            {
                //送出食物
                SendFoodToFacility(facility.deliverRequest);
                //结束Request
                facility.EndDeliverRequest();
            }
            else
            {
                //Request时间流逝 0.2秒
                facility.deliverRequest.DeliverTimeGo();
            }
        }
    }

    private void CookCheckEndFacility(FacilitySetData facility)
    {
        //直接检查库存
        for (int i = 0; i < facility.listSlotStore.Count; i++)
        {
            //确保是有连接的 否则不用理这格库存
            if (facility.CheckWhetherInSlotHaveLink(i))
            {
                //有库存
                if (facility.listSlotStore[i].Count > 0)
                {
                    //直接吃掉
                    int foodID = facility.listSlotStore[i].Dequeue();
                    sceneGameData.EatFood(foodID);
                    EventCenter.Instance.EventTrigger("UpdateFood", foodID);

                    Debug.Log("EatFood " + foodID);
                    //刷新食物
                    if (sceneGameData.CheckWhetherMeetOrder())
                    {
                        //停止烹饪
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
    /// Request完成后 目标库存塞入食物
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
