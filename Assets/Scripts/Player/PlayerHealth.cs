using Survivors.Base;
using UnityEngine;

namespace Survivors.Player
{
    public class PlayerHealth : HealthBase
    {
        protected override float DamageFlashTime => 0.5f;
        public override bool ShowHealthDamage => false;
        public override bool ShowHealthBar => true;
        public override bool ShowDamageEffect => true;

        public override void ChangeHealth(in float healthDelta)
        {
            base.ChangeHealth(in healthDelta);
            
            //TODO Add health change VFX
        }

        public override void Kill()
        {
            Debug.LogError("PLAYER DIED");
        }
    }
}