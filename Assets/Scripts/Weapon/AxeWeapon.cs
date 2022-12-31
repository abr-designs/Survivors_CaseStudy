using System;
using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Weapons.Items;
using Survivors.Weapons.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Survivors.Weapons
{
    public class AxeWeapon : WeaponBase_v2, IUseProjectiles, IUserProjectileSpeed, IUseProjectileRadius
    {
        private readonly Sprite _sprite;
        private readonly Color32 _spriteColor;
        
        public int ProjectileCount => projectileCount + PassiveManager.ProjectileAdd;

        public float LaunchSpeed => launchSpeed * PassiveManager.ProjectileSpeed;
        public float Acceleration => acceleration * PassiveManager.ProjectileSpeed;

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
            
            OnScaleChanged(PassiveManager.AttackArea);
        }

        public override void OnScaleChanged(float newScale)
        {
            scale = newScale * localScale;
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
            var alreadyHit = new HashSet<EnemyHealth>();

            var attackArea = ProjectileRadius * scale;
            var currentSpeed = direction * LaunchSpeed;
            var accel = Acceleration;
            var currentPosition = PlayerPosition;
            var currentRotation = projectileTransform.localEulerAngles;
            var hitCount = 0;

            for (var t = 0f; t < 5f; t+= Time.deltaTime)
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
                        if (hitCount >= maxHitCount)
                        {
                            t = 999;
                            break;
                        }
                        
                        StartCoroutine(EnemyHitCooldownCoroutine(enemyHealth, 0.5f, alreadyHit));
                    }
                }

                yield return null;
            }
            
            Object.Destroy(projectileTransform.gameObject);
        }
        
        

    }
}