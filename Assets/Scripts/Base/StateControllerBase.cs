using System;
using Survivors.Base.Interfaces;
using UnityEngine;

namespace Survivors.Base
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(IAnimationController))]
    [RequireComponent(typeof(IMovementController))]
    public abstract class StateControllerBase : MonoBehaviour, IShadow
    {
        //Properties
        //============================================================================================================//
        private bool _isDead;
        
        protected IAnimationController AnimationController;
        protected IMovementController MovementController;
        protected SpriteRenderer SpriteRenderer;

        [SerializeField]
        protected STATE defaultAnimationState;
        private STATE _currentAnimationState;

        public float ShadowOffset => shadowOffset;
        [SerializeField] protected float shadowOffset;

        //Unity Functions
        //============================================================================================================//

        protected virtual void OnEnable()
        {
            IShadow.OnAddShadow?.Invoke(this);
        }

        // Start is called before the first frame update
        //FIXME I dont want this to be a virtual, to prevent accidental overwrites
        protected virtual void Start()
        {
            MovementController = GetComponent<IMovementController>();
            AnimationController = GetComponent<IAnimationController>();
            SpriteRenderer = GetComponent<SpriteRenderer>();

            SetState(defaultAnimationState);
        }

        private void Update()
        {
            if (_isDead)
                return;

            UpdateState();
        }

        protected virtual void OnDisable()
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
                    MovementController.SetActive(false);
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
