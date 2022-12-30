using System;
using Survivors.Base.Managers;
using Survivors.Base.Managers.Interfaces;
using Survivors.Managers;
using UnityEngine.InputSystem;

namespace Survivors
{
    public class InputDelegator: ManagerBase, IEnable, SInput.IGameplayActions
    {
        public static Action<float, float> OnMovementChanged;
        
        
        private float inputX, inputY;

        public InputDelegator()
        {
            Input.SInput.Gameplay.SetCallbacks(this);
        }

        //Unity Functions
        //============================================================================================================//
        
        public void OnEnable()
        {
            Input.SInput.Gameplay.Enable();
        }

        public void OnDisable()
        {
            Input.SInput.Gameplay.Disable();
        }

        //IGameplayActions Callbacks
        //============================================================================================================//
        
        public void OnVerticalMovement(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            if (Math.Abs(value - inputY) < 0.001f)
                return;

            inputY = value;
            OnMovementChanged?.Invoke(inputX, inputY);
        }

        public void OnHorizontalMovement(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            if (Math.Abs(value - inputX) < 0.001f)
                return;

            inputX = value;
            OnMovementChanged?.Invoke(inputX, inputY);
        }
    }
}