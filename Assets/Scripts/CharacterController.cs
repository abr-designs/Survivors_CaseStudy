using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Survivors
{
    [RequireComponent(typeof(AnimationController))]
    [RequireComponent(typeof(MovementController))]
    public class CharacterController : MonoBehaviour
    {
        //Properties
        //============================================================================================================//
        private bool _isDead;
        
        private AnimationController _animationController;
        private MovementController _movementController;
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private STATE defaultAnimationState;
        private STATE _currentAnimationState;

        //Unity Functions
        //============================================================================================================//
        private void OnEnable()
        {
            InputDelegator.OnMovementChanged += OnMovementChanged;
            InputDelegator.OnKillPressed += () =>
            {
                SetState(STATE.DEATH);
            };
        }

        // Start is called before the first frame update
        private void Start()
        {
            _movementController = GetComponent<MovementController>();
            _animationController = GetComponent<AnimationController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            SetState(defaultAnimationState);
        }

        private void Update()
        {
            if (_isDead)
                return;

            UpdateState();
        }

        private void OnDisable()
        {
            InputDelegator.OnMovementChanged -= OnMovementChanged;
        }

        //State Functions
        //============================================================================================================//

        private void SetState(in STATE newState)
        {
            _currentAnimationState = newState;
            _animationController.SetCurrentState(newState);
            switch (newState)
            {
                case STATE.DEATH:
                    _isDead = true;
                    _movementController.enabled = false;
                    break;
            }
        }

        private void UpdateState()
        {
            switch (_currentAnimationState)
            {
                case STATE.IDLE:
                    IdleState();
                    break;
                case STATE.RUN:
                    RunState();
                    break;
            }
        }

        private void IdleState()
        {
            if(_movementController.IsMoving == false)
                return;
            
            SetState(STATE.RUN);
        }

        private void RunState()
        {
            if(_movementController.IsMoving)
                return;
            
            SetState(STATE.IDLE);
        }

        //Callback Functions
        //============================================================================================================//
        
        private void OnMovementChanged(float x, float _)
        {
            if (_isDead)
                return;
            
            if (x < 0)
                _spriteRenderer.flipX = true;
            else if (x > 0)
                _spriteRenderer.flipX = false;
        }
        //============================================================================================================//
    }
}
