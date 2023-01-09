using Survivors.Base;
using Survivors.Factories;
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

        private int _xpDrop;
        
        private Transform _playerTransform;
        
        //============================================================================================================//

        protected override void OnEnable()
        {
            base.OnEnable();
            enemyHealth.OnKilled += OnKilled;
        }

        protected override void Start()
        { }

        protected override void OnDisable()
        {
            base.OnDisable();
            enemyHealth.OnKilled -= OnKilled;
        }

        //============================================================================================================//

        public void SetupEnemy(in Transform playerTransform, in EnemyProfileScriptableObject enemyProfile, in float difficultyMultiplier = 1f)
        {
            MovementController = enemyMovementController;
            AnimationController = animationControllerBase;
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _playerTransform = playerTransform;
            
            SpriteRenderer.sprite = enemyProfile.defaultSprite;
            enemyHealth.SetStartingHealth(enemyProfile.baseHealth * difficultyMultiplier);
            enemyMovementController.SetSpeed(enemyProfile.baseSpeed * difficultyMultiplier);
            shadowOffset = enemyProfile.shadowOffset;

            enemyHealth.Damage = enemyProfile.baseDamage;
            _xpDrop = enemyProfile.xpDrop;

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
        
        private void OnKilled()
        {
            FactoryManager
                .GetFactory<CollectableFactory>()
                .CreateXpCollectable(_xpDrop, transform.position);
        }
        
    }
}