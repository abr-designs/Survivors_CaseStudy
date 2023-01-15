using System;
using Survivors.Base.Interfaces;
using Survivors.Base.Managers.Interfaces;
using UnityEngine;

namespace Survivors.Base
{
    public abstract class StateControllerBase : IShadow, IEnable, IUpdate
    {
        //Properties
        //============================================================================================================//
        public Transform transform { get; }

        private bool _isDead;
        
        protected IAnimationController AnimationController;
        protected IMovementController MovementController;
        protected SpriteRenderer SpriteRenderer;

        private STATE _currentAnimationState;

        public float ShadowOffset => shadowOffset;
        protected float shadowOffset;
        //============================================================================================================//

        public StateControllerBase(
            in IMovementController movementController,
            in IAnimationController animationController, 
            in SpriteRenderer spriteRenderer,
            in STATE defaultAnimationState)
        {
            transform = spriteRenderer.transform;
            SpriteRenderer = spriteRenderer;

            MovementController = movementController;
            AnimationController = animationController;

            SetState(defaultAnimationState);

        }

        //Unity Functions
        //============================================================================================================//

        public virtual void OnEnable()
        {
            IShadow.OnAddShadow?.Invoke(this);
        }

        public void Update()
        {
            if (_isDead)
                return;

            UpdateState();
        }

        public virtual void OnDisable()
        {
            IShadow.OnRemoveShadow?.Invoke(this);
        }

        //State Functions
        //============================================================================================================//

        protected void SetState(in STATE newState)
        {
            _currentAnimationState = newState;
            AnimationController.SetCurrentState(newState);
            switch (newState)
            {
                case STATE.DEATH:
                    _isDead = true;
                    throw new NotImplementedException("Need to stop movement");
                    //MovementController.SetActive(false);
                    break;
            }
        }

        private void UpdateState()
        {
            switch (_currentAnimationState)
            {
                case STATE.NONE:
                    return;
                case STATE.IDLE:
                    IdleState();
                    break;
                case STATE.RUN:
                    RunState();
                    break;
                case STATE.ATTACK:
                    break;
                case STATE.DEATH:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void IdleState()
        {
            throw new NotImplementedException( "Make sure you fill out the state in override function");
        }

        protected virtual void RunState()
        {
            throw new NotImplementedException( "Make sure you fill out the state in override function");
        }

        protected virtual void AttackState()
        {
            throw new NotImplementedException( "Make sure you fill out the state in override function");
        }

        protected abstract void DeathState();

        //Callback Functions
        //============================================================================================================//
        
        protected void OnMovementChanged(float x, float _)
        {
            if (_isDead)
                return;

            switch (x)
            {
                case < 0:
                    SpriteRenderer.flipX = true;
                    break;
                case > 0:
                    SpriteRenderer.flipX = false;
                    break;
            }
        }
        //============================================================================================================//
        
    }
}
