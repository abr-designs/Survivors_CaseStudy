using System;
using Survivors.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Enemies
{
    public class EnemyHealth : HealthBase
    {
        public static event Action<EnemyHealth> OnNewEnemy; 
        public static event Action<EnemyHealth> OnEnemyRemoved; 
        
        protected override float DamageFlashTime => 0.1f;
        
        public override bool ShowHealthDamage => true;
        public override bool ShowHealthBar => false;
        public override bool ShowDamageEffect => true;

        //Unity Functions
        //============================================================================================================//
        
        protected override void OnEnable()
        {
            OnNewEnemy?.Invoke(this);
        }

        protected override void OnDisable()
        {
            OnEnemyRemoved?.Invoke(this);
        }
        
        //============================================================================================================//
        
        public override void ChangeHealth(in float healthDelta)
        {
            base.ChangeHealth(in healthDelta);
            
            //TODO Add health change VFX
        }

        //============================================================================================================//
#if UNITY_EDITOR

        [ContextMenu("Damage Test")]
        private void TestDamage()
        {
            ChangeHealth(-Random.Range(0f, 10f));
        }
        
#endif
    }
}