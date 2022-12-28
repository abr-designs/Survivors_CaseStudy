using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Attacks
{
    public class AxeAttack : AttackBase_v2
    {
        private int projectileCount = 1;

        private readonly Sprite _sprite;
        private readonly Color32 _spriteColor;

        private float launchSpeed;
        private float acceleration;
        private float spinSpeed;

        private int maxHitCount;
        
        private float projectileInterval;
        private float projectileRadius;
        
        public AxeAttack(in AttackProfileScriptableObject attackProfile) : base(in attackProfile)
        {
            _sprite = attackProfile.sprite;
            _spriteColor = attackProfile.spriteColor;

            launchSpeed = attackProfile.launchSpeed;
            acceleration = attackProfile.acceleration;
            spinSpeed = attackProfile.spinSpeed;
            
            maxHitCount = attackProfile.maxHitCount;
            
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
                var direction = Vector2.up;

                direction.x = Random.Range(-0.5f, 0.5f);

                var newProjectile = FactoryManager
                    .GetFactory<ProjectileFactory>()
                    .CreateProjectile(PlayerPosition, _sprite, _spriteColor);
                
                StartCoroutine(AxeProjectileCoroutine(
                    newProjectile.transform, 
                    direction.normalized));

                yield return waitForSeconds;
            }
        }

        private IEnumerator AxeProjectileCoroutine(Transform projectileTransform, Vector2 direction)
        {
            var currentSpeed = direction * launchSpeed;
            var currentPosition = PlayerPosition;
            var currentRotation = projectileTransform.localEulerAngles;
            var alreadyHit = new HashSet<EnemyHealth>();
            var hitCount = 0;

            for (var t = 0f; t < 5f || hitCount < maxHitCount; t+= Time.deltaTime)
            {
                currentPosition += currentSpeed * Time.deltaTime;
                currentSpeed += acceleration * Vector2.up;

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
                        hitCount++;
                    }
                }

                yield return null;
            }
            
            Object.Destroy(projectileTransform.gameObject);
        }


    }
}