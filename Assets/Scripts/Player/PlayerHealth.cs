﻿using Survivors.Base;

namespace Survivors.Player
{
    public class PlayerHealth : HealthBase
    {
        public override bool ShowHealthDamage => false;
        public override bool ShowHealthBar => true;
        public override bool ShowDamageEffect => true;

        public override void ChangeHealth(in float healthDelta)
        {
            base.ChangeHealth(in healthDelta);
            
            //TODO Add health change VFX
        }
    }
}