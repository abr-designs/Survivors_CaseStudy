using Survivors.Base;
using Survivors.ScriptableObjets.Animation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Survivors.Player
{
    public class PlayerStateControllerBase : StateControllerBase
    {
        [SerializeField]
        private AnimationProfileScriptableObject animationProfile;
        
        private void OnEnable()
        {
            InputDelegator.OnMovementChanged += OnMovementChanged;
            InputDelegator.OnKillPressed += () =>
            {
                SetState(STATE.DEATH);
            };
        }

        protected override void Start()
        {
            base.Start();

            AnimationController.SetAnimationProfile(animationProfile);
        }
        
        private void OnDisable()
        {
            InputDelegator.OnMovementChanged -= OnMovementChanged;
        }
        
        protected override void IdleState()
        {
            if(MovementController.IsMoving == false)
                return;
            
            SetState(STATE.RUN);
        }

        protected override void RunState()
        {
            if(MovementController.IsMoving)
                return;
            
            SetState(STATE.IDLE);
        }

        protected override void DeathState()
        { }
    }
}