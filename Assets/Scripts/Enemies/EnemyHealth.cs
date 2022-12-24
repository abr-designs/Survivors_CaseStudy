using Survivors.Base;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : HealthBase
    {
        public override bool ShowHealthDamage => true;
        public override bool ShowHealthBar => false;
        public override bool ShowDamageEffect => true;
        
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