using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Survivors
{
    public class InputDelegator: MonoBehaviour, SInput.IGameplayActions
    {
        public static Action OnKillPressed;
        public static Action<float, float> OnMovementChanged;
        
        
        private float inputX, inputY;

        //Unity Functions
        //============================================================================================================//
        
        private void OnEnable()
        {
            Input.SInput.Gameplay.Enable();
        }

        private void Start()
        {
            Input.SInput.Gameplay.SetCallbacks(this);
        }

        private void OnDisable()
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

        public void OnKill(InputAction.CallbackContext context)
        {
            if (context.performed == false)
                return;

            if (context.ReadValueAsButton() == false)
                return;
            
            OnKillPressed?.Invoke();
        }
    }
}