using Survivors.Base;
using Survivors.Base.Interfaces;
using Survivors.Managers;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Player
{
    public class PlayerStateControllerBase : StateControllerBase
    {
        //============================================================================================================//
        public PlayerStateControllerBase(
            in PlayerHealth playerHealth,
            in PlayerProfileScriptableObject playerProfile,
            in IMovementController movementController, 
            in IAnimationController animationController, 
            in SpriteRenderer spriteRenderer, 
            in STATE defaultAnimationState) : base(in movementController, in animationController, in spriteRenderer, in defaultAnimationState)
        {
            playerHealth.SetStartingHealth(playerProfile.startingHealth);
            MovementController.SetSpeed(playerProfile.moveSpeed);
            shadowOffset = playerProfile.shadowOffset;

            ItemManager.AddStarters(playerProfile.startingWeapons, playerProfile.startingPassives);

            AnimationController.SetAnimationProfile(playerProfile.animationProfile);
            SetState(STATE.IDLE);
            OnEnable();
        }
        
        //Unity Functions
        //============================================================================================================//
        
        public override void OnEnable()
        {
            base.OnEnable();
            InputDelegator.OnMovementChanged += OnMovementChanged;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            InputDelegator.OnMovementChanged -= OnMovementChanged;
        }

        //Override State Functions
        //============================================================================================================//
        
        
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
        {
            
        }


    }
}