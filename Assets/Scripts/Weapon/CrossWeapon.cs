using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks.Items;
using Survivors.Weapons.Interfaces;
using UnityEngine;

namespace Survivors.Weapons
{
    public class CrossWeapon : WeaponBase_v2, IUseProjectiles, IUserProjectileSpeed
    {
        private readonly Sprite _projectileSprite;
        private readonly Color32 _spriteColor;

        public int ProjectileCount => projectileCount + PassiveManager.ProjectileAdd;
        private int projectileCount = 1;

        public float LaunchSpeed => launchSpeed * PassiveManager.ProjectileSpeed;
        public float Acceleration => acceleration * PassiveManager.ProjectileSpeed;

        private float launchSpeed;
        private float acceleration;
        private float spinSpeed;

        
        private float projectileInterval;
        private float projectileRadius;

        public CrossWeapon(in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            _projectileSprite = weaponProfile.projectileSprite;
            _spriteColor = weaponProfile.projectileSpriteColor;

            launchSpeed = weaponProfile.launchSpeed;
            acceleration = weaponProfile.acceleration;
            spinSpeed = weaponProfile.spinSpeed;
            
            projectileInterval = weaponProfile.projectileInterval;
            projectileRadius = weaponProfile.projectileRadius;
        }
        
        public override void LevelUp()
        {
            throw new System.NotImplementedException();
        }

        public override void OnScaleChanged(float newScale)
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
            for (var i = 0; i < ProjectileCount; i++)
            {
                var closestEnemy = EnemyManager.GetClosestEnemy(PlayerPosition);
                if (closestEnemy == null)
                    continue;

                var closestPosition = (Vector2)closestEnemy.transform.position;
                
                var newProjectile = FactoryManager
                    .GetFactory<ProjectileFactory>()
                    .CreateProjectile(PlayerPosition, _projectileSprite, _spriteColor);
                
                StartCoroutine(CrossProjectileCoroutine(
                    newProjectile.transform, 
                    (closestPosition - PlayerPosition).normalized));

                yield return waitForSeconds;
            }
        }

        private IEnumerator CrossProjectileCoroutine(Transform projectileTransform, Vector2 direction)
        {
            var currentSpeed = LaunchSpeed;
            var accel = Acceleration;
            
            var currentPosition = (Vector2)projectileTransform.position;
            var currentRotation = projectileTransform.localEulerAngles;
            var alreadyHit = new HashSet<EnemyHealth>();

            for (var t = 0f; t < 5f; t+= Time.deltaTime)
            {
                currentPosition += direction * (currentSpeed * Time.deltaTime);
                currentSpeed += accel;

                currentRotation.z += spinSpeed * Time.deltaTime;

                projectileTransform.localEulerAngles = currentRotation;
                projectileTransform.position = currentPosition;
                
                var enemiesInRange = EnemyManager.GetEnemiesInRange(currentPosition, projectileRadius, alreadyHit);
                if (enemiesInRange != null)
                {
                    foreach (var enemyHealth in enemiesInRange)
                    {
                        enemyHealth.ChangeHealth(-Damage);
                        alreadyHit.Add(enemyHealth);
                    }
                }

                yield return null;
            }
            
            Object.Destroy(projectileTransform.gameObject);
        }



    }
}