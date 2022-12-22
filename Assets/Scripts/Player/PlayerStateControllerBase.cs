using Survivors.ScriptableObjets.Animation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Survivors.Player
{
    public class PlayerStateControllerBase : StateControllerBase
    {
        [FormerlySerializedAs("_animationProfile")] [SerializeField]
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
    }
}