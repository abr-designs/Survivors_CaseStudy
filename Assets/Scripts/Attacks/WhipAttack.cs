using System.Collections;
using Survivors.Managers;
using UnityEngine;

namespace Survivors.Attacks
{
    public class WhipAttack : AttackBase
    {
        [SerializeField, Header("Whip Effect")]
        private SpriteRenderer effectPrefab;
        private SpriteRenderer _effectInstance;
        private Transform _effectInstanceTransform;

        [SerializeField, Min(1), Header("Projectiles")]
        private int projectileCount = 1;
        [SerializeField, Min(0)]
        private float projectileInterval;
        
        public override void Setup()
        {
            _effectInstance = Instantiate(effectPrefab, transform, false);
            _effectInstanceTransform = _effectInstance.transform;
            
            projectileCount = 1;
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

        public override void SetActive(in bool state)
        {
            _effectInstance.gameObject.SetActive(state);
            base.SetActive(in state);
        }
    }
}