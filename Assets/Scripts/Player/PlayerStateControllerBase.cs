using Survivors.Base;
using Survivors.Managers;
using Survivors.ScriptableObjets;

namespace Survivors.Player
{
    public class PlayerStateControllerBase : StateControllerBase
    {
        //============================================================================================================//
        
        public void SetupPlayer(in PlayerProfileScriptableObject playerProfile)
        {
            base.Start();

            var playerHealth = GetComponent<PlayerHealth>();
            
            playerHealth.SetStartingHealth(playerProfile.startingHealth);
            MovementController.SetSpeed(playerProfile.moveSpeed);
            shadowOffset = playerProfile.shadowOffset;

            ItemManager.AddStarters(playerProfile.startingWeapons, playerProfile.startingPassives);

            AnimationController.SetAnimationProfile(playerProfile.animationProfile);
            SetState(STATE.IDLE);
        }
        
        //Unity Functions
        //============================================================================================================//
        
        protected override void OnEnable()
        {
            base.OnEnable();
            InputDelegator.OnMovementChanged += OnMovementChanged;
        }

        protected override void Start()
        { }

        protected override void OnDisable()
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
        { }
    }
}