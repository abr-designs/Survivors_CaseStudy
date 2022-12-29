using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks.Items;
using UnityEngine;

namespace Survivors.Weapons
{
    public abstract class WeaponBase_v2
    {
        internal static MonoBehaviour CoroutineController;
        internal static Vector2 PlayerPosition;

        protected float Damage => damage * PassiveManager.Damage;

        public int Level { get; private set; } = 1;

        protected float scale;
        protected float damage;

        protected float cooldown;
        private float _cooldownTimer;

        protected readonly WeaponProfileScriptableObject WeaponProfile;

        //============================================================================================================//
        internal WeaponBase_v2(in WeaponProfileScriptableObject weaponProfile)
        {
            WeaponProfile = weaponProfile;
            
            scale = 1f;
            
            damage = weaponProfile.damage;
            cooldown = weaponProfile.cooldown;
        }
        
        public abstract void LevelUp();
        public abstract void OnScaleChanged(float newScale);

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

        public abstract string GetLevelUpText(in int nextLevel);

        protected static Coroutine StartCoroutine(in IEnumerator coroutine) => CoroutineController.StartCoroutine(coroutine);
        
        protected static IEnumerator EnemyHitCooldownCoroutine(EnemyHealth enemy, float hitCooldown, HashSet<EnemyHealth> hitEnemies)
        {
            yield return new WaitForSeconds(hitCooldown);

            if(enemy == null)
                yield break;
            
            hitEnemies.Remove(enemy);
        }
    }
}