using System;
using System.Collections;
using System.Collections.Generic;
using Survivors.Base.Interfaces;
using Survivors.Managers;
using Survivors.Utilities;
using UnityEngine;

namespace Survivors.Base
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class HealthBase : MonoBehaviour, IHealth
    {
        public static event Action<IHealth> OnNewHealth;
        public static event Action<IHealth> OnHealthRemoved;

        public float StartingHealth => startingHealth;
        [SerializeField]
        private float startingHealth;
        public float MaxHealth { get; protected set; }
        public float CurrentHealth => currentHealth;
        [SerializeField]
        private float currentHealth;

        protected abstract float DamageFlashTime { get; }

        public abstract bool ShowHealthDamage { get; }
        public abstract bool ShowHealthBar { get; }
        public abstract bool ShowDamageEffect { get; }

        private SpriteRenderer _spriteRenderer;

        //============================================================================================================//

        //FIXME This should be using the observer pattern.
        protected virtual void OnEnable()
        {
            OnNewHealth?.Invoke(this);
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void OnDisable()
        {
            OnHealthRemoved?.Invoke(this);
        }

        //============================================================================================================//

        public void SetHealth(in float health, in bool setStarting = false)
        {
            if (setStarting)
                MaxHealth = startingHealth = health;
            
            currentHealth = health;
        }
        
        public virtual void ChangeHealth(in float healthDelta)
        {
            currentHealth += healthDelta;

            if(ShowHealthDamage &&  healthDelta < 0f)
                DamageTextManager.CreateText((int)healthDelta, transform.position);
            
            if (currentHealth <= 0f)
            {
                Kill();
                return;
            }
            
            if(healthDelta < 0f)
                DamageAnimator.Play(_spriteRenderer, DamageFlashTime);
        }

        public virtual void Kill()
        {
            //TODO Maybe do a death animation?
            
            Destroy(gameObject);
        }
        //============================================================================================================//
    }
}
