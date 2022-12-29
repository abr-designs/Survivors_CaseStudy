using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks.Items;
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
            range = weaponProfile.range;
            
            var projectileSprite = weaponProfile.projectileSprite;
            Color32 spriteColor = weaponProfile.projectileSpriteColor;
            
            _effectInstanceTransform = FactoryManager
                .GetFactory<ProjectileFactory>()
                .CreateProjectile(PlayerPosition, projectileSprite, spriteColor)
                .transform;

            _hitEnemies = new HashSet<EnemyHealth>();
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

        public override string GetLevelUpText(in int nextLevel)
        {
            //based on: https://vampire-survivors.fandom.com/wiki/Garlic
            /*
                Level 2	Base area up by 40%. Base damage up by 2.
                Level 3	Cooldown reduced by 0.1 seconds. Base damage up by 1.
                Level 4	Base area up by 20%. Base damage up by 1.
                Level 5	Cooldown reduced by 0.1 seconds. Base damage up by 2.
                Level 6	Base area up by 20%. Base damage up by 1.
                Level 7	Cooldown reduced by 0.1 seconds. Base damage up by 1.
                Level 8	Base area up by 20%. Base damage up by 1.
             */
            switch (nextLevel)
            {
                case 2:
                    return "Base area up by 40%. Base damage up by 2";
                case 3:
                    return "Cooldown reduced by 0.1 seconds. Base damage up by 1";
                case 4:
                    return "Base area up by 20%. Base damage up by 1";
                case 5:
                    return "Cooldown reduced by 0.1 seconds. Base damage up by 2";
                case 6:
                    return "Base area up by 20%. Base damage up by 1";
                case 7:
                    return "Cooldown reduced by 0.1 seconds. Base damage up by 1";
                case 8:
                    return "Base area up by 20%. Base damage up by 1";
            }

            return "";
        }

        public override void LevelUp()
        {
            //based on: https://vampire-survivors.fandom.com/wiki/Garlic
            /*
                Level 2	Base area up by 40%. Base damage up by 2.
                Level 3	Cooldown reduced by 0.1 seconds. Base damage up by 1.
                Level 4	Base area up by 20%. Base damage up by 1.
                Level 5	Cooldown reduced by 0.1 seconds. Base damage up by 2.
                Level 6	Base area up by 20%. Base damage up by 1.
                Level 7	Cooldown reduced by 0.1 seconds. Base damage up by 1.
                Level 8	Base area up by 20%. Base damage up by 1.
             */
            switch (Level)
            {
                case 2:
                    range = WeaponProfile.range * 1.4f;
                    damage += 2;
                    break;
                case 3:
                    cooldown -= 0.1f;
                    damage++;
                    break;
                case 4:
                    range = WeaponProfile.range * 1.6f;
                    damage++;
                    break;
                case 5:
                    cooldown -= 0.1f;
                    damage+=2;
                    break;
                case 6:
                    range = WeaponProfile.range * 1.8f;
                    damage++;
                    break;
                case 7:
                    cooldown -= 0.1f;
                    damage++;
                    break;
                case 8:
                    range = WeaponProfile.range * 2f;
                    damage++;
                    break;
            }
        }

        public override void OnScaleChanged(float newScale)
        {
            scale = newScale;
            _effectInstanceTransform.localScale = Vector3.one * scale;
        }
    }
}