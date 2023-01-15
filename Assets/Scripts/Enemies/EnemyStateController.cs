using System;
using Survivors.Base;
using Survivors.Base.Interfaces;
using Survivors.Factories;
using Survivors.Player;
using Survivors.ScriptableObjets.Enemies;
using UnityEngine;

namespace Survivors.Enemies
{
    public class EnemyStateController  : StateControllerBase
    {
        private EnemyHealth _enemyHealth;
        private EnemyMovementController _enemyMovementController;
        private IAnimationController _animationControllerBase;

        //private Collider2D collider2D;

        private readonly int _xpDrop;
        
        private readonly Transform _playerTransform;
        
        //============================================================================================================//

        public EnemyStateController(
            in Transform playerTransform, 
            in EnemyProfileScriptableObject enemyProfile,
            in EnemyHealth enemyHealth, 
            in Collider2D collider2D,
            
            in EnemyMovementController enemyMovementController,
            in IAnimationController animationController, 
            in SpriteRenderer spriteRenderer,
            in float difficultyMultiplier = 1f) : 
            base(enemyMovementController, in animationController, in spriteRenderer, in enemyProfile.defaultState)
        {
            if (playerTransform == null)
                throw new Exception();
            
            _playerTransform = playerTransform;
            _enemyHealth = enemyHealth;
            _enemyMovementController = enemyMovementController;
            _animationControllerBase = animationController;
            
            SpriteRenderer.sprite = enemyProfile.defaultSprite;
            _enemyHealth.SetStartingHealth(enemyProfile.baseHealth * difficultyMultiplier);
            _enemyMovementController.SetSpeed(enemyProfile.baseSpeed * difficultyMultiplier);
            shadowOffset = enemyProfile.shadowOffset;

            _enemyHealth.Damage = enemyProfile.baseDamage;
            _xpDrop = enemyProfile.xpDrop;

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
            
            animationController.SetAnimationProfile(enemyProfile.animationProfile);
            SetState(enemyProfile.defaultState);
            OnEnable();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _enemyHealth.OnKilled += OnKilled;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _enemyHealth.OnKilled -= OnKilled;
        }

        //============================================================================================================//

        protected override void RunState()
        {
            var playerPosition = _playerTransform.position;
            var currentPosition = transform.position;

            var dir = (playerPosition - currentPosition).normalized;
            
            _enemyMovementController.SetMoveDirection(dir);
            
            OnMovementChanged(dir.x, default);
        }

        protected override void DeathState()
        {
            
        }
        
        private void OnKilled()
        {
            OnDisable();
            FactoryManager
                .GetFactory<CollectableFactory>()
                .CreateXpCollectable(_xpDrop, transform.position);
        }

    }
}