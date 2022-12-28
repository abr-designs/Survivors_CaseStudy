using Survivors.Base;
using Survivors.Managers;
using UnityEngine;

namespace Survivors.Player
{
    public class PlayerHealth : HealthBase
    {
        protected override float DamageFlashTime => 0.5f;
        public override bool ShowHealthDamage => false;
        public override bool ShowHealthBar => true;
        public override bool ShowDamageEffect => true;

        //Unity Functions
        //============================================================================================================//

        protected override void OnEnable()
        {
            base.OnEnable();
            
            PassiveManager.OnMaxHealthChanged += OnMaxHealthChanged;
        }



        protected override void OnDisable()
        {
            base.OnDisable();
        }

        //Health Base Functions
        //============================================================================================================//
        

        public override void ChangeHealth(in float healthDelta)
        {
            base.ChangeHealth(healthDelta - PassiveManager.DamageReduction);
            
            //TODO Add health change VFX
        }

        public override void Kill()
        {
            Debug.LogError("PLAYER DIED");
        }

        //Callbacks
        //============================================================================================================//
        
        private void OnMaxHealthChanged(float mult)
        {
            MaxHealth = StartingHealth * mult;
        }
    }
}