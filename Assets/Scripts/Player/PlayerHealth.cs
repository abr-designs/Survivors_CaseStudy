using System;
using System.Collections;
using System.Collections.Generic;
using Survivors.Base;
using Survivors.Enemies;
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

        protected override void Start()
        {
            base.Start();
            
            SetStartingHealth(StartingHealth);
            _hitEnemies = new HashSet<EnemyHealth>();
        }

        private HashSet<EnemyHealth> _hitEnemies;
        private void Update()
        {
            var hitEnemies = EnemyManager.GetEnemiesInRange(transform.position, 0.125f, _hitEnemies);
            
            if (hitEnemies.Count == 0)
                return;

            float healthChangeSum = 0;
            foreach (var enemyHealth in hitEnemies)
            {
                healthChangeSum -= enemyHealth.Damage;
                StartCoroutine(EnemyHitCooldownCoroutine(enemyHealth, 0.5f, _hitEnemies));
            }

            ChangeHealth(healthChangeSum);
        }
        protected static IEnumerator EnemyHitCooldownCoroutine(EnemyHealth enemy, float hitCooldown, HashSet<EnemyHealth> hitEnemies)
        {
            hitEnemies.Add(enemy);
            
            yield return new WaitForSeconds(hitCooldown);

            if(enemy == null)
                yield break;
            
            hitEnemies.Remove(enemy);
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
            maxHealth = StartingHealth * mult;
        }
    }
}