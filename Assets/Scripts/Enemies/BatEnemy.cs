using System;
using Survivors.Base;
using Survivors.Player;
using Survivors.ScriptableObjets.Animation;
using UnityEngine;

namespace Survivors.Enemies
{
    [RequireComponent(typeof(EnemyMovementController)), Obsolete("Use EnemyStateController instead")]
    public class BatEnemy : StateControllerBase
    {

        /*[SerializeField]
        private AnimationProfileScriptableObject animationProfile;
        
        private Transform _playerTransform;
        private EnemyMovementController _enemyMovementController;*/

        protected override void Start()
        {
            
            /*base.Start();
            
            _playerTransform = FindObjectOfType<PlayerMovementController>().transform;
            _enemyMovementController = (EnemyMovementController) MovementController;
            
            AnimationController.SetAnimationProfile(animationProfile);
            SetState(defaultAnimationState);*/
        }

        /*protected override void RunState()
        {
            var playerPosition = _playerTransform.position;
            var currentPosition = transform.position;

            var dir = (playerPosition - currentPosition).normalized;
            
            _enemyMovementController.SetMoveDirection(dir);
            
            OnMovementChanged(dir.x, default);
        }*/

        protected override void DeathState()
        {
            
        }
    }
}