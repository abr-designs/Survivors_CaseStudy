using System;
using Survivors.Base;
using Survivors.Player;
using Survivors.ScriptableObjets.Enemies;
using UnityEngine;

namespace Survivors.Enemies
{
    public class EnemyStateController  : StateControllerBase
    {
        [SerializeField]
        private EnemyHealth enemyHealth;
        [SerializeField]
        private EnemyMovementController enemyMovementController;
        [SerializeField]
        private AnimationControllerBase animationControllerBase;

        [SerializeField] 
        private new Collider2D collider2D;
        
        private Transform _playerTransform;
        
        //============================================================================================================//
        
        protected override void Start()
        { }
        
        //============================================================================================================//

        public void SetupEnemy(in EnemyProfileScriptableObject enemyProfile, in float difficultyMultiplier = 1f)
        {
            MovementController = enemyMovementController;
            AnimationController = animationControllerBase;
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _playerTransform = FindObjectOfType<PlayerMovementController>().transform;
            
            SpriteRenderer.sprite = enemyProfile.defaultSprite;
            enemyHealth.SetHealth(enemyProfile.baseHealth * difficultyMultiplier);
            enemyMovementController.SetSpeed(enemyProfile.baseSpeed * difficultyMultiplier);
            shadowOffset = enemyProfile.shadowOffset;

            collider2D.offset = enemyProfile.colliderOffset;

            switch (collider2D)
            {
                case BoxCollider2D boxCollider2D:
                    boxCollider2D.size = enemyProfile.boxColliderSize;
                    break;
                case CircleCollider2D circleCollider2D:
                    circleCollider2D.radius = enemyProfile.circleColliderRadius;
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            animationControllerBase.SetAnimationProfile(enemyProfile.animationProfile);
            SetState(enemyProfile.defaultState);
        }

        protected override void RunState()
        {
            var playerPosition = _playerTransform.position;
            var currentPosition = transform.position;

            var dir = (playerPosition - currentPosition).normalized;
            
            enemyMovementController.SetMoveDirection(dir);
            
            OnMovementChanged(dir.x, default);
        }

        protected override void DeathState()
        {
            
        }
    }
}