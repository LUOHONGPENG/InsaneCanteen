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
    //��������ȥʱ�򴥷�
    private void Touch_performed(InputAction.CallbackContext context)
    {
        InvokeCheckDrag();
    }

    //�������ɿ�ʱ�򴥷�
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
    /// �Ƿ�����ק��ʩ��
    /// </summary>
    private bool isDraggingFacility = false;
    /// <summary>
    /// ������ק��ʩ������ID������ʩUI��ʼ��
    /// </summary>
    private int recordFacilityUIID = -1;


    /// <summary>
    /// �Ƿ�������
    /// </summary>
    private bool isLinkingSlot = false;
    /// <summary>
    /// ��ʼ���ߵĿ�
    /// </summary>
    private Vector2Int linkStartSlot = new Vector2Int(-1, -1);
    /// <summary>
    /// ��ʼ���ߵĿ�����
    /// </summary>
    private SlotType linkStartSlotType;

    /// <summary>
    /// ��ס���ʱ�򴥷� ����Ƿ�ץס����
    /// </summary>
    private void InvokeCheckDrag()
    {
        if (!isDraggingFacility && !isLinkingSlot)
        {
            //����Ƿ�ʼ��ק��Ϊ��ʩUI
            CheckGraphicRay();
            if (CheckRayHoverFacilityButton())
            {
                if (recordFacilityUIID > 0)
                {
                    //��ʼ��UI��ק��ʩ
                    isDraggingFacility = true;
                    EventCenter.Instance.EventTrigger("StartDragFacility", recordFacilityUIID);
                }
                return;
            }

            //����Ƿ�ʼ��ק��Ϊ����
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
    /// �ͷ����ʱ����
    /// </summary>
    private void InvokeDrop()
    {
        if (isDraggingFacility)
        {
            //��UI����ק��Facility�����
            if(recordFacilityUIID > 0)
            {
                FacilityExcelItem excelItem = PublicTool.GetFacilityItem(recordFacilityUIID);
                Vector3 realDropPos = GetMousePos() - PublicTool.CalculateFacilityModelDelta(excelItem.sizeX, excelItem.sizeY);
                Vector2Int tarPosID = PublicTool.ConvertPosToID(realDropPos);
                EventCenter.Instance.EventTrigger("SetFacility", new SetFacilityInfo(recordFacilityUIID, tarPosID));
                recordFacilityUIID = -1;
            }

            //�����Ƿ����ϳ�Facility,����Ӱ�췢�����ź�
            EventCenter.Instance.EventTrigger("EndDragFacility", null);
            isDraggingFacility = false;
            return;
        }

        if (isLinkingSlot)
        {
            if (linkStartSlot.x >= 0)
            {
                bool haveSetLink = false;
                //����Ƿ��䵽�˿�
                RaycastHit2D hitSlot = Physics2D.Raycast(GetMousePos(), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("MapSlot"));
                if (!haveSetLink && hitSlot.transform != null && hitSlot.transform.parent.GetComponent<MapSlotItem>() != null)
                {
                    MapSlotItem itemSlot = hitSlot.transform.parent.GetComponent<MapSlotItem>();

                    //����Ƿ�Ϊͬһ����ʩ
                    if(linkStartSlot.x == itemSlot.FacilityKeyID)
                    {
                        Debug.Log("Can not link same facility");
                    }
                    //����Ƿ�Ϊͬ�����
                    else if(itemSlot.slotType == linkStartSlotType)
                    {
                        Debug.Log("Can not link slots in the same type");
                    }
                    else
                    {
                        //����Ƿ��Slot����
                        SceneGameData sceneGameData = PublicTool.GetSceneGameData();
                        if (sceneGameData.dicFacility.ContainsKey(itemSlot.FacilityKeyID))
                        {
                            FacilitySetData facilityData = sceneGameData.dicFacility[itemSlot.FacilityKeyID];
                            //�����Slot�ǿյ�
                            if(itemSlot.slotType == SlotType.In && facilityData.listSlotIn.Count > itemSlot.slotID && facilityData.listSlotIn[itemSlot.slotID].x < 0)
                            {
                                //�������� Drop�ĵ�Ϊ���
                                haveSetLink = true;
                                EventCenter.Instance.EventTrigger("SetLink", new SetLinkInfo(linkStartSlot.x, linkStartSlot.y,itemSlot.FacilityKeyID,itemSlot.slotID));
                            }
                            else if(itemSlot.slotType == SlotType.Out && facilityData.listSlotOut.Count > itemSlot.slotID && facilityData.listSlotOut[itemSlot.slotID].x < 0)
                            {
                                //��������  Drop�ĵ�Ϊ����
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

            //�����Ƿ����ϵ��յ�Facility,����Ӱ�췢�����ź�
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
            //������ק����/���ߵ�ʱ��Ͳ�Ҫ����ɾ�������˰�
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

    //�����λ�ý������߼��forUI
    private void CheckGraphicRay()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Mouse.current.position.ReadValue();
        //pointerEventData.position = GetMousePos();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
    }

    /// <summary>
    /// ����Ƿ����ʩUI����ʼ��ק
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
