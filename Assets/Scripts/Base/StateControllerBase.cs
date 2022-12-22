using Survivors.Base.Interfaces;
using UnityEngine;

namespace Survivors
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(IAnimationController))]
    [RequireComponent(typeof(IMovementController))]
    public abstract class StateControllerBase : MonoBehaviour
    {
        //Properties
        //============================================================================================================//
        private bool _isDead;
        
        private IAnimationController _animationController;
        private IMovementController _movementController;
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private STATE defaultAnimationState;
        private STATE _currentAnimationState;

        //Unity Functions
        //============================================================================================================//

        // Start is called before the first frame update
        private void Start()
        {
            _movementController = GetComponent<IMovementController>();
            _animationController = GetComponent<IAnimationController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            SetState(defaultAnimationState);
        }

        private void Update()
        {
            if (_isDead)
                return;

            UpdateState();
        }

        //State Functions
        //============================================================================================================//

        protected void SetState(in STATE newState)
        {
            _currentAnimationState = newState;
            _animationController.SetCurrentState(newState);
            switch (newState)
            {
                case STATE.DEATH:
                    _isDead = true;
                    _movementController.SetActive(false);
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

        protected virtual void IdleState()
        {
            if(_movementController.IsMoving == false)
                return;
            
            SetState(STATE.RUN);
        }

        protected virtual void RunState()
        {
            if(_movementController.IsMoving)
                return;
            
            SetState(STATE.IDLE);
        }

        //Callback Functions
        //============================================================================================================//
        
        protected void OnMovementChanged(float x, float _)
        {
            if (_isDead)
                return;

            switch (x)
            {
                case < 0:
                    _spriteRenderer.flipX = true;
                    break;
                case > 0:
                    _spriteRenderer.flipX = false;
                    break;
            }
        }
        //============================================================================================================//
    }
}
