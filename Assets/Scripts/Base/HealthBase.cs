using System;
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

        public float MaxHealth => maxHealth;
        protected float maxHealth;
        public float CurrentHealth => currentHealth;
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

        protected virtual void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void OnDisable()
        {
            OnHealthRemoved?.Invoke(this);
        }

        //============================================================================================================//

        public void SetStartingHealth(in float startingHealth)
        {
            this.startingHealth = maxHealth = currentHealth = startingHealth;
        }
        
        public virtual void ChangeHealth(in float healthDelta)
        {
            currentHealth = Mathf.Clamp(currentHealth + healthDelta, 0, maxHealth);

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
