using System.Collections;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Weapons.Items;
using Survivors.Weapons.Interfaces;
using UnityEngine;

namespace Survivors.Weapons
{
    public class WhipWeapon : WeaponBase_v2, IUseProjectiles
    {
        private readonly Sprite projectileSprite;
        private readonly Color32 spriteColor;
        
        public int ProjectileCount => projectileCount + PassiveManager.ProjectileAdd;
        
        private float projectileInterval;
        private float projectileRadius;

        private readonly SpriteRenderer _playerSpriteRenderer;

        public WhipWeapon(in SpriteRenderer playerSpriteRenderer, in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            _playerSpriteRenderer = playerSpriteRenderer;
            projectileSprite = weaponProfile.projectileSprite;
            spriteColor = weaponProfile.projectileSpriteColor;

            localScale = 1f;
            
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

                var playerFaceRight = !_playerSpriteRenderer.flipX;
                var dir = playerFaceRight ? Vector2.right : Vector2.left;

                newProjectile.flipX = swap && !playerFaceRight;
                newProjectile.flipY = swap && playerFaceRight;

                projectileTransform.position = PlayerPosition + 
                                               dir * (scaleOffset * (swap ? 1 : -1))+
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