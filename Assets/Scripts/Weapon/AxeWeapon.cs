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
            
            OnScaleChanged(PassiveManager.AttackArea);
        }

        public override void LevelUp()
        {
            switch (Level)
            {
                case 2:
                case 5:
                    projectileCount++;
                    break;
                case 3:
                case 6:
                case 8:
                    damage += 20;
                    break;
                case 4:
                case 7:
                    maxHitCount += 2;
                    break;
            }
        }

        public override void OnScaleChanged(float newScale)
        {
            scale = newScale;
        }

        public override void PostUpdate()
        {

        }

        protected override void TriggerAttack()
        {
            StartCoroutine(AttackCoroutine());
        }

        public override string GetLevelUpText(in int nextLevel)
        {
            //Based on: https://vampire-survivors.fandom.com/wiki/Axe
            switch (nextLevel)
            {
                case 2:
                case 5:
                    return "Fires 1 more projectile.";
                case 3:
                case 6:
                case 8:
                    return "Base damage up by 20.";
                case 4:
                case 7:
                    return "Passes through 2 more enemies.";

            }

            return "";
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
                        
                        StartCoroutine(EnemyHitCooldownCoroutine(enemyHealth, 0.5f, alreadyHit));
                    }
                }

                yield return null;
            }
            
            Object.Destroy(projectileTransform.gameObject);
        }
        
        

    }
}