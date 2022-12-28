using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Weapons
{
    public class RadiusWeapon : WeaponBase_v2
    {
        private readonly Transform _effectInstanceTransform;
        
        public RadiusWeapon(in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            var sprite = weaponProfile.sprite;
            Color32 spriteColor = weaponProfile.spriteColor;
            
            _effectInstanceTransform = FactoryManager
                .GetFactory<ProjectileFactory>()
                .CreateProjectile(PlayerPosition, sprite, spriteColor)
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

        protected override void LevelUp()
        {
            throw new System.NotImplementedException();
        }
    }
}