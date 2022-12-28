using System.Collections;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Weapons
{
    public abstract class WeaponBase_v2
    {
        internal static MonoBehaviour CoroutineController;
        internal static Vector2 PlayerPosition;

        protected float Damage => damage * PassiveManager.Damage;
        
        private int _level = 1;
        
        protected float range;
        private float damage;

        private float cooldown;
        private float _cooldownTimer;

        //============================================================================================================//
        internal WeaponBase_v2(in WeaponProfileScriptableObject weaponProfile)
        {
            range = weaponProfile.range;
            damage = weaponProfile.damage;
            cooldown = weaponProfile.cooldown;
        }
        
        protected abstract void LevelUp();

        public void ManualUpdate(in float deltaTime)
        {
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= deltaTime;
                return;
            }

            TriggerAttack();
            _cooldownTimer = cooldown * PassiveManager.Cooldown;
        }

        public abstract void PostUpdate();

        protected abstract void TriggerAttack();

        protected static Coroutine StartCoroutine(in IEnumerator coroutine) => CoroutineController.StartCoroutine(coroutine);
    }
}