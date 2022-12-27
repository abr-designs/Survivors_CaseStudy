using Survivors.Base;
using UnityEngine;

namespace Survivors.Enemies
{
    public class EnemyHealth : HealthBase
    {
        protected override float DamageFlashTime => 0.1f;
        
        public override bool ShowHealthDamage => true;
        public override bool ShowHealthBar => false;
        public override bool ShowDamageEffect => true;
        
        private void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.Space) && Random.value < 0.5f)
                ChangeHealth(-Random.Range(0, 10f));
        }
        
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