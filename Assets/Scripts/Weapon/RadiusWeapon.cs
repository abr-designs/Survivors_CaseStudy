using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Weapons.Items;
using UnityEngine;

namespace Survivors.Weapons
{
    public class RadiusWeapon : WeaponBase_v2
    {
        private readonly Transform _effectInstanceTransform;
        private float range;
        
        private HashSet<EnemyHealth> _hitEnemies;

        public RadiusWeapon(in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            var projectileSprite = weaponProfile.projectileSprite;
            Color32 spriteColor = weaponProfile.projectileSpriteColor;
            
            _effectInstanceTransform = FactoryManager
                .GetFactory<ProjectileFactory>()
                .CreateProjectile(PlayerPosition, projectileSprite, spriteColor)
                .transform;

            _hitEnemies = new HashSet<EnemyHealth>();

            OnScaleChanged(PassiveManager.AttackArea);
        }

        public override void PostUpdate()
        {
            _effectInstanceTransform.position = PlayerPosition;
            
            //TODO Find all enemies in Radius
            var enemies = EnemyManager.GetEnemiesInRange(PlayerPosition, range * scale, _hitEnemies);

            if (enemies == null || enemies.Count == 0)
                return;

            foreach (var enemyHealth in enemies)
            {
                enemyHealth.ChangeHealth(-Damage);
                StartCoroutine(EnemyHitCooldownCoroutine(enemyHealth, cooldown, _hitEnemies));
            }
        }

        
        protected override void TriggerAttack()
        { }
        

        public override void OnScaleChanged(float newScale)
        {
            scale = localScale * newScale;
            _effectInstanceTransform.localScale = Vector3.one * scale;
            range = WeaponProfile.range * scale;
        }
    }
}