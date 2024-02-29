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

    private bool isDragging = false;

    /// <summary>
    /// 按住鼠标时候触发 检测是否抓住东西
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
    /// 释放鼠标时触发
    /// </summary>
    private void InvokeDrop()
    {
        if (isDragging)
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
            isDragging = false;
        }
    }

    private void InvokeDelete()
    {
        if (isDragging)
        {
            //还在拖拽物体的时候就不要出现删除操作了罢
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

    //在鼠标位置进行射线检测forUI
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
