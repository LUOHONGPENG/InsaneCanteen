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

    private bool isDragging = false;

    /// <summary>
    /// ��ס���ʱ�򴥷� ����Ƿ�ץס����
    /// </summary>
    private void InvokeCheckDrag()
    {
        //UI
        if (!isDragging)
        {
            CheckGraphicRay();
            CheckRayHoverUI();
            if (CheckRayHoverUI())
            {
                isDragging = true;
                if (recordFacilityUIID > 0)
                {
                    EventCenter.Instance.EventTrigger("StartDragFacility", recordFacilityUIID);
                }
            }
        }

    }

    /// <summary>
    /// �ͷ����ʱ����
    /// </summary>
    private void InvokeDrop()
    {
        if (isDragging)
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
            isDragging = false;
        }
    }

    private void InvokeDelete()
    {
        if (isDragging)
        {
            //������ק�����ʱ��Ͳ�Ҫ����ɾ�������˰�
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("MapFacility"));
        if (hit.transform == null)
        {
            return;
        }

        if (hit.transform.parent.GetComponent<MapFacilityItem>() != null)
        {
            Debug.Log("HitFacility");
            MapFacilityItem item = hit.transform.parent.GetComponent<MapFacilityItem>();
            EventCenter.Instance.EventTrigger("DeleteFacility", item.GetData().keyID);
        }

    }


    #endregion


    #region RayCheck
    int recordFacilityUIID = -1;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    //�����λ�ý������߼��forUI
    private void CheckGraphicRay()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Mouse.current.position.ReadValue();
        //pointerEventData.position = GetMousePos();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
    }

    private bool CheckRayHoverUI()
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
        return false;
    }


    #endregion


}
