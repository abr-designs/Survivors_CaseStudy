using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks.Items;
using UnityEngine;

namespace Survivors.Weapons
{
    public class RadiusWeapon : WeaponBase_v2
    {
        private readonly Transform _effectInstanceTransform;
        
        public RadiusWeapon(in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            var projectileSprite = weaponProfile.projectileSprite;
            Color32 spriteColor = weaponProfile.projectileSpriteColor;
            
            _effectInstanceTransform = FactoryManager
                .GetFactory<ProjectileFactory>()
                .CreateProjectile(PlayerPosition, projectileSprite, spriteColor)
                .transform;
        }

        public override void PostUpdate()
        {
            _effectInstanceTransform.position = PlayerPosition;
        }

        protected override void TriggerAttack()
        {
            //TODO Find all enemies in Radius
            var enemies = EnemyManager.GetEnemiesInRange(PlayerPosition, range);

            if (enemies == null || enemies.Count == 0)
                return;

            foreach (var enemyHealth in enemies)
            {
                enemyHealth.ChangeHealth(-Damage);
            }
        }

        public override void LevelUp()
        {
            throw new System.NotImplementedException();
        }

        public override void OnScaleChanged(float newScale)
        {
            throw new System.NotImplementedException();
        }
    }
}