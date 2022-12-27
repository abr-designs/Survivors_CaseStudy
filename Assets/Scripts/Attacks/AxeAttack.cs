using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Managers;
using UnityEngine;

namespace Survivors.Attacks
{
    public class AxeAttack : AttackBase
    {
        [SerializeField, Header("Axe Attack")]
        private GameObject crossPrefab;

        [SerializeField]
        private float launchSpeed;
        [SerializeField]
        private float acceleration;
        [SerializeField]
        private float spinSpeed;

        [SerializeField, Min(1)]
        private int maxHitCount;
        
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
                var direction = Vector2.up;

                direction.x = Random.Range(-0.5f, 0.5f);

                var newProjectile = Instantiate(crossPrefab, PlayerPosition, Quaternion.identity, transform);
                
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
            
            Destroy(projectileTransform.gameObject);
        }
    }
}