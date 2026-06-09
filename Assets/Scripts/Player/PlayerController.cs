using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : ActorController
{
    public bool StopMovement = false;
    public bool StopFire = false;
    public bool StopAction = false;
    
    public void Stop()
    {
        _onMoveInput?.Invoke(Vector2.zero);
        
        StopMovement = true;
        StopAction = true;
        StopFire = true;
    }

    public void Run()
    {
        StopMovement = false;
        StopAction = false;
        StopFire = false;
    }
    
    public void OnMoveInputHandler(InputAction.CallbackContext context)
    {
        if (!_isDead && !StopMovement)
        {
            var input = context.ReadValue<Vector2>();
            _onMoveInput?.Invoke(input);
        }
    }
    
    public void OnAttackInputHandler(InputAction.CallbackContext context)
    {
        if (context.performed && !_isDead && !StopFire)
        {
            _onAttackInput?.Invoke(true);
        }
    }
    
    public void OnKeyPressHandler(InputAction.CallbackContext context)
    {
        if (context.performed && !_isDead && !StopAction)
        {
            Debug.Log($"actionName = {context.action.name}");

            if (Enum.TryParse<UserInputActionName>(context.action.name, out var actionName))
            {
                _onKeyInput?.Invoke(actionName);
            }
        }
    }
}