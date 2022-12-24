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
            HealthManager.AddTrackedHealth(this);
        }

        private void OnDisable()
        {
            HealthManager.RemoveTrackedHealth(this);
        }

        //============================================================================================================//
        
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
