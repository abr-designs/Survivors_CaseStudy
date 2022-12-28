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
        private readonly SpriteRenderer _effectInstance;
        private readonly Transform _effectInstanceTransform;

        public int ProjectileCount => projectileCount + PassiveManager.ProjectileAdd;
        private int projectileCount = 1;
        
        private float projectileInterval;
        private float projectileRadius;
        
        public WhipWeapon(in WeaponProfileScriptableObject weaponProfile) : base(in weaponProfile)
        {
            var sprite = weaponProfile.sprite;
            Color32 spriteColor = weaponProfile.spriteColor;
            
            _effectInstance = FactoryManager
                .GetFactory<ProjectileFactory>()
                .CreateProjectile(PlayerPosition, sprite, spriteColor);

            _effectInstanceTransform = _effectInstance.transform;
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

        //TODO Might want to use a stored bounds value instead of Sprite Bounds so I can scale it for animations
        private IEnumerator AttackCoroutine()
        {
            var scaleOffset = _effectInstanceTransform.localScale.x / 2f;
            var waitForSeconds = new WaitForSeconds(projectileInterval);
            bool swap = true;
            float height = 0f;

            _effectInstance.gameObject.SetActive(true);
            for (int i = 0; i < ProjectileCount; i++)
            {
                _effectInstance.flipX = swap;
                _effectInstance.flipY = swap;
                _effectInstanceTransform.position = PlayerPosition + 
                                                    Vector2.right * (scaleOffset * (swap ? -1 : 1))+
                                                    Vector2.up * height;

                var enemies = EnemyManager.GetEnemiesInBounds(_effectInstance.bounds);

                if (enemies != null)
                {
                    foreach (var enemyHealth in enemies)
                    {
                        enemyHealth.ChangeHealth(-Damage);
                    }
                }


                yield return waitForSeconds;

                if (swap == false)
                    height += scaleOffset / 2f;
                
                swap = !swap;
            }
            _effectInstance.gameObject.SetActive(false);
        }
    }
}