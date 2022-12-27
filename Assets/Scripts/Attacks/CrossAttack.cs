using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Managers;
using UnityEngine;

namespace Survivors.Attacks
{
    public class CrossAttack : AttackBase
    {
        [SerializeField, Header("Cross Attack")]
        private GameObject crossPrefab;

        [SerializeField]
        private float launchSpeed;
        [SerializeField]
        private float acceleration;
        [SerializeField]
        private float spinSpeed;
        
        [SerializeField, Min(1), Header("Projectiles")]
        private int projectileCount = 1;
        [SerializeField, Min(0)]
        private float projectileInterval;
        [SerializeField, Min(0f)]
        private float projectileRadius;
        
        public override void Setup()
        {
            
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
                
                var newProjectile = Instantiate(crossPrefab, PlayerPosition, Quaternion.identity, transform);
                
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
            
            Destroy(projectileTransform.gameObject);
        }
    }
}