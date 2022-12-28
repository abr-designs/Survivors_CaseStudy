using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Attacks
{
    public class CrossAttack : AttackBase_v2
    {
        private int projectileCount = 1;

        private readonly Sprite _sprite;
        private readonly Color32 _spriteColor;

        private float launchSpeed;
        private float acceleration;
        private float spinSpeed;

        
        private float projectileInterval;
        private float projectileRadius;

        public CrossAttack(in AttackProfileScriptableObject attackProfile) : base(in attackProfile)
        {
            _sprite = attackProfile.sprite;
            _spriteColor = attackProfile.spriteColor;

            launchSpeed = attackProfile.launchSpeed;
            acceleration = attackProfile.acceleration;
            spinSpeed = attackProfile.spinSpeed;
            
            projectileInterval = attackProfile.projectileInterval;
            projectileRadius = attackProfile.projectileRadius;
        }
        
        protected override void LevelUp()
        {
            throw new System.NotImplementedException();
        }

        public override void PostUpdate()
        {

        }

        protected override void TriggerAttack()
        {
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            var waitForSeconds = new WaitForSeconds(projectileInterval);
            for (var i = 0; i < projectileCount; i++)
            {
                var closestEnemy = EnemyManager.GetClosestEnemy(PlayerPosition);
                if (closestEnemy == null)
                    continue;

                var closestPosition = (Vector2)closestEnemy.transform.position;
                
                var newProjectile = FactoryManager
                    .GetFactory<ProjectileFactory>()
                    .CreateProjectile(PlayerPosition, _sprite, _spriteColor);
                
                StartCoroutine(CrossProjectileCoroutine(
                    newProjectile.transform, 
                    (closestPosition - PlayerPosition).normalized));

                yield return waitForSeconds;
            }
        }

        private IEnumerator CrossProjectileCoroutine(Transform projectileTransform, Vector2 direction)
        {
            var currentSpeed = launchSpeed;
            var currentPosition = (Vector2)projectileTransform.position;
            var currentRotation = projectileTransform.localEulerAngles;
            var alreadyHit = new HashSet<EnemyHealth>();

            for (var t = 0f; t < 5f; t+= Time.deltaTime)
            {
                currentPosition += direction * (currentSpeed * Time.deltaTime);
                currentSpeed += acceleration;

                currentRotation.z += spinSpeed * Time.deltaTime;

                projectileTransform.localEulerAngles = currentRotation;
                projectileTransform.position = currentPosition;
                
                var enemiesInRange = EnemyManager.GetEnemiesInRange(currentPosition, projectileRadius, alreadyHit);
                if (enemiesInRange != null)
                {
                    foreach (var enemyHealth in enemiesInRange)
                    {
                        enemyHealth.ChangeHealth(-damage);
                        alreadyHit.Add(enemyHealth);
                    }
                }

                yield return null;
            }
            
            Object.Destroy(projectileTransform.gameObject);
        }


    }
}