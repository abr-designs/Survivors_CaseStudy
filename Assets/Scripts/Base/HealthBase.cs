using System;
using System.Collections;
using System.Collections.Generic;
using Survivors.Base.Interfaces;
using Survivors.Managers;
using UnityEngine;

namespace Survivors.Base
{
    public abstract class HealthBase : MonoBehaviour, IHealth
    {
        public static event Action<IHealth> OnNewHealth;
        public static event Action<IHealth> OnHealthRemoved;

        public float StartingHealth => startingHealth;
        [SerializeField]
        private float startingHealth;
        public float CurrentHealth => currentHealth;
        [SerializeField]
        private float currentHealth;
        
        public abstract bool ShowHealthDamage { get; }
        public abstract bool ShowHealthBar { get; }
        public abstract bool ShowDamageEffect { get; }

        //============================================================================================================//

        //FIXME This should be using the observer pattern.
        private void OnEnable()
        {
            OnNewHealth?.Invoke(this);
        }

        private void OnDisable()
        {
            OnHealthRemoved?.Invoke(this);
        }

        //============================================================================================================//

        public void SetHealth(in float health, in bool setStarting = false)
        {
            if (setStarting)
                startingHealth = health;
            
            currentHealth = health;
        }
        
        public virtual void ChangeHealth(in float healthDelta)
        {
            currentHealth += healthDelta;
            
            if(ShowHealthDamage &&  healthDelta < 0f)
                DamageTextManager.CreateText((int)healthDelta, transform.position);
            
        }

        public virtual void Kill()
        {
            //TODO Maybe do a death animation?
            
            Destroy(gameObject);
        }
        //============================================================================================================//
    }
}
