using System;
using System.Collections;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Managers;
using Survivors.ScriptableObjets.Weapons.Items;
using UnityEngine;

namespace Survivors.Weapons
{
    public abstract class WeaponBase_v2
    {
        internal static MonoBehaviour CoroutineController;
        internal static Vector2 PlayerPosition;

        protected float Damage => damage * PassiveManager.Damage;

        public int Level { get; set; } = 1;

        protected float scale;
        protected float damage;

        protected float localScale;
        protected int maxHitCount;
        protected int projectileCount;
        
        protected float launchSpeed;
        protected float acceleration;
        protected float spinSpeed;

        protected float cooldown;
        private float _cooldownTimer;

        protected readonly WeaponProfileScriptableObject WeaponProfile;

        //============================================================================================================//
        internal WeaponBase_v2(in WeaponProfileScriptableObject weaponProfile)
        {
            localScale = 1f;
            
            Level = 1;
            WeaponProfile = weaponProfile;
            
            scale = 1f;
            
            damage = weaponProfile.damage;
            cooldown = weaponProfile.cooldown;

            maxHitCount = weaponProfile.maxHitCount;
            projectileCount = weaponProfile.projectileCount;
            
            launchSpeed = weaponProfile.launchSpeed;
            acceleration = weaponProfile.acceleration;
            spinSpeed = weaponProfile.spinSpeed;
        }

        public void LevelUp()
        {
            bool shouldUpdateScale = false;
            var changes = WeaponProfile.GetLevelUpChanges(Level);

            foreach (var levelStat in changes)
            {
                switch (levelStat.type)
                {
                    case STAT_TYPE.DAMAGE:
                        damage += levelStat.change;
                        break;
                    case STAT_TYPE.PROJECTILE:
                        projectileCount += (int)levelStat.change;
                        break;
                    case STAT_TYPE.AREA:
                        localScale += levelStat.change;
                        shouldUpdateScale = true;
                        break;
                    case STAT_TYPE.COOLDOWN:
                        cooldown -= levelStat.change;
                        break;
                    case STAT_TYPE.PASSTHROUGH:
                        maxHitCount += (int)levelStat.change;
                        break;
                    case STAT_TYPE.SPEED:
                        var increase = 1f + levelStat.change;
                        launchSpeed = WeaponProfile.launchSpeed * increase;
                        acceleration = WeaponProfile.acceleration * increase;
                        spinSpeed = WeaponProfile.spinSpeed * increase;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            if(shouldUpdateScale)
                OnScaleChanged(PassiveManager.AttackArea);
        }
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
        protected static Coroutine StartCoroutine(in IEnumerator coroutine) => CoroutineController.StartCoroutine(coroutine);
        
        protected static IEnumerator EnemyHitCooldownCoroutine(EnemyHealth enemy, float hitCooldown, HashSet<EnemyHealth> hitEnemies)
        {
            hitEnemies.Add(enemy);
            
            yield return new WaitForSeconds(hitCooldown);

            if(enemy == null)
                yield break;
            
            hitEnemies.Remove(enemy);
        }
    }
}