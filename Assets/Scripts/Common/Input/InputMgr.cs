using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMgr : MonoSingleton<InputMgr>
{
    private PlayerInput playerInput;
    private InputAction touchAction;
    private InputAction touchPositionAction;
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
    }



    private void DisableInput()
    {
        touchAction.performed -= Touch_performed;
        touchAction.canceled -= Touch_canceled;

        if(playerInput != null)
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

    }

    //�������ɿ�ʱ�򴥷�
    private void Touch_canceled(InputAction.CallbackContext context)
    {

    }
    #endregion
}
