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

        private float localScale;

        public CrossWeapon(in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            _projectileSprite = weaponProfile.projectileSprite;
            _spriteColor = weaponProfile.projectileSpriteColor;

            launchSpeed = weaponProfile.launchSpeed;
            acceleration = weaponProfile.acceleration;
            spinSpeed = weaponProfile.spinSpeed;
            
            projectileInterval = weaponProfile.projectileInterval;
            projectileRadius = weaponProfile.projectileRadius;
            
            localScale = 1f;
            
            OnScaleChanged(PassiveManager.AttackArea);
        }
        
        public override void LevelUp()
        {
            //Based on: https://vampire-survivors.fandom.com/wiki/Cross
            /*  Level 2	Base damage up by 10.
                Level 3	Base speed up by 25%. Base area up by 10%.
                Level 4	Fires 1 more projectile.
                Level 5	Base damage up by 10.
                Level 6	Base speed up by 25%. Base area up by 10%.
                Level 7	Fires 1 more projectile.
                Level 8	Base damage up by 10.
            */
            switch (Level)
            {
                case 2:
                case 5:
                case 8:
                    damage += 10;
                    break;
                case 3:
                    localScale = 1.1f;
                    launchSpeed = WeaponProfile.launchSpeed * 1.25f;
                    acceleration = WeaponProfile.acceleration * 1.25f;
                    spinSpeed = WeaponProfile.spinSpeed * 1.25f;

                    OnScaleChanged(PassiveManager.AttackArea);
                    break;
                case 4:
                case 7:
                    projectileCount++;
                    break;
                case 6:
                    localScale = 1.2f;
                    launchSpeed = WeaponProfile.launchSpeed * 1.5f;
                    acceleration = WeaponProfile.acceleration * 1.5f;
                    spinSpeed = WeaponProfile.spinSpeed * 1.5f;

                    OnScaleChanged(PassiveManager.AttackArea);
                    break;
            }
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

        public override string GetLevelUpText(in int nextLevel)
        {
            //Based on: https://vampire-survivors.fandom.com/wiki/Cross
            /*  Level 2	Base damage up by 10.
                Level 3	Base speed up by 25%. Base area up by 10%.
                Level 4	Fires 1 more projectile.
                Level 5	Base damage up by 10.
                Level 6	Base speed up by 25%. Base area up by 10%.
                Level 7	Fires 1 more projectile.
                Level 8	Base damage up by 10.
            */
            switch (nextLevel)
            {
                case 2:
                case 5:
                case 8:
                    return "Base damage up by 10.";
                case 3:
                case 6:
                    return "Base speed up by 25%. Base area up by 10%.";
                case 4:
                case 7:
                    return "Fires 1 more projectile.";
            }

            return "";
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
                    .CreateProjectile(PlayerPosition, _projectileSprite, _spriteColor, scale);
                
                StartCoroutine(CrossProjectileCoroutine(
                    newProjectile.transform, 
                    (closestPosition - PlayerPosition).normalized));

                yield return waitForSeconds;
            }
        }

        private IEnumerator CrossProjectileCoroutine(Transform projectileTransform, Vector2 direction)
        {
            var alreadyHit = new HashSet<EnemyHealth>();

            var radius = projectileRadius * scale;
            var currentSpeed = LaunchSpeed;
            var accel = Acceleration;
            
            var currentPosition = (Vector2)projectileTransform.position;
            var currentRotation = projectileTransform.localEulerAngles;

            for (var t = 0f; t < 5f; t+= Time.deltaTime)
            {
                currentPosition += direction * (currentSpeed * Time.deltaTime);
                currentSpeed += accel;

                currentRotation.z += spinSpeed * Time.deltaTime;

                projectileTransform.localEulerAngles = currentRotation;
                projectileTransform.position = currentPosition;
                
                var enemiesInRange = EnemyManager.GetEnemiesInRange(currentPosition, radius, alreadyHit);
                if (enemiesInRange != null)
                {
                    foreach (var enemyHealth in enemiesInRange)
                    {
                        enemyHealth.ChangeHealth(-Damage);
                        alreadyHit.Add(enemyHealth);
                        StartCoroutine(EnemyHitCooldownCoroutine(enemyHealth, 0.5f, alreadyHit));
                    }
                }

                yield return null;
            }
            
            Object.Destroy(projectileTransform.gameObject);
        }



    }
}