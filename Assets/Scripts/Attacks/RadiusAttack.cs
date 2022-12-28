using Survivors.Factories;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Attacks
{
    public class RadiusAttack : AttackBase_v2
    {
        private readonly Transform _effectInstanceTransform;
        
        public RadiusAttack(in AttackProfileScriptableObject attackProfile) : base(in attackProfile)
        {
            var sprite = attackProfile.sprite;
            Color32 spriteColor = attackProfile.spriteColor;
            
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
                enemyHealth.ChangeHealth(-damage);
            }
        }

        protected override void LevelUp()
        {
            throw new System.NotImplementedException();
        }
    }
}