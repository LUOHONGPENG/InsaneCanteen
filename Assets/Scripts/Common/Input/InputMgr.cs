using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputMgr : MonoSingleton<InputMgr>
{
    private PlayerInput playerInput;
    private InputAction touchAction;
    private InputAction touchPositionAction;
    private InputAction deleteAction;
    private bool isInitInput = false;

    #region Init

    public IEnumerator IE_Init()
    {
        InitInput();
        yield break;
    }

    private void InitInput()
    {
        if (!isInitInput)
        {
            playerInput = new PlayerInput();
            touchAction = playerInput.Gameplay.Touch;
            touchPositionAction = playerInput.Gameplay.TouchPosition;
            deleteAction = playerInput.Gameplay.Delete;
            isInitInput = true;
        }
    }

    private void EnableInput()
    {
        if(playerInput == null)
        {
            InitInput();
        }

        playerInput.Enable();
        touchAction.performed += Touch_performed;
        touchAction.canceled += Touch_canceled;
        deleteAction.performed += Delete_performed;
    }



    private void DisableInput()
    {
        touchAction.performed -= Touch_performed;
        touchAction.canceled -= Touch_canceled;
        deleteAction.performed -= Delete_performed;

        if (playerInput != null)
        {
            playerInput.Disable();
        }
    }



    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }
    #endregion

    #region Bind
    //左键点击下去时候触发
    private void Touch_performed(InputAction.CallbackContext context)
    {
        InvokeCheckDrag();
    }

    //左键点击松开时候触发
    private void Touch_canceled(InputAction.CallbackContext context)
    {
        InvokeDrop();
    }

    private void Delete_performed(InputAction.CallbackContext context)
    {
        InvokeDelete();
    }

    public Vector3 GetMousePos()
    {
        if (PublicTool.GetSceneGameMgr().mapCamera != null)
        {
            return PublicTool.GetSceneGameMgr().mapCamera.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Ray GetMouseRay()
    {
        Vector2 screenPosition = touchPositionAction.ReadValue<Vector2>();
        Ray ray = PublicTool.GetSceneGameMgr().mapCamera.ScreenPointToRay(screenPosition);
        return ray;
    }

    #endregion

    #region Drag

    /// <summary>
    /// 是否在拖拽设施中
    /// </summary>
    private bool isDraggingFacility = false;
    /// <summary>
    /// 正在拖拽设施的类型ID（从设施UI开始）
    /// </summary>
    private int recordFacilityUIID = -1;


    /// <summary>
    /// 是否在连线
    /// </summary>
    private bool isLinkingSlot = false;
    /// <summary>
    /// 开始连线的孔
    /// </summary>
    private Vector2Int linkStartSlot = new Vector2Int(-1, -1);
    /// <summary>
    /// 开始连线的孔类型
    /// </summary>
    private SlotType linkStartSlotType;

    /// <summary>
    /// 按住鼠标时候触发 检测是否抓住东西
    /// </summary>
    private void InvokeCheckDrag()
    {
        if (!isDraggingFacility && !isLinkingSlot)
        {
            //检查是否开始拖拽点为设施UI
            CheckGraphicRay();
            if (CheckRayHoverFacilityButton())
            {
                if (recordFacilityUIID > 0)
                {
                    //开始从UI拖拽设施
                    isDraggingFacility = true;
                    EventCenter.Instance.EventTrigger("StartDragFacility", recordFacilityUIID);
                }
                return;
            }

            //检查是否开始拖拽点为出孔
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("MapSlot"));
            if (hit.transform != null && hit.transform.parent.GetComponent<MapSlotItem>() != null)
            {
                MapSlotItem itemSlot = hit.transform.parent.GetComponent<MapSlotItem>();

                linkStartSlot = new Vector2Int(itemSlot.FacilityKeyID, itemSlot.slotID);
                linkStartSlotType = itemSlot.slotType;
                isLinkingSlot = true;
                EventCenter.Instance.EventTrigger("StartLinkSlot", linkStartSlot);
            }
        }

    }

    /// <summary>
    /// 释放鼠标时触发
    /// </summary>
    private void InvokeDrop()
    {
        if (isDraggingFacility)
        {
            //从UI处拖拽出Facility的情况
            if(recordFacilityUIID > 0)
            {
                FacilityExcelItem excelItem = PublicTool.GetFacilityItem(recordFacilityUIID);
                Vector3 realDropPos = GetMousePos() - PublicTool.CalculateFacilityModelDelta(excelItem.sizeX, excelItem.sizeY);
                Vector2Int tarPosID = PublicTool.ConvertPosToID(realDropPos);
                EventCenter.Instance.EventTrigger("SetFacility", new SetFacilityInfo(recordFacilityUIID, tarPosID));
                recordFacilityUIID = -1;
            }

            //不管是否有拖出Facility,都不影响发出该信号
            EventCenter.Instance.EventTrigger("EndDragFacility", null);
            isDraggingFacility = false;
            return;
        }

        if (isLinkingSlot)
        {
            if (linkStartSlot.x >= 0)
            {
                bool haveSetLink = false;
                //检查是否落到了孔
                RaycastHit2D hitSlot = Physics2D.Raycast(GetMousePos(), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("MapSlot"));
                if (!haveSetLink && hitSlot.transform != null && hitSlot.transform.parent.GetComponent<MapSlotItem>() != null)
                {
                    MapSlotItem itemSlot = hitSlot.transform.parent.GetComponent<MapSlotItem>();

                    //检查是否为同一个设施
                    if(linkStartSlot.x == itemSlot.FacilityKeyID)
                    {
                        Debug.Log("Can not link same facility");
                    }
                    //检查是否为同种类孔
                    else if(itemSlot.slotType == linkStartSlotType)
                    {
                        Debug.Log("Can not link slots in the same type");
                    }
                    else
                    {
                        //检查是否该Slot已满
                        SceneGameData sceneGameData = PublicTool.GetSceneGameData();
                        if (sceneGameData.dicFacility.ContainsKey(itemSlot.FacilityKeyID))
                        {
                            FacilitySetData facilityData = sceneGameData.dicFacility[itemSlot.FacilityKeyID];
                            //如果该Slot是空的
                            if(itemSlot.slotType == SlotType.In && facilityData.listSlotIn.Count > itemSlot.slotID && facilityData.listSlotIn[itemSlot.slotID].x < 0)
                            {
                                //建立连接 Drop的点为入孔
                                haveSetLink = true;
                                EventCenter.Instance.EventTrigger("SetLink", new SetLinkInfo(linkStartSlot.x, linkStartSlot.y,itemSlot.FacilityKeyID,itemSlot.slotID));
                            }
                            else if(itemSlot.slotType == SlotType.Out && facilityData.listSlotOut.Count > itemSlot.slotID && facilityData.listSlotOut[itemSlot.slotID].x < 0)
                            {
                                //建立连接  Drop的点为出孔
                                haveSetLink = true;
                                EventCenter.Instance.EventTrigger("SetLink", new SetLinkInfo(itemSlot.FacilityKeyID, itemSlot.slotID,linkStartSlot.x, linkStartSlot.y));
                            }
                            else
                            {
                                Debug.Log("Invalid Slot");
                            }
                        }
                    }
                }
            }

            //不管是否有拖到终点Facility,都不影响发出该信号
            EventCenter.Instance.EventTrigger("EndLinkSlot", null);
            linkStartSlot = new Vector2Int(-1, -1);
            isLinkingSlot = false;
            return;
        }


    }

    private void InvokeDelete()
    {
        if (isDraggingFacility||isLinkingSlot)
        {
            //还在拖拽物体/连线的时候就不要出现删除操作了罢
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("MapFacility"));
        if (hit.transform != null && hit.transform.parent.GetComponent<MapFacilityItem>() != null)
        {
            Debug.Log("HitFacility");
            MapFacilityItem itemFacility = hit.transform.parent.GetComponent<MapFacilityItem>();
            EventCenter.Instance.EventTrigger("DeleteFacility", itemFacility.GetData().keyID);
        }

    }


    #endregion


    #region RayCheck
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    //在鼠标位置进行射线检测forUI
    private void CheckGraphicRay()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Mouse.current.position.ReadValue();
        //pointerEventData.position = GetMousePos();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
    }

    /// <summary>
    /// 检测是否从设施UI处开始拖拽
    /// </summary>
    /// <returns></returns>
    private bool CheckRayHoverFacilityButton()
    {
        //UI
        foreach(RaycastResult item in raycastResults)
        {
            if(item.gameObject.tag == "FacilityButton")
            {
                if (item.gameObject.GetComponent<FacilityButtonUIItem>() != null)
                {
                    FacilityButtonUIItem facilityButton = item.gameObject.GetComponent<FacilityButtonUIItem>();
                    recordFacilityUIID = facilityButton.GetFacilityID();
                    //Debug.Log(recordFacilityUIID);
                    return true;
                }
            }
        }
        recordFacilityUIID = -1;
        return false;
    }


    #endregion


}
