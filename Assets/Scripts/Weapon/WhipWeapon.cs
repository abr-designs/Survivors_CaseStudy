using System.Collections;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks.Items;
using Survivors.Weapons.Interfaces;
using UnityEngine;

namespace Survivors.Weapons
{
    public class WhipWeapon : WeaponBase_v2, IUseProjectiles
    {
        private readonly Sprite projectileSprite;
        private readonly Color32 spriteColor ;
        
        public int ProjectileCount => projectileCount + PassiveManager.ProjectileAdd;
        private int projectileCount = 1;
        
        private float projectileInterval;
        private float projectileRadius;

        private float localScale;
        
        public WhipWeapon(in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            projectileSprite = weaponProfile.projectileSprite;
            spriteColor = weaponProfile.projectileSpriteColor;

            localScale = 1f;
            
            OnScaleChanged(PassiveManager.AttackArea);
        }

        public override void LevelUp()
        {
            //Based on: https://vampire-survivors.fandom.com/wiki/Whip
            /*
                Level 2	Fires 1 more projectile.
                Level 3	Base damage up by 5.
                Level 4	Base damage up by 5. Base area up by 10%.
                Level 5	Base damage up by 5.
                Level 6	Base damage up by 5. Base area up by 10%.
                Level 7	Base damage up by 5.
                Level 8	Base damage up by 5.
             */
            switch (Level)
            {
                case 2:
                    projectileCount++;
                    break;
                case 3:
                case 5:
                case 7:
                case 8:
                    damage += 5;
                    break;
                case 4:
                    damage += 5;
                    localScale = 1.1f;
                    OnScaleChanged(PassiveManager.AttackArea);
                    break;
                case 6:
                    damage += 5;
                    localScale = 1.2f;
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
            //Based on: https://vampire-survivors.fandom.com/wiki/Whip
            /*
                Level 2	Fires 1 more projectile.
                Level 3	Base damage up by 5.
                Level 4	Base damage up by 5. Base area up by 10%.
                Level 5	Base damage up by 5.
                Level 6	Base damage up by 5. Base area up by 10%.
                Level 7	Base damage up by 5.
                Level 8	Base damage up by 5.
             */
            switch (nextLevel)
            {
                case 2:
                    return "Fires 1 more projectile.";
                case 3:
                    return "Base damage up by 5.";
                case 4:
                    return "Base damage up by 5. Base area up by 10%.";
                case 5:
                    return "Base damage up by 5.";
                case 6:
                    return "Base damage up by 5. Base area up by 10%.";
                case 7:
                    return "Base damage up by 5.";
                case 8:
                    return "Base damage up by 5.";
            }

            return "";
        }

        //TODO Might want to use a stored bounds value instead of Sprite Bounds so I can scale it for animations
        private IEnumerator AttackCoroutine()
        {
            var waitForSeconds = new WaitForSeconds(projectileInterval);
            bool swap = true;
            float height = 0f;

            for (int i = 0; i < ProjectileCount; i++)
            {
                var newProjectile = FactoryManager
                    .GetFactory<ProjectileFactory>()
                    .CreateProjectile(PlayerPosition, projectileSprite, spriteColor, scale);

                var projectileTransform = newProjectile.transform;
                projectileTransform.localScale = Vector3.one * PassiveManager.AttackArea;
                
                var scaleOffset = projectileTransform.localScale.x / 2f;
                
                newProjectile.flipX = swap;
                newProjectile.flipY = swap;
                projectileTransform.position = PlayerPosition + 
                                               Vector2.right * (scaleOffset * (swap ? -1 : 1))+
                                               Vector2.up * height;

                yield return StartCoroutine(ProjectileCoroutine(newProjectile, 0.3f));

                yield return waitForSeconds;

                if (swap == false)
                    height += scaleOffset / 2f;
                
                swap = !swap;
            }
        }

        private IEnumerator ProjectileCoroutine(SpriteRenderer spriteRenderer, float lifetime)
        {
            var enemies = EnemyManager.GetEnemiesInBounds(spriteRenderer.bounds);

            if (enemies != null)
            {
                foreach (var enemyHealth in enemies)
                {
                    enemyHealth.ChangeHealth(-Damage);
                }
            }

            yield return new WaitForSeconds(lifetime);
            
            Object.Destroy(spriteRenderer.gameObject);
        }
    }
}