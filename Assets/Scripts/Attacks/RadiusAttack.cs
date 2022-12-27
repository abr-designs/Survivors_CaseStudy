using Survivors.Managers;
using UnityEngine;

namespace Survivors.Attacks
{
    public class RadiusAttack : AttackBase
    {
        //TODO Probably should move this to a Factory
        [SerializeField, Header("Radius Attack")]
        private GameObject effectPrefab;

        private Transform _effectInstanceTransform;

        public override void Setup()
        {
            if (_effectInstanceTransform)
                return;
            
            _effectInstanceTransform = Instantiate(effectPrefab, transform, false).transform;
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

        public override void SetActive(in bool state)
        {
            _effectInstanceTransform.gameObject.SetActive(state);
            base.SetActive(in state);
        }
    }
}