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
    public class AxeWeapon : WeaponBase_v2, IUseProjectiles, IUserProjectileSpeed, IUseProjectileRadius
    {
        private readonly Sprite _sprite;
        private readonly Color32 _spriteColor;
        
        public int ProjectileCount => projectileCount + PassiveManager.ProjectileAdd;
        private int projectileCount = 1;

        public float LaunchSpeed => launchSpeed * PassiveManager.ProjectileSpeed;
        public float Acceleration => acceleration * PassiveManager.ProjectileSpeed;
        
        private float launchSpeed;
        private float acceleration;
        private float spinSpeed;

        private int maxHitCount;

        public float ProjectileRadius => projectileRadius * PassiveManager.AttackArea;
        
        private float projectileInterval;
        private float projectileRadius;
        
        public AxeWeapon(in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            _sprite = weaponProfile.sprite;
            _spriteColor = weaponProfile.projectileSpriteColor;

            launchSpeed = weaponProfile.launchSpeed;
            acceleration = weaponProfile.acceleration;
            spinSpeed = weaponProfile.spinSpeed;
            
            maxHitCount = weaponProfile.maxHitCount;
            
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
                var direction = Vector2.up;

                direction.x = Random.Range(-0.5f, 0.5f);

                var newProjectile = FactoryManager
                    .GetFactory<ProjectileFactory>()
                    .CreateProjectile(PlayerPosition, _sprite, _spriteColor, PassiveManager.AttackArea);
                
                StartCoroutine(AxeProjectileCoroutine(
                    newProjectile.transform, 
                    direction.normalized));

                yield return waitForSeconds;
            }
        }

        private IEnumerator AxeProjectileCoroutine(Transform projectileTransform, Vector2 direction)
        {
            var attackArea = ProjectileRadius;
            var currentSpeed = direction * LaunchSpeed;
            var accel = Acceleration;
            var currentPosition = PlayerPosition;
            var currentRotation = projectileTransform.localEulerAngles;
            var alreadyHit = new HashSet<EnemyHealth>();
            var hitCount = 0;

            for (var t = 0f; t < 5f || hitCount < maxHitCount; t+= Time.deltaTime)
            {
                currentPosition += currentSpeed * Time.deltaTime;
                currentSpeed += accel * Vector2.up;

                currentRotation.z += spinSpeed * Time.deltaTime;

                projectileTransform.localEulerAngles = currentRotation;
                projectileTransform.position = currentPosition;
                
                var enemiesInRange = EnemyManager.GetEnemiesInRange(currentPosition, attackArea, alreadyHit);
                if (enemiesInRange != null)
                {
                    foreach (var enemyHealth in enemiesInRange)
                    {
                        enemyHealth.ChangeHealth(-Damage);
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