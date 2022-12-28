using System.Collections;
using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Attacks
{
    public class WhipAttack : AttackBase_v2
    {
        private readonly SpriteRenderer _effectInstance;
        private readonly Transform _effectInstanceTransform;
        
        private int projectileCount = 1;
        
        private float projectileInterval;
        private float projectileRadius;
        
        public WhipAttack(in AttackProfileScriptableObject attackProfile) : base(in attackProfile)
        {
            var sprite = attackProfile.sprite;
            Color32 spriteColor = attackProfile.spriteColor;
            
            _effectInstance = FactoryManager
                .GetFactory<ProjectileFactory>()
                .CreateProjectile(PlayerPosition, sprite, spriteColor);

            _effectInstanceTransform = _effectInstance.transform;
        }

        protected override void LevelUp()
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
            for (int i = 0; i < projectileCount; i++)
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
                        enemyHealth.ChangeHealth(-damage);
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